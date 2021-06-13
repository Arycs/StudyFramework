using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.Managers;

namespace YouYouServer.Model
{
    /// <summary>
    /// YFRedisClient
    /// </summary>
    public static class YFRedisClient
    {
        private static object lock_obj = new object();

        private static CSRedisClient m_CurrClient = null;

        /// <summary>
        /// InitRedisClient
        /// </summary>
        public static void InitRedisClient()
        {
            if (m_CurrClient == null)
            {
                lock (lock_obj)
                {
                    if (m_CurrClient == null)
                    {
                        m_CurrClient = new CSRedisClient(ServerConfig.RedisConnectionString);
                        RedisHelper.Initialization(m_CurrClient);
                        Console.WriteLine("RedisHelper Init Complete");
                    }
                }
            }
        }
    }
}

