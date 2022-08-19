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
            GameEntry.UI.OpenUIForm(UIFormId.UI_Loading);
            GameEntry.Data.UserDataManager.EnterGame();
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