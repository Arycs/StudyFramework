using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;
using YouYou.Proto;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 用户数据
/// </summary>
public class UserDataManager : IDisposable
{
    /// <summary>
    /// 玩家账号
    /// </summary>
    public long AccountId;

    public int CurrJobId;

    public long CurrRoleId;
    public MyCommonEnum.Sex Sex;
    public string NickName;
    public int Level;
    public int CurrSceneId;
    public Vector3 CurrPos;
    
    /// <summary>
    /// 服务器返回角色列表数据
    /// </summary>
    public WS2C_ReturnRoleList ReturnRoleListData;

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
        ReturnRoleListData = proto;
        var roleCount = ReturnRoleListData.RoleList.Count;
        if (roleCount == 0)
        {
            GameEntry.UI.OpenUIForm(UIFormId.UI_CreateRole, roleCount);
        }
        else
        {
            GameEntry.UI.OpenUIForm(UIFormId.UI_SelectRole);
        }
    }

    /// <summary>
    /// 设置当前职业ID
    /// </summary>
    /// <param name="currJobId"></param>
    public void SetCurrJobId(int currJobId)
    {
        CurrJobId = currJobId;
    }

    /// <summary>
    /// 设置当权角色ID
    /// </summary>
    /// <param name="currRoleId"></param>
    public void SetCurrRoleId(long currRoleId)
    {
        CurrRoleId = currRoleId;
    }

    /// <summary>
    /// 服务器返回创建角色消息
    /// </summary>
    /// <param name="proto"></param>
    public void OnCreateRole(WS2C_ReturnCreateRole proto)
    {
        if (proto.Result)
        {
            CurrRoleId = proto.RoleId;
            GameEntry.Procedure.ChangeState(ProcedureState.EnterGame);
        }
        else
        {
            //根据错误码 弹出提示
            GameEntry.LogError("创建角色失败");
        }
    }

    public void EnterGame()
    {
        C2WS_EnterGame proto = new C2WS_EnterGame();
        proto.RoleId = CurrRoleId;
        GameEntry.Socket.SendMainMsg(proto);
    }

    public void OnReturnRoleInfo(WS2C_ReturnRoleInfo proto)
    {
        CurrRoleId = proto.RoleId;
        CurrJobId = proto.JobId;
        Sex = (MyCommonEnum.Sex) proto.Sex;
        NickName = proto.NickName;
        Level = proto.Level;
        CurrSceneId = proto.CurrSceneId;
        CurrPos = new UnityEngine.Vector3(proto.CurrPos.X, proto.CurrPos.Y, proto.CurrPos.Z);
    }

    /// <summary>
    /// /服务器返回进入游戏消息
    /// </summary>
    public void OnEnterGameComplete()
    {
        EnterSceneApply(CurrSceneId);
    }

    /// <summary>
    /// 进入场景申请
    /// </summary>
    /// <param name="sceneId"></param>
    private void EnterSceneApply(int sceneId)
    {
        C2GWS_EnterScene_Apply proto = new C2GWS_EnterScene_Apply();
        proto.SceneId = sceneId;
        GameEntry.Socket.SendMainMsg(proto);
    }

    /// <summary>
    /// 服务器返回进入场景申请消息
    /// </summary>
    /// <param name="proto"></param>
    public void OnReturnEnterSceneApply(GS2C_ReturnEnterScene_Apply proto)
    {
        if (proto.Result)
        {
            CurrSceneId = proto.SceneId;
            CurrPos = new Vector3(proto.CurrPos.X, proto.CurrPos.Y, proto.CurrPos.Z);
            GameEntry.Procedure.ChangeState(ProcedureState.WorldMap);
        }
        else
        {
            //TODO 弹出提示
        }
    }

    /// <summary>
    /// 进入场景
    /// </summary>
    /// <param name="sceneId"></param>
    public void EnterScene(int sceneId)
    {
        C2GWS_EnterScene proto = new C2GWS_EnterScene();
        proto.SceneId = sceneId;
        GameEntry.Socket.SendMainMsg(proto);
    }

    /// <summary>
    /// 服务器返回场景中已有角色消息
    /// </summary>
    /// <param name="prot"></param>
    public void OnReturnSceneLineRoleList(GS2C_ReturnSceneLineRoleList prot)
    {
        
    }

}