using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class SocketManager : ManagerBase,IDisposable
    {
        [Header("每帧最大发送数量")] public int MaxSendCount = 5;

        [Header("每次发包最大的字节")] public int MaxSendByteCount = 1024;

        [Header("每帧最大处理包数量")] public int MaxReceiveCount = 5;

        [Header("心跳间隔")] public int HeartbeatInterval = 10;

        /// <summary>
        /// 上次的心跳时间
        /// </summary>
        private float m_PrevHeartbeatTime = 0;

        /// <summary>
        /// PING值(毫秒)
        /// </summary>
        [HideInInspector] public int PingValue;

        /// <summary>
        /// 游戏服务器时间
        /// </summary>
        [HideInInspector] public long GameServerTime;

        /// <summary>
        /// 和服务器对表的时刻
        /// </summary>
        [HideInInspector] public float CheckServerTime;

        /// <summary>
        /// 是否已经连接到了主Socket
        /// </summary>
        private bool m_IsConnectToMainSocket = false;

        private SocketManager m_SocketManager;

        /// <summary>
        /// 发送用的MS
        /// </summary>
        public MMO_MemoryStream SocketSendMS
        {
            get;
            private set;
        }

        /// <summary>
        /// 接收用的MS
        /// </summary>
        public MMO_MemoryStream SocketReceiveMS
        {
            get;
            private set;
        }


        /// <summary>
        /// SocketTcp访问器的链表
        /// </summary>
        private LinkedList<SocketTcpRoutine> m_SocketTcpRoutineList;
        public SocketManager()
        {
            m_SocketTcpRoutineList = new LinkedList<SocketTcpRoutine>();
        }

        /// <summary>
        /// 注册SocketTcp访问器
        /// </summary>
        /// <param name="routine"></param>
        internal void RegisterSocketTcpRoutine(SocketTcpRoutine routine)
        {
            m_SocketTcpRoutineList.AddFirst(routine);
        }
        
        /// <summary>
        /// 移除SocketTcp访问器
        /// </summary>
        /// <param name="routine"></param>
        internal void RemoveSocketTcpRoutine(SocketTcpRoutine routine)
        {
            m_SocketTcpRoutineList.Remove(routine);
        }

        internal void OnUpdate()
        {
            for (LinkedListNode<SocketTcpRoutine> curr = m_SocketTcpRoutineList.First; curr != null; curr = curr.Next) 
            {
                curr.Value.OnUpdate();
            }
        }

        public void Dispose()
        {
            m_SocketTcpRoutineList.Clear();
        }

        //===================================业务逻辑====================================
        /// <summary>
        /// 主Socket
        /// </summary>
        private SocketTcpRoutine m_MainSocket;

        /// <summary>
        /// 链接主Socket 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void ConnectToMainSocket(string ip, int port)
        {
            m_MainSocket.Connect(ip, port);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="buffer"></param>
        public void SendMsg(byte[] buffer)
        {
            m_MainSocket.SendMsg(buffer);
        }

        public void SendMsg(IProto proto)
        {
#if DEBUG_LOG_PROTO
            Debug.Log("<color=#ffa200>发送消息:</color><color=#FFFB80>" + proto.ProtoEnName + "" + proto.ProtoCode +
                      "</color>");
            Debug.Log("<color=#ffdeb3>==>>" + JsonUtility.ToJson(proto) + "</color>");
#endif
            m_MainSocket.SendMsg(proto.ToArray());
        }


    }
}