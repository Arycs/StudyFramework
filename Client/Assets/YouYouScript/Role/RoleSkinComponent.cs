using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色皮肤组件
/// </summary>
public class RoleSkinComponent : MonoBehaviour
{
    /// <summary>
    /// 根骨骼
    /// </summary>
    [SerializeField] private Transform m_RootBone;

    /// <summary>
    /// 角色身上的SkinnedMeshRenderer 一个角色只有一个SkinnedMeshRenderer
    /// </summary>
    [SerializeField] private SkinnedMeshRenderer m_CurrSkinnedMeshRenderer;

    /// <summary>
    /// 部件
    /// </summary>
    [SerializeField] private SkinnedMeshRendererPart[] Parts;

    /// <summary>
    /// 部件长度
    /// </summary>
    [SerializeField] private int m_PartsLen;

    /// <summary>
    /// 角色当前穿戴的部件列表
    /// </summary>
    private List<SkinnedMeshRenderer> m_CurrPartList;

    /// <summary>
    /// 合并网格对象集合
    /// </summary>
    private List<CombineInstance> m_CombineInstances;

    /// <summary>
    /// 材质集合
    /// </summary>
    private List<Material> m_Materials;

    /// <summary>
    /// SkinnedMeshRenderer对应的骨骼信息
    /// </summary>
    private List<Transform> m_Bones;

    /// <summary>
    /// 角色身上的骨骼数组
    /// </summary>
    private Transform[] m_BoneTransforms;

    private void Awake()
    {
        m_CurrPartList = new List<SkinnedMeshRenderer>();
        m_CombineInstances = new List<CombineInstance>();
        m_Materials = new List<Material>();
        m_Bones = new List<Transform>();
    }

    private void OnDestroy()
    {
        m_CurrPartList.Clear();
        m_CurrPartList = null;

        m_CombineInstances.Clear();
        m_CombineInstances = null;

        m_Materials.Clear();
        m_Materials = null;

        m_Bones.Clear();
        m_Bones = null;

        m_RootBone = null;
        m_CurrSkinnedMeshRenderer = null;
    }

    private void Start()
    {
        m_PartsLen = Parts.Length;

        m_BoneTransforms = m_RootBone.GetComponentsInChildren<Transform>();
        m_CurrSkinnedMeshRenderer.sharedMesh = new Mesh();
        
        //TODO  临时写的, 具体加载 哪些应该由服务器获取玩家身上的皮肤
        LoadPart(new List<int> {1, 2});
    }

    /// <summary>
    /// 加载部件
    /// </summary>
    /// <param name="parts"></param>
    public void LoadPart(List<int> parts)
    {
        m_CombineInstances.Clear();
        m_Materials.Clear();
        m_Bones.Clear();
        m_CurrPartList.Clear();

        int len = parts.Count;
        for (int i = 0; i < len; i++)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetPartByNo(parts[i]);
            if (skinnedMeshRenderer != null)
            {
                m_CurrPartList.Add(skinnedMeshRenderer);
            }
        }

        for (int i = 0; i < m_CurrPartList.Count; i++)
        {
            var skinnedMeshRenderer = m_CurrPartList[i];
            m_Materials.AddRange(skinnedMeshRenderer.materials);
            //添加合并网络
            for (int sub = 0; sub < skinnedMeshRenderer.sharedMesh.subMeshCount; sub++)
            {
                var ci = new CombineInstance {mesh = skinnedMeshRenderer.sharedMesh, subMeshIndex = sub};
                m_CombineInstances.Add(ci);
            }
            
            //==================此处是加载SkinnedMeshRenderer对应的骨骼,也就是Bones的数量实际上会大于玩家身上的骨骼数量===========
            for (int sub = 0; sub < skinnedMeshRenderer.bones.Length; sub++)
            {
                var lenBonesTrans = m_BoneTransforms.Length;
                for (int m = 0; m < lenBonesTrans; m++)
                {
                    var t = m_BoneTransforms[m];
                    if (t.name != skinnedMeshRenderer.bones[sub].name)
                    {
                        continue;
                    }
                    m_Bones.Add(t);
                    break;
                }
            }
        }
        
        m_CurrSkinnedMeshRenderer.sharedMesh.CombineMeshes(m_CombineInstances.ToArray(),false,false); //合并模型
        m_CurrSkinnedMeshRenderer.bones = m_Bones.ToArray(); // 赋予骨骼
        m_CurrSkinnedMeshRenderer.materials = m_Materials.ToArray(); // 赋予材质
    }

    /// <summary>
    /// 根据编号获取组件
    /// </summary>
    /// <param name="no"></param>
    /// <returns></returns>
    private SkinnedMeshRenderer GetPartByNo(int no)
    {
        for (int i = 0; i < m_PartsLen; i++)
        {
            var skinnedMeshRendererPart = Parts[i];
            if (skinnedMeshRendererPart.No == no)
            {
                return skinnedMeshRendererPart.Part;
            }
        }

        return null;
    }

    /// <summary>
    /// 皮肤配置
    /// </summary>
    [Serializable]
    public class SkinnedMeshRendererPart
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int No;

        /// <summary>
        /// 皮肤部件
        /// </summary>
        public SkinnedMeshRenderer Part;
    }
}