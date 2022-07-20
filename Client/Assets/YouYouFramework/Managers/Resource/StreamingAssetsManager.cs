using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// StreamingAssets管理器
    /// </summary>
    public class StreamingAssetsManager
    {
        /// <summary>
        /// StreamingAssets资源路径
        /// </summary>
        private string m_StreamingAssetsPath;

        public StreamingAssetsManager()
        {
            m_StreamingAssetsPath = "file://" + Application.streamingAssetsPath;
#if UNITY_ANDROID && !UNITY_EDITOR
            m_StreamingAssetsPath = Application.streamingAssetsPath;
#endif
        }

        #region ReadStreamingAsset 读取StreamingAssets下的资源

        /// <summary>
        /// 读取StreamingAssets下的资源
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async UniTask<byte[]> ReadStreamingAsset(string url)
        {
            using (WWW www = new WWW(url))
            {
                await www;
                return www.error == null ? www.bytes : null;
            }
        }

        #endregion

        #region ReadAssetBundle 读取只读区资源包
        /// <summary>
        /// 读取只读区资源包
        /// </summary>
        /// <param name="fileUrl">资源路径</param>
        /// <param name="onComplete"></param>
        public async UniTask<byte[]> ReadAssetBundle(string fileUrl)
        {
           return await ReadStreamingAsset($"{m_StreamingAssetsPath}/AssetBundles/{fileUrl}");
           
        }
        #endregion
    }
}