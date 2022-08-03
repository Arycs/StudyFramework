using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DTSys_SceneDetailDBModel
{
    private List<DTSys_SceneDetailEntity> m_retList = new List<DTSys_SceneDetailEntity>();

    /// <summary>
    /// 根据场景编号获取场景明细
    /// </summary>
    /// <param name="sceneId"></param>
    /// <param name="sceneGrade"></param>
    /// <returns></returns>
    public List<DTSys_SceneDetailEntity> GetListBySceneId(int sceneId, int sceneGrade)
    {
        m_retList.Clear();
        List<DTSys_SceneDetailEntity> lst = this.GetList();
        int len = lst.Count;
        for (int i = 0; i < len; i++)
        {
            DTSys_SceneDetailEntity entity = lst[i];
            if (entity.SceneId == sceneId && entity.SceneGrade <= sceneGrade)
            {
                m_retList.Add(entity);
            }
        }

        return m_retList;
    }
}