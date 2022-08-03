using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using YouYou;

public class RoleCtrl : BaseSprite, IUpdateComponent
{
    /// <summary>
    /// 皮肤编号
    /// </summary>
    [SerializeField] private int m_SkinId = 0;

    /// <summary>
    /// 当前的皮肤
    /// </summary>
    private Transform m_CurrSkinTransform;

    /// <summary>
    /// 当前皮肤的MeshRenderer
    /// </summary>
    private SkinnedMeshRenderer m_CurrSkinnedMeshRenderer;

    /// <summary>
    ///  网格合并的皮肤SkinnedMeshRenderer, 作为一种换装方式
    /// </summary>
    private RoleSkinComponent m_CurrRoleSkinComponent;

    #region 动画相关

    /// <summary>
    /// 动画片段字典
    /// </summary>
    private Dictionary<string, AnimationClip> m_AnimationClipDic;

    /// <summary>
    /// 动画画布
    /// </summary>
    private PlayableGraph m_PlayableGraph;

    /// <summary>
    /// 动画组件
    /// </summary>
    private Animator m_Animator;

    /// <summary>
    /// 动画输出
    /// </summary>
    private AnimationPlayableOutput m_AnimationPlayableOutput;

    /// <summary>
    /// 动画混合Playable
    /// </summary>
    private AnimationMixerPlayable m_AnimationMixerPlayable;

    /// <summary>
    /// 动画剪辑Playable字典
    /// </summary>
    private Dictionary<int, RoleAnimInfo> m_RoleAnimInfoDic = new Dictionary<int, RoleAnimInfo>(100);

    #endregion

    /// <summary>
    /// 当前角色对应表格数据
    /// </summary>
    private DTRoleEntity m_CurrDTRole;

    /// <summary>
    /// 当前角色动画分类数据
    /// </summary>
    private DTRoleAnimCategoryEntity m_CurrDTRoleAnimCategory;

