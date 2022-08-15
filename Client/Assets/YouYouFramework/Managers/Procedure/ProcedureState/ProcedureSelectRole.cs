using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 选人流程
    /// </summary>
    public class ProcedureSelectRole : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GameEntry.Event.CommonEvent.AddEventListener(CommonEventId.OnRegClientComplete,OnRegClientComplete);
            //TODO 打开区服列表,选区之类的,这里直连一个区服
            ConnectServer("192.168.1.7", 1304);
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            GameEntry.Event.CommonEvent.RemoveEventListener(CommonEventId.OnRegClientComplete,OnRegClientComplete);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>
        /// 连接到网关服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void ConnectServer(string ip, int port)
        {
            GameEntry.Socket.ConnectToMainSocket(ip,port, (bool result) =>
            {
                if (result)
                {
                    GameEntry.Data.UserDataManager.RegClient();
                }
                else
                {
                    GameEntry.UI.OpenDialogFormBySysCode(SysCode.Connect_TimeOut);
                }
            });
        }
        
        private void OnRegClientComplete(object userData)
        {
            VarBool varBool = userData as VarBool;
            if (varBool)
            {
                //链接成功
                //加载选择角色场景
                GameEntry.Scene.LoadScene(SysScene.SelectRole,true,onComplete: () =>
                {
                    GameEntry.UI.CloseUIForm(UIFormId.UI_LogonBG);
                    GameEntry.Data.UserDataManager.GetRoleList();
                });
            }
        }
    }
}