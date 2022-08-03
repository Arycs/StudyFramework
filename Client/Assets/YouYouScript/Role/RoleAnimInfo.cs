using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// 角色动画信息
/// </summary>
public class RoleAnimInfo
{
    /// <summary>
    /// 索引号
    /// </summary>
    public int Index = 0;

    /// <summary>
    /// 当前动画剪辑
    /// </summary>
    public AnimationClipPlayable CurrPlayable;

    /// <summary>
    /// 当前的动画数据
    /// </summary>
    public DTRoleAnimationEntity CurrRoleAnimationData;

    /// <summary>
    /// 最后使用时间
    /// </summary>
    public float LastUseTime;

    /// <summary>
    /// 是否已经加载
    /// </summary>
    public bool IsLoad;

    /// <summary>
    /// 动画是否正在播放
    /// </summary>
    public bool IsPlaying;

    /// <summary>
    /// 动画是否过期
    /// </summary>
    public bool IsExpire => CurrRoleAnimationData.InitLoad == 0 && Time.time > LastUseTime + CurrRoleAnimationData.Expire && !IsPlaying && IsLoad;
}


