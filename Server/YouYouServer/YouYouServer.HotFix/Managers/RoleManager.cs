using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Common.DBData;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.DataTable;

namespace YouYouServer.HotFix
{
    public sealed class RoleManager
    {
        public static async Task<List<RoleBriefEntity>> GetRoleListAsync(long accountId)
        {
            string searchRoleKey = $"{ServerConfig.AreaServerId}_{accountId}_SearchRole";
            List<RoleBriefEntity> retList = new List<RoleBriefEntity>();
            long len = await YFRedisHelper.LLenAsync(searchRoleKey);
            if (len > 0)
            {
                RoleBriefEntity[] retArr = await YFRedisHelper.LRangeAsync<RoleBriefEntity>(searchRoleKey, 0, -1);
                foreach (var item in retArr)
                {
                    retList.Add(item);
                }
            }
            else
            {
                //去Mongo查看
                List<RoleEntity> lst =
                    await DBModelMgr.RoleDBModel.GetListAsync(
                        Builders<RoleEntity>.Filter.Eq(a => a.AccountId, accountId));
                if (lst!=null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        RoleBriefEntity roleBriefEntity = new RoleBriefEntity
                        {
                            RoleId = item.YFId,
                            JobId = item.JobId,
                            Sex = item.Sex,
                            NickName = item.NickName,
                            Level = item.Level
                        };
                        retList.Add(roleBriefEntity);
                        
                        //加入redis
                        await YFRedisHelper.LPushAsync(searchRoleKey, roleBriefEntity);
                    }
                }
            }

            return retList;
        }
        
        /// <summary>
        /// 异步创建角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="jobId"></param>
        /// <param name="sex"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public static async Task<RoleEntity> CreateRoleAsync(long accountId, byte jobId, byte sex, string nickName)
        {
            //1.把nickName写入nickName集合
            long result = await YFRedisHelper.SAddAsync(ServerConfig.RoleNickNameKey, nickName);

            //写入失败 昵称已经存在
            if (result == 0)
            {
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.RoleLog, "CreateRoleAsync Fail NickName In Redis AccountId={0}", accountId);
                return null;
            }

            //2.去Mongodb 查询是否存在
            RoleEntity roleEntity = await DBModelMgr.RoleDBModel.GetEntityAsync(Builders<RoleEntity>.Filter.Eq(a => a.NickName, nickName));

            //如果DB存在 不能 创建角色
            if (roleEntity != null)
            {
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.RoleLog, "CreateRoleAsync Fail NickName In DB AccountId={0}", accountId);
                return null;
            }

            //3.写入MongoDB
            roleEntity = new RoleEntity();
            roleEntity.YFId = await DBModelMgr.UniqueIDGameServer.GetUniqueIDAsync(UniqueIDGameServer.CollectionType.Role.ToInt());
            roleEntity.AccountId = accountId;
            roleEntity.JobId = jobId;
            roleEntity.Sex = sex;
            roleEntity.NickName = nickName;
            roleEntity.Level = 1;
            roleEntity.CreateTime = DateTime.UtcNow;
            roleEntity.UpdateTime = DateTime.UtcNow;
            roleEntity.CurrSceneId = SysConfig.NewRole_InitSceneId;

            //根据初始场景位置 查询出生点坐标
            Vector3 currPos = new Vector3();
            DTSys_SceneEntity dtSysSceneEntity = DataTableManager.Sys_SceneList.GetDic(roleEntity.CurrSceneId);
            currPos.X = dtSysSceneEntity.PlayerBornPos_1;
            currPos.Y = dtSysSceneEntity.PlayerBornPos_2;
            currPos.Z = dtSysSceneEntity.PlayerBornPos_3;
            roleEntity.PosData = currPos;
            
            await DBModelMgr.RoleDBModel.AddAsync(roleEntity);
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.RoleLog, "CreateRoleAsync Success RoleId={0}", roleEntity.YFId);
            
            //加入 Redis
            RoleBriefEntity roleBriefEntity = new RoleBriefEntity()
            {
                RoleId = roleEntity.YFId,
                JobId = roleEntity.JobId,
                Sex = roleEntity.Sex,
                NickName = roleEntity.NickName,
                Level = roleEntity.Level
            };
            string searchRoleKey = $"{ServerConfig.AreaServerId}_{accountId}_SearchRole";
            await YFRedisHelper.LPushAsync(searchRoleKey, roleBriefEntity);
            return roleEntity;
        }

        /// <summary>
        /// 异步获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static async Task<RoleEntity> GetRoleEntityAsync(long roleId)
        {
            RoleEntity roleEntity = await YFRedisHelper.YFCacheShellAsync($"GetRoleEntity_{ServerConfig.AreaServerId}",roleId.ToString(),GetRoleEntityAsync);
            return roleEntity;
        }
        
        /// <summary>
        /// 异步获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static async Task<RoleEntity> GetRoleEntityAsync(string roleId)
        {
            return await DBModelMgr.RoleDBModel.GetEntityAsync(
                Builders<RoleEntity>.Filter.Eq(a => a.YFId, long.Parse(roleId)));
        }

        /// <summary>
        /// 异步保存角色信息
        /// </summary>
        /// <param name="roleEntity"></param>
        public static async Task SaveRoleEntity(RoleEntity roleEntity)
        {
            roleEntity.UpdateTime = DateTime.Now;

            await YFRedisHelper.HSetAsync($"GetRoleEntity_{ServerConfig.AreaServerId}", roleEntity.YFId.ToString(),
                roleEntity);
            await DBModelMgr.RoleDBModel.UpdateAsync(roleEntity);
        }
    }
}