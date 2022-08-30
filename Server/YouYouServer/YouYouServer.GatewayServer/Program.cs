using System;
using YouYouServer.Commmon;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model;

namespace YouYouServer.GatewayServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello GatewayServer!");
            TimerManager.Init();
            HotFixConfig.Load();
            HotFixHelper.LoadHotFixAssembly();
            ServerConfig.Init();
            DataTableManager.Init();
            LoggerMgr.Init();
            YFRedisClient.InitRedisClient();
            GatewayServerManager.Init();

            Console.ReadLine();
        }
    }
}
