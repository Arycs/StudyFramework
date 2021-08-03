using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class ConstDefine
    {
        /// <summary>
        /// 版本文件名称
        /// </summary>
        public const string VersionFileName = "VersionFile.bytes";

        /// <summary>
        /// 资源版本号
        /// </summary>
        public const string ResourceVersion = "ResourceVersion";

        /// <summary>
        /// 资源信息文件名称
        /// </summary>
        public const string AssetInfoName = "AssetInfo.bytes";

        /// <summary>
        /// lua脚本资源包路径
        /// </summary>
        public const string XLuaAssetBundlePath = "download/xlualogic.assetbundle";

        /// <summary>
        /// 自定义Shader资源包路径
        /// </summary>
        public const string CusShaderAssetBundlePath = "download/cusshaders.assetbundle";

        /// <summary>
        /// 声音资源包路径
        /// </summary>
        public const string AudioAssetBundlePath = "download/audio.assetbundle";
        
        /// <summary>
        /// 点击按钮声音
        /// </summary>
        public const int Audio_ButtonClick = 201;

        /// <summary>
        /// UI关闭声音
        /// </summary>
        public const int Audio_UIClose = 202;

        /// <summary>
        /// Http重试
        /// </summary>
        public const string Http_Retry = "Http_Retry";
    }
}