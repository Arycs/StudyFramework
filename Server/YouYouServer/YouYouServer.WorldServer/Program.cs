using System;
using System.Threading.Tasks;
using YouYouServer.Core.Logger;
using YouYouServer.Core.Utils;
using YouYouServer.Model;
using YouYouServer.Model.DataTable;
using YouYouServer.Model.DBModels;
using YouYouServer.Model.Entitys;
using YouYouServer.Model.Logic.DBModels;
using YouYouServer.Model.Logic.Entitys;
using YouYouServer.Model.Managers;
using YouYouServer.Model.ServerManager;
using YouYouServer.Model.Test;

namespace YouYouServer.WorldServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello WorldServer!");
            ServerConfig.Init();
            DataTableManager.Init();
            LoggerMgr.Init();
            YFRedisClient.InitRedisClient();

            WorldServerManager.Init();
            
            Console.ReadLine();
        }
    }
}
