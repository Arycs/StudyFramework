using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YouYouServer.Common;
using YouYouServer.Core.Logger;
using YouYouServer.Core.Utils;
using YouYouServer.Model.Entitys;
using YouYouServer.Model.Logic.DBModels;

namespace YouYouServer.Model.Managers
{
    public sealed class RoleManager
    {
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
            //1. 把nickName 写入nickName集合
            long result = await YFRedisHelper.SAddAsync(ServerConfig.RoleNickNameKey, nickName);

            //写入失败 昵称已存在
            if (result == 0)
            {
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.RoleLog, "CreateRoleAsync Fail NickNameIn Redis AccountId = {0}", accountId);
                return null;
            }

            // 2. 去Mongodb 查询是否存在
            RoleEntity roleEntity = await DBModelMgr.RoleDBModel.GetEntityAsync(Builders<RoleEntity>.Filter.Eq(a => a.NickName, nickName));

            //如果DB 存在 不能创建角色
            if (roleEntity != null)
            {
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.RoleLog, "CreateRoleAsync Fail NickName In DB AccountId = {0}", accountId);
                return null;
            }

            //3. 写入MongoDB
            roleEntity = new RoleEntity();
            roleEntity.YFId = await DBModelMgr.UniqueIDAccount.GetUniqueIDAsync((int)ConstDefine.CollectionType.Role);
            roleEntity.AccountId = accountId;
            roleEntity.JobId = jobId;
            roleEntity.Sex = sex;
            roleEntity.NickName = nickName;
            roleEntity.Level = 1;
            roleEntity.CreateTime = DateTime.UtcNow;
            roleEntity.UpdateTime = DateTime.UtcNow;

            await DBModelMgr.RoleDBModel.AddAsync(roleEntity);

            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.RoleLog, "CreateRoleAsync Success RoleId = {0}", roleEntity.AccountId);

            return roleEntity;
        }
    }
}
