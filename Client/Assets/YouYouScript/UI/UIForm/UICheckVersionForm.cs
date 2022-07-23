using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YouYou;

public class UICheckVersionForm : UIFormBase
{
    [SerializeField] private Text txtTip;

    [SerializeField] private Text txtSize;

    [SerializeField] private Text txtResourceVersion;

    [SerializeField] private Scrollbar scrollbar;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        GameEntry.Event.CommonEvent.AddEventListener(SysEventId.CheckVersionBeginDownload, OnCheckVersionBeginDownload);
        GameEntry.Event.CommonEvent.AddEventListener(SysEventId.CheckVersionDownloadUpdate,
            OnCheckVersionDownloadUpdate);
        GameEntry.Event.CommonEvent.AddEventListener(SysEventId.CheckVersionDownloadComplete,
            OnCheckVersionDownloadComolete);

        GameEntry.Event.CommonEvent.AddEventListener(SysEventId.PreloadBegin, OnPreloadBegin);
        GameEntry.Event.CommonEvent.AddEventListener(SysEventId.PreloadUpdate, OnPreloadUpdate);
        GameEntry.Event.CommonEvent.AddEventListener(SysEventId.PreloadComplete, OnPreloadComplete);

        GameEntry.Event.CommonEvent.AddEventListener(SysEventId.CloseCheckVersionUI,OnCloseCheckVersionUI);
        
        txtTip.gameObject.SetActive(false);
        txtSize.gameObject.SetActive(false);
        scrollbar.gameObject.SetActive(false);
    }

    #region 检查更新事件

    private void OnCheckVersionBeginDownload(object userData)
    {
        txtTip.gameObject.SetActive(true);
        txtSize.gameObject.SetActive(true);
        scrollbar.gameObject.SetActive(true);

        Debug.Log("==============>>>>>>>>>>>>>检查版本 资源版本号  ");
        txtResourceVersion.text = $"资源版本号 {GameEntry.Resource.ResourceManager.CDNVersion}";
    }

    private void OnCheckVersionDownloadUpdate(object userData)
    {
        BaseParams args = userData as BaseParams;

        txtTip.text = $"正在下载{args.IntParam1}/{args.IntParam2}";
        scrollbar.size = (float) args.IntParam1 / args.IntParam2;

        txtSize.text = $"{(float) args.ULongParam1 / (1024 * 1024):f2}M/{(float) args.ULongParam2 / (1024 * 1024):f2}M";
        Debug.Log("==============>>>>>>>>>>>>>检查版本 正在下载  ");

    }

    private void OnCheckVersionDownloadComolete(object userData)
    {
        
    }

    #endregion

    #region 预加载事件

    private void OnPreloadBegin(object userData)
    {
        txtTip.gameObject.SetActive(true);
        scrollbar.gameObject.SetActive(true);
        txtSize.gameObject.SetActive(false);
        txtResourceVersion.text = $"资源版本号 {GameEntry.Resource.ResourceManager.CDNVersion}";
        Debug.Log("==============>>>>>>>>>>>>>预加载 资源版本号  ");

    }

    private void OnPreloadUpdate(object userData)
    {
        BaseParams args = userData as BaseParams;

        txtTip.text = $"正在加载资源 {Mathf.Min(args.FloatParam1, 100):f0}%";
        scrollbar.size = args.FloatParam1 * 0.01f;
        Debug.Log("==============>>>>>>>>>>>>>预加载 加载  ");

    }

    private void OnPreloadComplete(object userData)
    {

    }

    private void OnCloseCheckVersionUI(object userData)
    {
        Destroy(gameObject); //临时

    }

    #endregion

    protected override void OnBeforeDestroy()
    {
        base.OnBeforeDestroy();
        GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.CheckVersionBeginDownload,
            OnCheckVersionBeginDownload);
        GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.CheckVersionDownloadUpdate,
            OnCheckVersionDownloadUpdate);
        GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.CheckVersionDownloadComplete,
            OnCheckVersionDownloadComolete);

        GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.PreloadBegin, OnPreloadBegin);
        GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.PreloadUpdate, OnPreloadUpdate);
        GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.PreloadComplete, OnPreloadComplete);

        GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.CloseCheckVersionUI, OnCloseCheckVersionUI);
    }
}