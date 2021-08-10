using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户数据
/// </summary>
public class UserDataManager : IDisposable
{
    /// <summary>
    /// 共享用户数据
    /// </summary>
    public ShareUserData ShareUserData;

   public UserDataManager()
   {
        ShareUserData = new ShareUserData();
   }

   public void Clear()
   {
        ShareUserData.Dispose();
   }

   public void Dispose()
   {
      
   }

   public void ReceiveTask()
   {
      
   }
}
