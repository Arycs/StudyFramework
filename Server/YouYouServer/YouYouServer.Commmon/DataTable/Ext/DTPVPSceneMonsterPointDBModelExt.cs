using System;
using System.Collections.Generic;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTPVPSceneMonsterPoint数据管理
    /// </summary>
    public partial class DTPVPSceneMonsterPointDBModel
    {
        public List<DTPVPSceneMonsterPointEntity> ret = new List<DTPVPSceneMonsterPointEntity>();

        /// <summary>
        /// 根据场景 获取刷怪点列表
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public List<DTPVPSceneMonsterPointEntity> GetListBySceneId(int sceneId)
        {
            ret.Clear();
            var len = m_List.Count;
            for (int i = 0; i < len; i++)
            {
                if (m_List[i].SceneId == sceneId)
                {
                    ret.Add(m_List[i]);
                }
            }
            return ret;
        }
    }

}
