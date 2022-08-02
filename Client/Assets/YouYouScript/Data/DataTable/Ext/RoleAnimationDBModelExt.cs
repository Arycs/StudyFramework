using System.Collections.Generic;
using YouYou;

/// <summary>
/// RoleAnimation数据管理 扩展
/// </summary>
public partial class RoleAnimationDBModel
{
    private List<RoleAnimationEntity> ret = new List<RoleAnimationEntity>();
    
    /// <summary>
    /// 通过 动画组ID 获取对应动画
    /// </summary>
    /// <param name="animGroupId"></param>
    /// <returns></returns>
    public List<RoleAnimationEntity> GetListByGroupId(int animGroupId)
    {
        ret.Clear();
        int len = m_List.Count;
        for (int i = 0; i < len; i++)
        {
            var roleAnimation = m_List[i];
            if (roleAnimation.GroupId == animGroupId)
            {
                ret.Add(roleAnimation);
            }
        }
        return ret;
    }
}