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
using YouYouServer.Model.Test;

namespace YouYouServer.WorldServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerConfig.Init();
            DataTableManager.Init();
            LoggerMgr.Init();
            YFRedisClient.InitRedisClient();

            Sys_UIFormEntity Sys_UIFormEntity = DataTableManager.Sys_UIFormDBModel.Get(1);
            Console.WriteLine("Sys_UIFormEntity" + Sys_UIFormEntity.Id + ":Sys_UIFormEntity" + Sys_UIFormEntity.Desc);

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        private static async void Add()
        {
            //加入MongoDB
            RoleEntity roleEntity = new RoleEntity();
            roleEntity.YFId = DBModelMgr.UniqueIDGameServer.GetUniqueID((int)CollectionType.Role);
            roleEntity.NickName = "傲然于尘世丶" + new Random().Next(1, 888);
            roleEntity.Level = 1;
            roleEntity.CreateTime = DateTime.Now;
            roleEntity.UpdateTime = DateTime.Now;

            roleEntity.TaskList.Add(new TaskEntity() { TaskId = 1, CurrStatus = 0 });
            roleEntity.TaskList.Add(new TaskEntity() { TaskId = 2, CurrStatus = 1 });

            roleEntity.SkillDic[1] = new SkillEntity() { SkillId = 1, CurrLevel = 2 };
            roleEntity.SkillDic[2] = new SkillEntity() { SkillId = 2, CurrLevel = 1 };
            roleEntity.SkillDic[3] = new SkillEntity() { SkillId = 3, CurrLevel = 1 };

            await DBModelMgr.RoleDBModel.AddAsync(roleEntity);
            LoggerMgr.Log(Core.LoggerLevel.Log, 0, "Add Role YFID = {0}", roleEntity.YFId);

            //加入 Redis 的hash
            await RedisHelper.HSetAsync(ServerConfig.RoleHashKey, roleEntity.YFId.ToString(), roleEntity);
        }

        private static async void GetRoleFromRedis(long roleId)
        {
            //RoleEntity entity = await RedisHelper.HGetAsync<RoleEntity>(GameServerConfig.RoleHashKey, roleId.ToString());

            // RoleEntity entity = await RedisHelper.CacheShellAsync<RoleEntity>(GameServerConfig.RoleHashKey, roleId.ToString(), -1, AddRole);
            RoleEntity entity = await YFRedisHelper.YFCacheShellAsync(ServerConfig.RoleHashKey, roleId.ToString(), GetRoleFromMongodb);
            if (entity != null)
            {
                Console.WriteLine("nickName = " + entity.NickName);
            }
            else
            {
                Console.WriteLine("数据不存在");
            }
        }

        private static async Task<RoleEntity> GetRoleFromMongodb(string field)
        {
            //从 Mongodb中读取
            return await DBModelMgr.RoleDBModel.GetEntityAsync(long.Parse(field));
        }
    }
}
