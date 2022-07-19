﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 资源加载器
    /// </summary>
    public class AssetLoaderRoutine 
    {
        /// <summary>
        /// 资源加载请求
        /// </summary>
        private AssetBundleRequest m_CurrAssetBundleRequest;
        
        /// <summary>
        /// 资源请求更新
        /// </summary>
        /// <returns></returns>
        public Action<float> OnAssetUpdate;

        /// <summary>
        /// 加载资源完毕
        /// </summary>
        public Action<UnityEngine.Object> OnLoadAssetComplete;

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="assetBundle"></param>
        public void LoadAsset(string assetName, AssetBundle assetBundle)
        {
            m_CurrAssetBundleRequest = assetBundle.LoadAssetAsync(assetName);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            m_CurrAssetBundleRequest = null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void OnUpdate()
        {
            UpdateAssetBundleRequest();
        }

        /// <summary>
        /// 更新加载资源请求
        /// </summary>
        private void UpdateAssetBundleRequest()
        {
            if (m_CurrAssetBundleRequest != null)
            {
                if (m_CurrAssetBundleRequest.isDone)
                {
                    UnityEngine.Object obj = m_CurrAssetBundleRequest.asset;
                    if (obj!=null)
                    {
                        GameEntry.Log(LogCategory.Resource,"资源 =>{0} 加载完毕",obj.name);
                        Reset(); //一定要早点Reset
                        if (OnLoadAssetComplete != null)
                        {
                            OnLoadAssetComplete(obj);
                        }
                    }
                    else
                    {
                        GameEntry.LogError("资源=>{0} 加载失败", obj.name);
                        Reset(); //一定要早点Reset
                        if (OnLoadAssetComplete != null)
                        {
                            OnLoadAssetComplete(obj);
                        }
                    }
                }
            }
            else
            {
                //加载进度
                if (OnAssetUpdate != null)
                {
                    OnAssetUpdate(m_CurrAssetBundleRequest.progress);
                }
            }
        }
    }
}