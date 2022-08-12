﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace YouYouServer.Common
{
    public sealed class ServerConfig
    {
        /// <summary>
        /// 区服编号（这个编号指的是整个区的编号）
        /// </summary>
        public static int AreaServerId;

        /// <summary>
        /// 当前服务器类型
        /// </summary>
        public static ConstDefine.ServerType CurrServerType;

        /// <summary>
        /// 当前服务器编号 
        /// </summary>
        public static int CurrServerId = 0;

        /// <summary>
        /// Mongo 连接字符串
        /// </summary>
        public static string MongoConnectionString;

        /// <summary>
        /// Redis链接字符串
        /// </summary>
        public static string RedisConnectionString;

        /// <summary>
        /// 数据表路径
        /// </summary>
        public static string DataTablePath;

        /// <summary>
        /// 服务器列表
        /// </summary>
        public static List<Server> ServerList;

        /// <summary>
        /// 场景和游戏服务器的对应字典
        /// </summary>
        public static Dictionary<int, SceneConfig> SceneInServerDic;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Configs\\ServerConfig.xml";

            XDocument doc = XDocument.Load(path);

            AreaServerId = doc.Root.Element("AreaServerId").Value.ToInt();
            CurrServerType = (ConstDefine.ServerType)doc.Root.Element("CurrServerType").Value.ToInt();
            CurrServerId = doc.Root.Element("CurrServerId").Value.ToInt();

            MongoConnectionString = doc.Root.Element("MongoConnectionString").Value;
            RedisConnectionString = doc.Root.Element("RedisConnectionString").Value;
            DataTablePath = doc.Root.Element("DataTablePath").Value;

            ServerList = new List<Server>();
            SceneInServerDic = new Dictionary<int, SceneConfig>();

            //初始化服务器节点
            IEnumerable<XElement> lst = doc.Root.Elements("Servers").Elements("Item");
            foreach (XElement item in lst)
            {
                ServerList.Add(new Server()
                {
                    CurrServerType = (ConstDefine.ServerType)item.Attribute("ServerType").Value.ToInt(),
                    ServerId = item.Attribute("ServerId").Value.ToInt(),
                    Ip = item.Attribute("Ip").Value,
                    Port = item.Attribute("Port").Value.ToInt()
                });
            }

            //初始化游戏服务器节点中 配置的场景ID
            IEnumerable<XElement> lstScenes = doc.Root.Element("Scenes").Elements("Item");
            foreach (var item in lstScenes)
            {
                int sceneId = item.Attribute("SceneId").Value.ToInt();
                int serverId = item.Attribute("ServerId").Value.ToInt();
                string aoiJsonDataPath = item.Attribute("AOIJsonDataPath").Value;

                SceneConfig configScene = new SceneConfig()
                {
                    SceneId = sceneId,
                    ServerId = serverId,
                    AOIJsonDataPath = aoiJsonDataPath,
                };

                SceneInServerDic[sceneId] = configScene;

                //找到对应的服务器
                Server server = GetServer(ConstDefine.ServerType.GameServer, serverId);
                if (server != null)
                {
                    server.Sceneconfigs.Add(configScene);
                }
            }

            Console.WriteLine("ServerConfig Init Complete");
        }

        /// <summary>
        /// 账号数据库DBName
        /// </summary>
        public const string AccountDBName = "DBAccount";

        #region GameServerDBName 游戏服DBName
        private static string m_GameServerDBName = null;
        /// <summary>
        /// 游戏服DBName
        /// </summary>
        public static string GameServerDBName
        {
            get
            {
                if (string.IsNullOrEmpty(m_GameServerDBName))
                {
                    m_GameServerDBName = string.Format("GameServer_{0}", AreaServerId);
                }
                return m_GameServerDBName;
            }
        }
        #endregion

        #region RoleHashKey 角色哈希Key
        private static string m_RoleHashKey = null;
        /// <summary>
        /// 角色哈希Key
        /// </summary>
        public static string RoleHashKey
        {
            get
            {
                if (string.IsNullOrEmpty(m_RoleHashKey))
                {
                    m_RoleHashKey = string.Format("{0}_RoleHash", AreaServerId);
                }
                return m_RoleHashKey;
            }
        }
        #endregion

        #region RoleNickNameKey 角色昵称Key
        private static string m_RoleNickNameKey = null;

        /// <summary>
        /// 角色昵称Key
        /// </summary>
        public static string RoleNickNameKey
        {
            get
            {
                if (string.IsNullOrEmpty(m_RoleNickNameKey))
                {
                    m_RoleNickNameKey = string.Format("{0}_NickName", AreaServerId);
                }
                return m_RoleNickNameKey;
            }
        }
        #endregion

        #region Server 单台服务器
        /// <summary>
        /// 单台服务器
        /// </summary>
        public class Server
        {
            public Server()
            {
                Sceneconfigs = new List<SceneConfig>(100);
            }

            /// <summary>
            /// 服务器类型
            /// </summary>
            public ConstDefine.ServerType CurrServerType;

            /// <summary>
            /// 服务器编号
            /// </summary>
            public int ServerId;

            /// <summary>
            /// 服务器Ip
            /// </summary>
            public string Ip;

            /// <summary>
            /// 服务器端口号
            /// </summary>
            public int Port;

            /// <summary>
            /// 这个服务器需要开启的场景列表
            /// </summary>
            public List<SceneConfig> Sceneconfigs { get; }
        }
        #endregion

        #region GetServer 根据服务器类型和编号获取服务器
        /// <summary>
        /// 根据服务器类型和编号获取服务器
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static Server GetServer(ConstDefine.ServerType serverType, int serverId)
        {
            int len = ServerList.Count;
            for (int i = 0; i < len; i++)
            {
                Server server = ServerList[i];
                if (server.CurrServerType == serverType && server.ServerId == serverId)
                {
                    return server;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据服务器类型获取服务器列表
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static List<Server> GetServerByType(ConstDefine.ServerType serverType)
        {
            List<Server> lst = new List<Server>();
            int len = ServerList.Count;
            for (int i = 0; i < len; i++)
            { 
                Server server = ServerList[i];
                if (server.CurrServerType == serverType)
                {
                    lst.Add(server);
                }
            }
            return lst;
        }
        #endregion

        #region GetCurrServer获取当前服务器
        /// <summary>
        /// 获取当前服务器
        /// </summary>
        /// <returns></returns>
        public static Server GetCurrServer()
        {
            return GetServer(CurrServerType, CurrServerId);
        }
        #endregion


        public class SceneConfig
        {
            /// <summary>
            /// 场景编号
            /// </summary>
            public int SceneId;

            /// <summary>
            /// 服务器编号
            /// </summary>
            public int ServerId;

            /// <summary>
            /// AOI区域数据
            /// </summary>
            public string AOIJsonDataPath;
        }
    }
}
