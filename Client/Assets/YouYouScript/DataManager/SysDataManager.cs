using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 系统数据管理器
/// </summary>
public class SysDataManager
{
    /// <summary>
    /// 当前服务器时间
    /// </summary>
    public long CurrServerTime;

    /// <summary>
    /// 当前的渠道设置
    /// </summary>
    public ChannelConfigEntity CurrChannelConfig { get; private set; }

    public SysDataManager()
    {
        CurrChannelConfig = new ChannelConfigEntity();
    }

    public void Clear()
    {
      
    }

    public void Dispose()
    {
      
    }
}
