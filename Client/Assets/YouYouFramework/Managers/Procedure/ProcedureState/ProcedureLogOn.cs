using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 登录流程
    /// </summary>
    public class ProcedureLogOn : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GameEntry.Log(LogCategory.Procedure, "OnEnter ProcedureLogOn");
            GameEntry.UI.OpenUIForm(SysUIFormId.UI_LogonBG, onOpen: OnLogonBGOpen);
            var arr = GameEntry.DataTable.SkillLevelList.Get(4).Args;
            for (int i = 0; i < arr.Length; i++)
            {
                Debug.Log($"===>>>>>>>>>>{arr[i]}");
            }
        }

        private void OnLogonBGOpen(UIFormBase uiFormBase)
        {
            GameEntry.UI.OpenUIForm(SysUIFormId.UI_Login);

            GameEntry.Event.CommonEvent.Dispatch(SysEventId.CloseCheckVersionUI);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            GameEntry.UI.CloseUIForm(SysUIFormId.UI_Login);
            GameEntry.UI.CloseUIForm(SysUIFormId.UI_LogonBG);

            GameEntry.Log(LogCategory.Procedure, "OnLeave ProcedureLogOn");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}