﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 连接代理基类
    /// </summary>
    public abstract class ConnectAgentBase : IDisposable
    {
        /// <summary>
        /// 目标服务器配置
        /// </summary>
        public ServerConfig.Server TargetServerConfig;

        /// <summary>
        /// 目标服务器连接器
        /// </summary>
        public ServerConnect TargetServerConnect;

        /// <summary>
        /// 监听中心服务器发来的消息
        /// </summary>
        public virtual void AddEventListener()
        {

        }

        /// <summary>
        /// 移除中心服务器发来的消息
        /// </summary>
        public virtual void RemoveEventListener()
        {

        }

        public void Dispose()
        {
            RemoveEventListener();
        }
    }
}
