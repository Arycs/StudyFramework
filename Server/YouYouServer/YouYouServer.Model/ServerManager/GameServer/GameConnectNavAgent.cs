using System;
using System.Collections.Generic;
using System.Text;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;

namespace YouYouServer.Model
{
    public class GameConnectNavAgent : ConnectAgentBase
    {
        public GameConnectNavAgent()
        {
            List<ServerConfig.Server> servers = ServerConfig.GetServerByType(ConstDefine.ServerType.NavServer);
            if (servers != null && servers.Count == 1)
            {
                TargetServerConfig = servers[0];
                TargetServerConnect = new ServerConnect(TargetServerConfig);
                AddEventListener();
            }
            else
            {
                LoggerMgr.Log(LoggerLevel.LogError, LogType.SysLog, "No NavServer");
            }
        }

        #region RegisterToNavServer 注册到寻路服务器

        /// <summary>
        /// 注册到寻路服务器
        /// </summary>
        public void RegisterToNavServer()
        {
            TargetServerConnect.Connect(onConnectSuccess: () =>
            {
                GetNavPath(1, 1, new GS2NS_Vector3() { X = 171.9f, Y = 25.5F, Z = 345.6f },
                    new GS2NS_Vector3() { X = 172.1f, Y = 25.5f, Z = 331.6f });
            });
        }

        #endregion

        public override void AddEventListener()
        {
            base.AddEventListener();
            TargetServerConnect.EventDispatcher.AddEventListener(ProtoIdDefine.Proto_NS2GS_ReturnNavPath, OnNS2GS_ReturnNavPath);
        }

        private void GetNavPath(int taskId, int sceneId, GS2NS_Vector3 beginPos, GS2NS_Vector3 endPos)
        {
            GS2NS_GetNavPath proto = new GS2NS_GetNavPath();
            proto.TaskId = taskId;
            proto.SceneId = sceneId;
            proto.BeginPos = beginPos;
            proto.EndPos = endPos;

            TargetServerConnect.ClientSocket.SendMsg(proto);
        }

        private void OnNS2GS_ReturnNavPath(byte[] buffer)
        {
            NS2GS_ReturnNavPath proto = NS2GS_ReturnNavPath.Parser.ParseFrom(buffer);

            Console.WriteLine(proto.Valid);
            Console.WriteLine(proto.Path);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            TargetServerConnect.EventDispatcher.RemoveEventListener(ProtoIdDefine.Proto_NS2GS_ReturnNavPath, OnNS2GS_ReturnNavPath);
        }
    }
}
