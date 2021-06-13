using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    public class HttpManager : ManagerBase
    {
        /// <summary>
        /// 发送Http数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="callBack">回调方法</param>
        /// <param name="isPost">是否是Post请求</param>
        /// <param name="dic">Post请求要处理的数据</param>
        public void SendData(string url, HttpSendDataCallBack callBack, bool isPost = false,bool isGetData = false,
            Dictionary<string, object> dic = null)
        {
            //
            Debug.Log("从池中获取Http访问器");

            HttpRoutine http = GameEntry.Pool.DequeueClassObject<HttpRoutine>();
            http.SendData(url,callBack,isPost,isGetData,dic);
        }

    }
}