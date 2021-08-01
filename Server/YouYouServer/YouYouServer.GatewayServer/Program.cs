using System;
using YouYouServer.Core.Logger;
using YouYouServer.Model;
using YouYouServer.Model.Managers;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.GatewayServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello GatewayServer!");

            ServerConfig.Init();
            DataTableManager.Init();
            LoggerMgr.Init();
            YFRedisClient.InitRedisClient();

            GatewayServerManager.Init();

            Console.ReadLine();
        }
    }
}
