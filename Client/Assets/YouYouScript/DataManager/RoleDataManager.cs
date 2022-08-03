using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class RoleDataManager : IDisposable
{
    /// <summary>
    /// 当前已经加载的角色
    /// </summary>
    private LinkedList<RoleCtrl> m_RoleList;


    public RoleDataManager()
    {
        m_RoleList = new LinkedList<RoleCtrl>();
    }

    /// <summary>
    /// 根据职业编号创建角色
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="onComplete"></param>
    public void CreatePlayerByJobId(int jobId, BaseAction<RoleCtrl> onComplete = null)
    {
        //角色ID
        int roleId = GameEntry.DataTable.JobList.Get(jobId).RoleId;
        //加载角色控制器
        GameEntry.Pool.GameObjectSpawn(SysPrefabId.RoleCtrl, (Transform trans, bool isNewInstance) =>
        {
            RoleCtrl roleCtrl = trans.GetComponent<RoleCtrl>();
            roleCtrl.Init(roleId);

            //设置角色坐标位置
            roleCtrl.transform.position = new Vector3(
                GameEntry.Scene.CurrSceneEntity.PlayerBornPos_1,
                GameEntry.Scene.CurrSceneEntity.PlayerBornPos_2,
                GameEntry.Scene.CurrSceneEntity.PlayerBornPos_3);
            
            if (!isNewInstance)
            {
                //如果不是新实例,在这里执行OnOpen方法
                roleCtrl.OnOpen();
            }
            m_RoleList.AddLast(roleCtrl);
            onComplete?.Invoke(roleCtrl);
        });
    }

    /// <summary>
    /// 角色回池
    /// </summary>
    /// <param name="roleCtrl"></param>
    public void DeSpawnRole(RoleCtrl roleCtrl)
    {
        //先执行角色回池的方法 把角色以来的其他零件回池
        roleCtrl.OnClose();
        //然后回池
        GameEntry.Pool.GameObjectDeSpawn(roleCtrl.transform);
        m_RoleList.Remove(roleCtrl);
    }

    /// <summary>
    /// 回池所有角色
    /// </summary>
    public void DeSpawnAllRole()
    {
        for (LinkedListNode<RoleCtrl> curr = m_RoleList.First; curr != null;)
        {
            var next = curr.Next;
            DeSpawnRole(curr.Value);
            curr = next;
        }
    }

    public void Dispose()
    {
        m_RoleList.Clear();
    }

    /// <summary>
    /// 检查需要卸载的角色动画
    /// </summary>
    public void CheckUnLoadRoleAnimation()
    {
        for (var curr = m_RoleList.First; curr != null;)
        {
            var next = curr.Next;
            curr.Value.CheckUnLoadRoleAnimation();
            curr = next;
        }
        
    }
}