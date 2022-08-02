using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class RoleCtrl : BaseSprite
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

    /// <summary>
    /// 动画片段字典
    /// </summary>
    private Dictionary<string, AnimationClip> m_AnimationClipDic;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            ChangeSkin(100001);
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            ChangeSkin(100002);
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
        RoleEntity drRole = GameEntry.DataTable.RoleList.Get(roleId);
        m_SkinId = drRole.PrefabId;

        LoadInitRoleAnimations(drRole.AnimGroupId);
    }

    /// <summary>
    /// 加载初始角色动画
    /// </summary>
    /// <param name="animGroupId"></param>
    private void LoadInitRoleAnimations(int animGroupId)
    {
        List<RoleAnimationEntity> roleAnimations = GameEntry.DataTable.RoleAnimationList.GetListByGroupId(animGroupId);
        int lenRoleAnimations = roleAnimations.Count;
        for (int i = 0; i < lenRoleAnimations; i++)
        {
            RoleAnimationEntity roleAnimation = roleAnimations[i];
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
    private void LoadRoleAnimation(RoleAnimationEntity roleAnimation)
    {
        GameEntry.Resource.ResourceLoaderManager.LoadMainAsset(AssetCategory.RoleSources,
            GameUtil.GetRoleAnimationPath(roleAnimation.AnimPath), (
                entity =>
                {
                    var animationClip = entity.Target as AnimationClip;
                    m_AnimationClipDic[roleAnimation.AnimPath] = animationClip;
                    Debug.LogError($"角色动画路径===> {roleAnimation.AnimPath} :=====: 动画片段==>{animationClip}");
                }));
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_AnimationClipDic = new Dictionary<string, AnimationClip>();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        LoadSkin();
    }

    public override void OnClose()
    {
        base.OnClose();
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
            if (m_CurrRoleSkinComponent == null)
            {
                //角色根阶段上的SkinnedMeshRenderer
                m_CurrSkinnedMeshRenderer = m_CurrSkinTransform.GetComponentInChildren<SkinnedMeshRenderer>();
            }
        });
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
}