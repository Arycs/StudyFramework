using System;
using YouYouServer.Core.Logger;
using YouYouServer.Model;
using YouYouServer.Model.Managers;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello GameServer!");

            ServerConfig.Init();
            DataTableManager.Init();
            LoggerMgr.Init();
            YFRedisClient.InitRedisClient();

            GameServerManager.Init();
            Console.ReadLine();
        }
    }
}
