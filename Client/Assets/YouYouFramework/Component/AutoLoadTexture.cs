using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XLua;

/// <summary>
/// �Զ�����ͼƬ
/// </summary>
public class AutoLoadTexture : MonoBehaviour
{
    /// <summary>
    /// ͼƬ����
    /// </summary>
    public string ImgName;

    /// <summary>
    /// ͼƬ·��
    /// </summary>
    public string ImgPath;

    /// <summary>
    /// �Ƿ�����ͼƬԭ����С
    /// </summary>
    public bool IsSetNativeSize;

    void Start()
    {
    }

    public void SetImg()
    {
        Image img = GetComponent<Image>();

        if (img != null && !string.IsNullOrEmpty(ImgPath))
        {
//            AssetBundleMgr.Instance.LoadOrDownload<Object>(ImgPath, ImgName, (UnityEngine.Object asset, object userData) =>
//            {
//                Sprite obj = null;
//                if (asset is Sprite)
//                {
//                    obj = (Sprite)asset;
//                }
//                else if (asset is Texture2D)
//                {
//                    Texture2D texture = asset as Texture2D;
//                    obj = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//                }
//
//                if (img == null) return;
//
//                img.overrideSprite = obj;
//                if (IsSetNativeSize)
//                {
//                    img.SetNativeSize();
//                }
//            }, type: 1);
        }
    }
}