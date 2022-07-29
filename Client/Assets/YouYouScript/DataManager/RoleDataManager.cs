using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class RoleDataManager :IDisposable
{
    /// <summary>
    /// 根据职业编号创建角色
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="onComplete"></param>
    public void CreatePlayerByJobId(int jobId, BaseAction<RoleCtrl> onComplete = null)
    {
        int skinId = GameEntry.DataTable.JobList.Get(jobId).RoleId;
        GameEntry.Pool.GameObjectSpawn(SysPrefabId.RoleCtrl, (trans =>
        {
            RoleCtrl roleCtrl = trans.GetComponent<RoleCtrl>();
            roleCtrl.Init(skinId);
            onComplete?.Invoke(roleCtrl);
        }));
    }


    public void Dispose()
    {
    }
}
