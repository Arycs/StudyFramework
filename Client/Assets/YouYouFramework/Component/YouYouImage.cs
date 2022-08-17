using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace YouYou
{
    public class YouYouImage : Image
    {
        [Header("本地化语言Key")] [SerializeField] private string m_Localization;

        protected override void Start()
        {
            base.Start();
            if (GameEntry.Localization != null && !string.IsNullOrEmpty(m_Localization))
            {
                string path = GameUtil.GetUIResPath(GameEntry.Localization.GetString(m_Localization));

                Texture2D texture = null;

                Sprite obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));

                sprite = obj;

                SetNativeSize();
            }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nativeSize"></param>
        public void LoadImage(string path, bool nativeSize = false)
        {
            GameEntry.Resource.ResourceLoaderManager.LoadMainAsset(AssetCategory.UIRes, GameUtil.GetUIResPath(path),
                onComplete: (
                    entity =>
                    {
                        Texture2D texture = entity.Target as Texture2D;
                        Sprite obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f));
                        sprite = obj;
                        if (nativeSize)
                        {
                            SetNativeSize();
                        }
                    }));
        }
    }
}