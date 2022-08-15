using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;
using YouYou.Proto;

/// <summary>
/// 用户数据
/// </summary>
public class UserDataManager : IDisposable
{
   /// <summary>
   /// 玩家账号
   /// </summary>
   public long AccountId;
   
   public UserDataManager()
   {
   }

   public void Clear()
   {
   }

   public void Dispose()
   {
      
   }

   public void ReceiveTask()
   {
      
   }

   /// <summary>
   /// 注册客户端
   /// </summary>
   public void RegClient()
   {
      C2GWS_RegClient proto = new C2GWS_RegClient {AccountId = AccountId};
      GameEntry.Socket.SendMainMsg(proto);
   }

   /// <summary>
   /// 获取角色列表
   /// </summary>
   public void GetRoleList()
   {
      C2WS_GetRoleList proto = new C2WS_GetRoleList();
      GameEntry.Socket.SendMainMsg(proto);
   }

   /// <summary>
   /// 服务器返回角色列表
   /// </summary>
   /// <param name="proto"></param>
   public void OnReturnRoleList(WS2C_ReturnRoleList proto)
   {
      if (proto.RoleList.Count == 0)
      {
         GameEntry.UI.OpenUIForm(UIFormId.UI_CreateRole);
      }
   }
}
