using System.Collections.Generic;
using YouYouServer.Common;
using YouYouServer.Core.Logger;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 游戏服管理器
    /// </summary>
    public sealed class GameServerManager
    {
        /// <summary>
        /// 当前服务器
        /// </summary>
        public static ServerConfig.Server CurrServer;

        /// <summary>
        /// 连接到中心服务器代理
        /// </summary>
        public static GameConnectWorldAgent ConnectWorldAgent;

        public static void Init()
        {
            CurrServer = ServerConfig.GetCurrServer();

            //实例化连接到中心服务器代理
            ConnectWorldAgent = new GameConnectWorldAgent();
            ConnectWorldAgent.RegisterToWorldServer();
        }

        
    }
}
