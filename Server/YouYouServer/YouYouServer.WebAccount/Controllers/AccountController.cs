using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouYouServer.Core;
using YouYouServer.Core.Common;
using YouYouServer.Core.Utils;
using YouYouServer.Model.DataTable;
using YouYouServer.Model.Logic.DBModels;
using YouYouServer.Model.Logic.Entitys;
using YouYouServer.Model.Managers;

namespace YouYouServer.WebAccount.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public async Task<string> Post()
        {
            string json = Request.HttpContext.Request.Form["json"];
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            RetValue ret = new RetValue();

            //时间戳
            long t = dic["t"].ToLong();
            string deviceIdentifier = dic["deviceIdentifier"].ToString();
            string deviceModel = dic["deviceModel"].ToString();
            string sign = dic["sign"].ToString();

            //1.判断时间戳 如果大于3秒，直接返回错误
            if (YFDateTimeUtil.GetTimestamp() - t > 30)
            {
                ret.HasError = true;
                ret.ErrorCode = 10001;
                return JsonConvert.SerializeObject(ret);
            }

            //2.验证签名
            string signServer = YFEncryptUtil.Md5(string.Format("{0}:{1}", t, deviceIdentifier));
            if (!signServer.Equals(sign,StringComparison.CurrentCultureIgnoreCase))
            {
                ret.HasError = true;
                ret.ErrorCode = 10002;
                return JsonConvert.SerializeObject(ret);
            }

            int type = dic["Type"].ToInt();
            string userName = dic["UserName"].ToString();
            string pwd = dic["Password"].ToString();
            short channelId = dic["ChannelId"].ToShort();

            if (type == 0)
            {
                //注册
                AccountEntity accountEntity = await RegisterAsync(userName, pwd, channelId, deviceIdentifier, deviceModel);
                if (accountEntity == null)
                {
                    ret.HasError = true;
                    ret.ErrorCode = 10003;
                    return JsonConvert.SerializeObject(ret);
                }

                ret.Value = JsonConvert.SerializeObject(accountEntity);
            }
            else if(type == 1)
            {
                //登录
                AccountEntity accountEntity = await LoginAsync(userName, pwd, channelId, deviceIdentifier, deviceModel);
                if (accountEntity == null)
                {
                    ret.HasError = true;
                    ret.ErrorCode = 10004;
                    return JsonConvert.SerializeObject(ret);
                }
                ret.Value = JsonConvert.SerializeObject(accountEntity);
            }

            return JsonConvert.SerializeObject(ret);
        }

        #region LoginAsync 异步登录
        /// <summary>
        /// 异步登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="channelId"></param>
        /// <param name="deviceIdentifier"></param>
        /// <param name="deviceModel"></param>
        /// <returns></returns>
        private async Task<AccountEntity> LoginAsync(string userName, string pwd, short channelId, string deviceIdentifier, string deviceModel)
        {
            AccountEntity accountEntity = await YFRedisHelper.YFCacheShellAsync("Login_User", string.Format("{0}^{1}", userName, pwd), GetAccountAsync);
            // 如果账号存在 并且换了设备 更新设备信息
            if (accountEntity != null && (accountEntity.ChannelId != channelId || !accountEntity.DeviceIdentifier.EndsWith(deviceIdentifier,StringComparison.CurrentCultureIgnoreCase)|| !accountEntity.DeviceModel.EndsWith(deviceModel,StringComparison.CurrentCultureIgnoreCase)))
            {
                accountEntity.ChannelId = channelId;
                accountEntity.DeviceIdentifier = deviceIdentifier;
                accountEntity.DeviceModel = deviceModel;

                await DBModelMgr.AccountDBModel.UpdateAsync(accountEntity);
            }

            return accountEntity;
        }
        #endregion

        #region RegisterAsync 异步注册
        /// <summary>
        /// 异步注册方法
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="channelId"></param>
        /// <param name="deviceIdentifier"></param>
        /// <param name="deviceModel"></param>
        /// <returns></returns>
        private async Task<AccountEntity> RegisterAsync(string userName, string pwd, short channelId, string deviceIdentifier, string deviceModel)
        {
            //1.把UserName 写入UserName 集合
            long result = await YFRedisHelper.SAddAsync("Register_UserName", userName);

            //写入失败
            if (result == 0)
            {
                return null;
            }

            //2.去Mongodb 查询是否存在
            AccountEntity accountEntity = await DBModelMgr.AccountDBModel.GetEntityAsync(Builders<AccountEntity>.Filter.Eq(a => a.UserName, userName));

            //如果DB 存在 不能注册
            if (accountEntity != null)
            {
                return null;
            }

            //3.写入DBModel 
            accountEntity = new AccountEntity();
            accountEntity.YFId = await DBModelMgr.UniqueIDAccount.GetUniqueIDAsync(0);

            accountEntity.ChannelId = channelId;
            accountEntity.DeviceIdentifier = deviceIdentifier;
            accountEntity.DeviceModel = deviceModel;

            accountEntity.UserName = userName;
            accountEntity.Password = pwd;
            accountEntity.CreateTime = DateTime.UtcNow;
            accountEntity.UpdateTime = DateTime.UtcNow;

            await DBModelMgr.AccountDBModel.AddAsync(accountEntity);
            return accountEntity;
        }
        #endregion

        /// <summary>
        /// 异步查询账户实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<AccountEntity> GetAccountAsync(string key)
        {
            string[] arr = key.Split("^");
            if (arr.Length == 2)
            {
                return await DBModelMgr.AccountDBModel.GetEntityAsync(Builders<AccountEntity>.Filter.And(Builders<AccountEntity>.Filter.Eq(a => a.UserName, arr[0]),
                    Builders<AccountEntity>.Filter.Eq(a => a.Password, arr[1])));
            }
            return null;
        }


        [HttpGet]
        public async Task<string> Get(long id)
        {
            //==========================读表获取数据===============
            //Sys_UIFormEntity sys_UIFormEntity = DataTableManager.Sys_UIFormDBModel.Get(id);
            //return Newtonsoft.Json.JsonConvert.SerializeObject(sys_UIFormEntity);

            //==========================向Mongo中添加数据===============

            //AccountEntity accountEntity = new AccountEntity();
            //accountEntity.YFId = await DBModelMgr.UniqueIDAccount.GetUniqueIDAsync(0);
            //accountEntity.UserName = "Arycs2";
            //accountEntity.Password = "123456";
            //accountEntity.CreateTime = DateTime.UtcNow;
            //accountEntity.UpdateTime = DateTime.UtcNow;

            //await DBModelMgr.AccountDBModel.AddAsync(accountEntity);

            //return accountEntity.YFId.ToString();


            //==========================从Redis中获取数据===============
            string str = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;
            AccountEntity accountEntity = await YFRedisHelper.YFCacheShellAsync("youyouAccount", id.ToString(), GetAccount);
            return JsonConvert.SerializeObject(accountEntity) + "server = " + str;
        }

        private async Task<AccountEntity> GetAccount(string arg)
        {
            long yfId = 0;
            long.TryParse(arg, out yfId);

            return await DBModelMgr.AccountDBModel.GetEntityAsync(yfId);
        }
    }
}
