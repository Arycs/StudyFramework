using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 进入游戏流程
    /// </summary>
    public class ProcedureEnterGame : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("OnEnter ProcedureEnterGame");
            Debug.LogError("角色编号=" + GameEntry.Data.UserDataManager.CurrRoleId);
            Debug.LogError("职业编号=" + GameEntry.Data.UserDataManager.CurrJobId);

            //TODO 这里发送进入游戏消息 最后才切换世界地图
            GameEntry.Procedure.ChangeState(ProcedureState.WorldMap);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            Debug.Log("OnLeave ProcedureEnterGame");
            GameEntry.CameraCtrl.Open();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}