    /// <summary>
    /// 当前角色状态机管理器
    /// </summary>
    private RoleFsmManager m_CurrRoleFsmManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_AnimationClipDic = new Dictionary<string, AnimationClip>();
        m_CurrRoleFsmManager = new RoleFsmManager(this);
    }

    protected override void OnBeforDestroy()
    {
        base.OnBeforDestroy();
        //销毁画布
        if (m_PlayableGraph.IsValid())
        {
            m_PlayableGraph.Destroy();
        }
    }

    /// <summary>
    /// 切换皮肤
    /// </summary>
    /// <param name="skinId"></param>
    private void ChangeSkin(int skinId)
    {
        if (m_SkinId == skinId)
        {
            return;
        }

        m_SkinId = skinId;
        LoadSkin();
    }


    public void Init(int roleId)
    {
        m_CurrDTRole = GameEntry.DataTable.RoleList.Get(roleId);
        m_CurrDTRoleAnimCategory = GameEntry.DataTable.RoleAnimCategoryList.Get(roleId);
        m_SkinId = m_CurrDTRole.PrefabId;

        //初始化状态机
        m_CurrRoleFsmManager.Init();
    }

    /// <summary>
    /// 加载初始角色动画
    /// </summary>
    /// <param name="animGroupId"></param>
    private void LoadInitRoleAnimations(int animGroupId)
    {
        m_RoleAnimInfoDic.Clear();

        //根据动画组编号, 加载动画
        List<DTRoleAnimationEntity> roleAnimations =
            GameEntry.DataTable.RoleAnimationList.GetListByGroupId(animGroupId);
        int lenRoleAnimations = roleAnimations.Count;
        for (int i = 0; i < lenRoleAnimations; i++)
        {
            DTRoleAnimationEntity roleAnimation = roleAnimations[i];
            m_RoleAnimInfoDic.Add(roleAnimation.Id, new RoleAnimInfo()
            {
                CurrRoleAnimationData = roleAnimation,
                IsLoad = false,
                LastUseTime = 0,
                Index = i
            });

            if (roleAnimation.InitLoad == 1)
            {
                LoadRoleAnimation(roleAnimation);
            }
        }
    }

    /// <summary>
    /// 加载角色动画
    /// </summary>
    /// <param name="roleAnimation"></param>
    private void LoadRoleAnimation(DTRoleAnimationEntity roleAnimation, BaseAction<RoleAnimInfo> onComplete = null)
    {
        GameEntry.Resource.ResourceLoaderManager.LoadMainAsset(AssetCategory.RoleSources,
            GameUtil.GetRoleAnimationPath(roleAnimation.AnimPath), (
                entity =>
                {
                    var animationClip = entity.Target as AnimationClip;
                    m_AnimationClipDic[roleAnimation.AnimPath] = animationClip;

                    Debug.LogError($"角色动画路径===> {roleAnimation.AnimPath} :=====: 动画片段==>{roleAnimation}");

                    //创建AnimationClipPlayable
                    AnimationClipPlayable animationClipPlayable =
                        AnimationClipPlayable.Create(m_PlayableGraph, animationClip);

                    if (m_RoleAnimInfoDic.TryGetValue(roleAnimation.Id, out var roleAnimInfo))
                    {
                        roleAnimInfo.CurrPlayable = animationClipPlayable;
                        roleAnimInfo.IsLoad = true; //当前动画已经加载
                        roleAnimInfo.LastUseTime = 0;

                        //链接到MixerPlayable
                        m_PlayableGraph.Connect(animationClipPlayable, 0, m_AnimationMixerPlayable, roleAnimInfo.Index);
                        //把权重设置为0
                        m_AnimationMixerPlayable.SetInputWeight(roleAnimInfo.Index, 0);

                        onComplete?.Invoke(roleAnimInfo);
                    }
                }));
    }

    /// <summary>
    /// 根据动画分类进行播放
    /// </summary>
    /// <param name="roleAnimCategory"></param>
    /// <returns></returns>
    public RoleAnimInfo PlayAnimByAnimCategory(MyCommonEnum.RoleAnimCategory roleAnimCategory)
    {
        int animId = -1;
        switch (roleAnimCategory)
        {
            default:
            case MyCommonEnum.RoleAnimCategory.IdleNormal:
                animId = m_CurrDTRoleAnimCategory.IdleNormalAnimId;
                break;
            case MyCommonEnum.RoleAnimCategory.Run:
                animId = m_CurrDTRoleAnimCategory.RunAnimId;
                break;
            case MyCommonEnum.RoleAnimCategory.Attack:
                animId = m_CurrDTRoleAnimCategory.Attack_1;
                break;
        }

        RoleAnimInfo roleAnimInfo = null;
        PlayAnimByAnimId(animId, ref roleAnimInfo);
        return roleAnimInfo;
    }

    /// <summary>
    /// 根据动画编号播放动画
    /// </summary>
    /// <param name="animId"></param>
    private void PlayAnimByAnimId(int animId, ref RoleAnimInfo roleAnimInfo)
    {
        //将正在播放的动画 属性设置为false
        var enumerator = m_RoleAnimInfoDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.IsPlaying = false;
        }

        if (m_RoleAnimInfoDic.TryGetValue(animId, out roleAnimInfo))
        {
            roleAnimInfo.LastUseTime = Time.time; //设置最后使用时间
            roleAnimInfo.IsPlaying = true;

            if (roleAnimInfo.IsLoad)
            {
                PlayAnim(roleAnimInfo);
            }
            else
            {
                //动画不存在 先加载动画
                LoadRoleAnimation(roleAnimInfo.CurrRoleAnimationData, PlayAnim);
            }
        }
    }

    private void PlayAnim(RoleAnimInfo roleAnimInfo)
    {
        m_PlayableGraph.Play();

        //根据索引拿到Playable
        var playable = m_AnimationMixerPlayable.GetInput(roleAnimInfo.Index);
        playable.SetTime(0);
        playable.Play();

        int len = m_RoleAnimInfoDic.Count;
        for (int i = 0; i < len; i++)
        {
            if (i == roleAnimInfo.Index)
            {
                //动画的长度
                m_AnimationMixerPlayable.SetInputWeight(i, 1);
            }
            else
            {
                m_AnimationMixerPlayable.SetInputWeight(i, 0);
            }
        }
    }


    public override void OnOpen()
    {
        base.OnOpen();
        //注册更新组件
        GameEntry.RegisterUpdateComponent(this);
        LoadSkin();
    }

    public override void OnClose()
    {
        base.OnClose();
        GameEntry.RemoveUpdateComponent(this);
        DeSpawn();
    }

    private void LoadSkin()
    {
        //先把当前皮肤卸载
        UnLoadSkin();

        //加载皮肤
        GameEntry.Pool.GameObjectSpawn(m_SkinId, (Transform trans, bool isNewInstance) =>
        {
            m_CurrSkinTransform = trans;
            m_CurrSkinTransform.SetParent(transform);
            m_CurrSkinTransform.localPosition = Vector3.zero;

            m_CurrRoleSkinComponent = m_CurrSkinTransform.GetComponent<RoleSkinComponent>();

            m_Animator = m_CurrSkinTransform.GetComponent<Animator>();
            //第一步 创建画布
            if (m_PlayableGraph.IsValid())
            {
                m_PlayableGraph.Destroy();
            }

            m_PlayableGraph = PlayableGraph.Create("PlayableGraph_" + m_SkinId);

            //创建输出节点
            m_AnimationPlayableOutput = AnimationPlayableOutput.Create(m_PlayableGraph, "output", m_Animator);
            CreateMixerPlayable();
            if (m_CurrRoleSkinComponent == null)
            {
                //角色根阶段上的SkinnedMeshRenderer
                m_CurrSkinnedMeshRenderer = m_CurrSkinTransform.GetComponentInChildren<SkinnedMeshRenderer>();
            }

            LoadInitRoleAnimations(m_CurrDTRole.AnimGroupId);

            //默认进入待机状态
            m_CurrRoleFsmManager.ChangeState(MyCommonEnum.RoleFsmState.Idle);
        });
    }

    /// <summary>
    /// 创建混合Playable
    /// </summary>
    private void CreateMixerPlayable()
    {
        //创建动画混合Playable
        m_AnimationMixerPlayable = AnimationMixerPlayable.Create(m_PlayableGraph, 100);

        //设置Output的源
        m_AnimationPlayableOutput.SetSourcePlayable(m_AnimationMixerPlayable, 0);

        //这时候是没有任何动画的
    }

    /// <summary>
    /// 加载皮肤材质
    /// </summary>
    /// <param name="materialName">皮肤材质路径</param>
    private void LoadSkinMaterial(string materialName)
    {
        if (m_CurrSkinnedMeshRenderer == null)
        {
            return;
        }

        GameEntry.Resource.ResourceLoaderManager.LoadMainAsset(AssetCategory.RoleSources, materialName, (entity =>
        {
            Material material = entity.Target as Material;

#if UNITY_EDITOR
            m_CurrSkinnedMeshRenderer.material = material;
#else
            m_CurrSkinnedMeshRenderer.shareMaterial = material;
#endif
        }));
    }

    /// <summary>
    /// 卸载皮肤
    /// </summary>
    private void UnLoadSkin()
    {
        if (m_CurrSkinTransform != null)
        {
            GameEntry.Pool.GameObjectDeSpawn(m_CurrSkinTransform);
            m_CurrSkinTransform = null;
        }

        m_CurrSkinnedMeshRenderer = null;
    }

    public void DeSpawn()
    {
        UnLoadSkin();
    }

    /// <summary>
    /// 检查需要卸载的角色动画
    /// </summary>
    public void CheckUnLoadRoleAnimation()
    {
        var enumerator = m_RoleAnimInfoDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var roleAnimInfo = enumerator.Current.Value;
            if (roleAnimInfo.IsExpire)
            {
                roleAnimInfo.IsLoad = false;
                roleAnimInfo.CurrPlayable.Destroy();
            }
        }
    }

    public void OnUpdate()
    {
        //TODO 测试代码
        if (Input.GetKeyUp(KeyCode.A))
        {
            m_CurrRoleFsmManager.ChangeState(MyCommonEnum.RoleFsmState.Idle);
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            m_CurrRoleFsmManager.ChangeState(MyCommonEnum.RoleFsmState.Run);
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            m_CurrRoleFsmManager.ChangeState(MyCommonEnum.RoleFsmState.Attack);
        }

        if (m_CurrRoleFsmManager == null)
        {
            return;
        }

        m_CurrRoleFsmManager.OnUpdate();
    }
}