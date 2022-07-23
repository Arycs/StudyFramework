using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using YouYouServer.Common;
using YouYouServer.Core;

namespace YouYouServer.WebAccount.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {

        public string Get(string key)
        {
            switch (key)
            {
                case "ReLoad":
                    HotFixMgr.Load();
                    break;
            }
            return "Complete";
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
