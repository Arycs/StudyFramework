﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YouYou.Proto;
using YouYouServer.Commmon;
using YouYouServer.Common;
using YouYouServer.Core;

namespace YouYouServer.Model
{
    public class GameConnectNavAgent : ConnectAgentBase
    {
        /// <summary>
        /// 寻路回调字典
        /// </summary>
        private Dictionary<long, Action<NS2GS_ReturnNavPath>> m_ReturnNavPathDic;

        private ServerTimer tickTime;

        /// <summary>
        /// PingValue
        /// </summary>
        public int PingValue;
        
        public GameConnectNavAgent()
        {
            m_ReturnNavPathDic = new Dictionary<long, Action<NS2GS_ReturnNavPath>>();

            List<ServerConfig.Server> servers = ServerConfig.GetServerByType(ConstDefine.ServerType.NavServer);
            if (servers != null && servers.Count == 1)
            {
                TargetServerConfig = servers[0];
                TargetServerConnect = new ServerConnect(TargetServerConfig);
                AddEventListener();
            }
            else
            {
                LoggerMgr.Log(LoggerLevel.LogError, Common.LogType.SysLog, "No NavServer");
            }
        }

        #region RegisterToNavServer 注册到寻路服务器

        /// <summary>
        /// 注册到寻路服务器
        /// </summary>
        public void RegisterToNavServer(Action onComplete)
        {
            TargetServerConnect.Connect(onConnectSuccess: () =>
            {
                tickTime = new ServerTimer(ServerTimerRunType.FixedInterval, OnTick, interval: 2);
                TimerManager.RegisterServerTimer(tickTime);
                
                onComplete?.Invoke();
            });

            TargetServerConnect.ClientSocket.OnDisConnect = () =>
            {
                TimerManager.RemoveServerTimer(tickTime);
            };
        }

        /// <summary>
        /// 发送心跳到寻路服务器
        /// </summary>
        private void OnTick()
        {
            GS2NS_Heartbeat proto = new GS2NS_Heartbeat();
            proto.ServerTime = DateTime.UtcNow.Ticks;
            proto.Ping = PingValue;
            TargetServerConnect.ClientSocket.SendMsg(proto);
        }

        #endregion

        public override void AddEventListener()
        {
            base.AddEventListener();
            TargetServerConnect.EventDispatcher.AddEventListener(ProtoIdDefine.Proto_NS2GS_ReturnNavPath, OnNS2GS_ReturnNavPath);
            TargetServerConnect.EventDispatcher.AddEventListener(ProtoIdDefine.Proto_NS2GS_Heartbeat, OnNS2GS_Heartbeat);

        }

        private void OnNS2GS_Heartbeat(byte[] buffer)
        {
            NS2GS_Heartbeat proto = NS2GS_Heartbeat.Parser.ParseFrom(buffer);
            PingValue = (int) ((DateTime.UtcNow.Ticks - proto.ServerTime) * 0.5f / 10000);
            Console.WriteLine($"GS PING {PingValue} ");
        }

        public void GetNavPath(int sceneId, UnityEngine.Vector3 beginPos, UnityEngine.Vector3 endPos, Action<NS2GS_ReturnNavPath> onComplete)
        {
            GS2NS_GetNavPath proto = new GS2NS_GetNavPath();

            proto.TaskId = GenerateTaskId();
            proto.SceneId = sceneId;


            proto.BeginPos = new GS2NS_Vector3() { X = beginPos.x, Y = beginPos.y, Z = beginPos.z };
            proto.EndPos = new GS2NS_Vector3() { X = endPos.x, Y = endPos.y, Z = endPos.z };

            m_ReturnNavPathDic[proto.TaskId] = onComplete;

            TargetServerConnect.ClientSocket.SendMsg(proto);
        }

        private void OnNS2GS_ReturnNavPath(byte[] buffer)
        {
            NS2GS_ReturnNavPath proto = NS2GS_ReturnNavPath.Parser.ParseFrom(buffer);

            if (m_ReturnNavPathDic.TryGetValue(proto.TaskId, out Action<NS2GS_ReturnNavPath> onComplete))
            {
                onComplete?.Invoke(proto);
                m_ReturnNavPathDic.Remove(proto.TaskId);
            }
        }

        private long GenerateTaskId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            TargetServerConnect.EventDispatcher.RemoveEventListener(ProtoIdDefine.Proto_NS2GS_ReturnNavPath, OnNS2GS_ReturnNavPath);
            TargetServerConnect.EventDispatcher.RemoveEventListener(ProtoIdDefine.Proto_NS2GS_Heartbeat, OnNS2GS_Heartbeat);

        }
    }
}
