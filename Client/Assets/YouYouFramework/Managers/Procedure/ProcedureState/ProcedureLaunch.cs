﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 开始流程
    /// </summary>
    public class ProcedureLaunch : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("OnEnter ProcedureLaunch");
            
            //访问账号服务器
            string url = GameEntry.Http.RealWebAccountUrl + "account?id=2";

            //从对象池中获取字典,用来存储数据,防止是之前的数据,对字典进行Clear
            Dictionary<string, object> dic = GameEntry.Pool.DequeueClassObject<Dictionary<string, object>>();
            dic.Clear();

            GameEntry.Data.SysDataManager.CurrChannelConfig.ChannelId = 0;
            GameEntry.Data.SysDataManager.CurrChannelConfig.InnerVersion = 1001;
            
            dic["Channe1Id"] = GameEntry.Data.SysDataManager.CurrChannelConfig.ChannelId;
            dic["InnerVersion"] = GameEntry.Data.SysDataManager.CurrChannelConfig.InnerVersion;
            GameEntry.Http.SendData(url,OnWebAccountInit,true,true,dic);
        }

        private void TempCheckVersion()
        {
            GameEntry.Procedure.ChangeState(ProcedureState.CheckVersion);
        }

        /// <summary>
        /// HTTP请求回来的数据,服务器没写好,先注释.
        /// </summary>
        /// <param name="args"></param>
        private void OnWebAccountInit(HttpCallBackArgs args)
        {
            Debug.Log("HasError = " + args.HasError);
            Debug.Log("Value = " + args.Value);

            if (!args.HasError)
            {
                LitJson.JsonData data = LitJson.JsonMapper.ToObject(args.Value);
                LitJson.JsonData config = LitJson.JsonMapper.ToObject(data["Value"].ToString());

                GameEntry.Data.SysDataManager.CurrChannelConfig.ServerTime =
                    long.Parse(config["ServerTime"].ToString());
                GameEntry.Data.SysDataManager.CurrChannelConfig.SourceVersion = config["SourceVersion"].ToString();
                GameEntry.Data.SysDataManager.CurrChannelConfig.SourceUrl = config["SourceUrl"].ToString();
                GameEntry.Data.SysDataManager.CurrChannelConfig.RechargeUrl = config["RechargeUrl"].ToString();
                GameEntry.Data.SysDataManager.CurrChannelConfig.TDAppId = config["TDAppId"].ToString();
                GameEntry.Data.SysDataManager.CurrChannelConfig.IsOpenTD = int.Parse(config["IsOpenTD"].ToString()) == 1;
             
                Debug.Log("RealSourceUrl" + GameEntry.Data.SysDataManager.CurrChannelConfig.RealSourceUrl);
                
                //连接上服务器, 切换检查版本更新流程
                GameEntry.Procedure.ChangeState(ProcedureState.CheckVersion);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //Debug.Log("OnUpdate ProcedureLaunch");
            if (Input.GetKeyDown(KeyCode.A))
            {
                TempCheckVersion();
            }
        }

        public override void OnLeave()
        {
            base.OnLeave();
            Debug.Log("OnLeave ProcedureLaunch");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

}
