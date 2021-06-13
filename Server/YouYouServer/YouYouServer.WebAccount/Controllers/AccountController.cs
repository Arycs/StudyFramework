using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            AccountEntity accountEntity =await YFRedisHelper.YFCacheShellAsync("youyouAccount",id.ToString(),GetAccount);
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
