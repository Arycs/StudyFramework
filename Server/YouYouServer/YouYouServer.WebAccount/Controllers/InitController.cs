using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouYouServer.Core.Utils;
using YouYouServer.Core.Common;
using YouYouServer.Model.Logic.Entitys;
using YouYouServer.Model.Managers;

namespace YouYouServer.WebAccount.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        public string Get()
        {
            return "OK";
        }

        [HttpPost]
        public string Post()
        {
            string json = Request.HttpContext.Request.Form["json"];
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            string channelId = dic["ChannelId"].ToString();
            string innerVersion = dic["InnerVersion"].ToString();
            ChannelEntity channelEntity = ChannelConfig.Get(channelId, innerVersion);
            RetValue ret = new RetValue();

            if (channelEntity != null)
            {
                Dictionary<string, object> retDic = new Dictionary<string, object>();
                retDic["ServerTime"] = YFDateTimeUtil.GetTimestamp();
                retDic["SourceVersion"] = channelEntity.SourceVersion;
                retDic["SourceUrl"] = channelEntity.SourceUrl;
                retDic["RechargeUrl"] = channelEntity.RechargeUrl;
                retDic["TDAppId"] = channelEntity.TDAppId;
                retDic["IsOpenTD"] = channelEntity.IsOpenTD;
                retDic["PayServerNo"] = channelEntity.PayServerNo;

                ret.Value = JsonConvert.SerializeObject(retDic);
            }
            else
            {
                ret.HasError = true;
            }

           
            return JsonConvert.SerializeObject(ret);

        }
    }
}
