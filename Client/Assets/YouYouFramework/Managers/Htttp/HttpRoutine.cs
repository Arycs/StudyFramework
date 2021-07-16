using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace YouYou
{
    /// <summary>
    /// Http发送数据的回调委托
    /// </summary>
    /// <param name="args"></param>
    public delegate void HttpSendDataCallBack(HttpCallBackArgs args);

    /// <summary>
    /// Http访问器
    /// </summary>
    public class HttpRoutine
    {
        #region 属性

        /// <summary>
        /// Http请求回调
        /// </summary>
        private HttpSendDataCallBack m_CallBack;

        /// <summary>
        /// Http请求回调数据
        /// </summary>
        private HttpCallBackArgs m_CallBackArgs;

        /// <summary>
        /// 是否繁忙
        /// </summary>
        public bool IsBusy { get; private set; }

        /// <summary>
        /// 是否获取data数据
        /// </summary>
        private bool m_IsGetData = false;
        
        #endregion

        public HttpRoutine()
        {
            m_CallBackArgs = new HttpCallBackArgs();
        }

        #region SendData 发送web数据
        /// <summary>
        /// 发送web数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="isPost">是否是Post请求</param>
        /// <param name="dic">Post需要处理的数据</param>
        public void SendData(string url, HttpSendDataCallBack callBack, bool isPost = false, bool isGetData = false,
            Dictionary<string, object> dic = null)
        {
            if (IsBusy) return;

            IsBusy = true;
            m_CallBack = callBack;
            m_IsGetData = isGetData;
            if (!isPost)
            {
                GetUrl(url);
            }
            else
            {
                //web加密
                if (dic != null)
                {
                    //客户端标识符
                    dic["deviceIdentifier"] = DeviceUtil.DeviceIdentifier;

                    //设备型号
                    dic["deviceModel"] = DeviceUtil.DeviceModel;

                    long t = GameEntry.Data.SysDataManager.CurrServerTime;
                    //签名
                    dic["sign"] = EncryptUtil.Md5(string.Format("{0}:{1}", t, DeviceUtil.DeviceIdentifier));

                    //时间戳
                    dic["t"] = t;
                }

                string json = string.Empty;
                if (dic != null)
                {
                    json = JsonMapper.ToJson(dic);
#if DEBUG_LOG_PROTO
                    Debug.Log("<color=#ffa200>发送消息:</color><color=#fffb80>" + url +"</color>");
                    Debug.Log("<color=#ffdeb3>==>>" + json + "</color>");
#endif
                    GameEntry.Pool.EnqueueClassObject(dic);
                }

                PostUrl(url, json);
            }
        }

        #endregion

        #region GetUrl Get请求,从url中请求数据
        /// <summary>
        /// Get请求,从url中请求数据
        /// </summary>
        /// <param name="url">地址</param>
        private void GetUrl(string url)
        {
            UnityWebRequest data = UnityWebRequest.Get(url);
            GameEntry.Http.StartCoroutine(Request(data));
        }

        #endregion

        #region PostUrl Post请求,向url提交要被处理的数据
        /// <summary>
        /// Post请求,向url提交要被处理的数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="json">要处理的数据</param>
        private void PostUrl(string url, string json)
        {
            //定义一个表单
            WWWForm form = new WWWForm();
            form.AddField("json", json);

            UnityWebRequest data = UnityWebRequest.Post(url, form);

            GameEntry.Http.StartCoroutine(Request(data));
        }

        #endregion

        #region Request 请求服务器
        /// <summary>
        /// 请求服务器
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerator Request(UnityWebRequest data)
        {
            //发送请求
            yield return data.SendWebRequest();

            IsBusy = false;
            // WebRequest有错误, 回调参数记录错误
            if (data.isNetworkError || data.isHttpError)
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = true;
                    m_CallBackArgs.Value = data.error;

                    if (!m_IsGetData)
                    {
                        GameEntry.Log(LogCategory.Proto,"<color=#00eaff>接收消息:</color><color=#00ff9c>" + data.url +"</color>");
                        GameEntry.Log(LogCategory.Proto,"<color=#c5e1dc>==>>" + JsonUtility.ToJson(m_CallBackArgs) + "</color>");
                    }
                    m_CallBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Value = data.downloadHandler.text;
                    if (!m_IsGetData)
                    {
                        GameEntry.Log(LogCategory.Proto,"<color=#00eaff>接收消息:</color><color=#00ff9c>" + data.url +"</color>");
                        GameEntry.Log(LogCategory.Proto,"<color=#c5e1dc>==>>" + JsonUtility.ToJson(m_CallBackArgs) + "</color>");
                    }

                    m_CallBackArgs.Data = data.downloadHandler.data;
                    m_CallBack(m_CallBackArgs);
                }
            }
            m_CallBackArgs.Data = null;
            data.Dispose();
            data = null;
            
            //Debug.Log("把Http访问器对象回池");
            GameEntry.Pool.EnqueueClassObject(this);
        }

        #endregion
    }
}