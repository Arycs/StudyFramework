using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.Managers;

namespace YouYouServer.Model
{
    public static class MongoDBClient
    {
        private static object lock_obj = new object();

        private static MongoClient m_CurrClient = null;

        /// <summary>
        /// 当前的MongoClient
        /// </summary>
        public static MongoClient CurrClient
        {
            get
            {
                if (m_CurrClient == null)
                {
                    lock (lock_obj)
                    {
                        if (m_CurrClient == null)
                        {
                            m_CurrClient = new MongoClient(ServerConfig.MongoConnectionString);
                        }
                    }
                }
                return m_CurrClient;
            }
        }
    }
}
