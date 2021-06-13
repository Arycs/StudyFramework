using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YouYouServer.Model
{
    public sealed class ServerConfig
    {
        /// <summary>
        /// 服务器编号
        /// </summary>
        public static int ServerId = 1;

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
        /// 初始化
        /// </summary>
        public static void Init()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Configs\\ServerConfig.xml";

            XDocument doc = XDocument.Load(path);

            int.TryParse(doc.Root.Element("ServerId").Value, out ServerId);
            MongoConnectionString = doc.Root.Element("MongoConnectionString").Value;
            RedisConnectionString = doc.Root.Element("RedisConnectionString").Value;
            DataTablePath = doc.Root.Element("DataTablePath").Value;


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
                    m_GameServerDBName = string.Format("GameServer_{0}", ServerId);
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
                    m_RoleHashKey = string.Format("{0}_RoleHash", ServerId);
                }
                return m_RoleHashKey;
            }
        }
        #endregion
    }
}
