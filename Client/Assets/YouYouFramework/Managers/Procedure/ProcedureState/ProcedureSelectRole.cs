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
                    //链接成功
                    //加载选择角色场景
                    GameEntry.Scene.LoadScene(SysScene.SelectRole,true,onComplete: () =>
                    {
                        GameEntry.UI.CloseUIForm(UIFormId.UI_LogonBG);
                        SearchRole();
                    });
                }
                else
                {
                    GameEntry.UI.OpenDialogFormBySysCode(SysCode.Connect_TimeOut);
                }
            });
        }
        
        /// <summary>
        /// 查询玩家玩家已有角色
        /// </summary>
        private void SearchRole()
        {
            
        }
    }
}