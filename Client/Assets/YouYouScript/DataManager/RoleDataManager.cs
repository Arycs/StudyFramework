using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;
using YouYou.Proto;
using Vector3 = UnityEngine.Vector3;

public class RoleDataManager : IDisposable
{
    /// <summary>
    /// 当前已经加载的角色
    /// </summary>
    private LinkedList<RoleCtrl> m_RoleList;

    /// <summary>
    /// 当前PVP场景中的角色字典
    /// </summary>
    private Dictionary<long, RoleCtrl> m_CurrPVPSceneRoleDic;

    /// <summary>
    /// 当前玩家
    /// </summary>
    public RoleCtrl CurrPlayer { get; set; }

    /// <summary>
    /// 通过摇杆控制玩家移动时的一个辅助gameObject
    /// </summary>
    public GameObject CurrPlayerMoveHelper { get; }

    public RoleDataManager()
    {
        m_RoleList = new LinkedList<RoleCtrl>();
        m_CurrPVPSceneRoleDic = new Dictionary<long, RoleCtrl>();
        CurrPlayerMoveHelper = new GameObject("CurrPlayerMoveHelper");
    }

    /// <summary>
    /// 根据职业编号创建角色
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="onComplete"></param>
    public void CreatePlayerByJobId(int jobId, BaseAction<RoleCtrl> onComplete = null)
    {
        //角色ID
        int baseRoleId = GameEntry.DataTable.JobList.Get(jobId).BaseRoleId;
        //加载角色控制器
        GameEntry.Pool.GameObjectSpawn(SysPrefabId.RoleCtrl, (Transform trans, bool isNewInstance) =>
        {
            RoleCtrl roleCtrl = trans.GetComponent<RoleCtrl>();
            roleCtrl.InitPlayerData(baseRoleId);

            if (!isNewInstance)
            {
                //如果不是新实例,在这里执行OnOpen方法
                roleCtrl.OnOpen();
            }

            m_RoleList.AddLast(roleCtrl);
            onComplete?.Invoke(roleCtrl);
        });
    }

    /// <summary>
    /// 创建怪
    /// </summary>
    /// <param name="spriteId"></param>
    /// <param name="onComplete"></param>
    public void CreateSprite(int spriteId, BaseAction<RoleCtrl> onComplete = null)
    {
        //加载角色控制器
        GameEntry.Pool.GameObjectSpawn(SysPrefabId.RoleCtrl, (Transform trans, bool isNewInstance) =>
        {
            RoleCtrl roleCtrl = trans.GetComponent<RoleCtrl>();
            roleCtrl.InitSpriteData(spriteId);

            if (!isNewInstance)
            {
                //如果不是新实例,在这里执行OnOpen方法
                roleCtrl.OnOpen();
            }

            m_RoleList.AddLast(roleCtrl);
            onComplete?.Invoke(roleCtrl);
        });
    }


    /// <summary>
    /// 角色回池
    /// </summary>
    /// <param name="roleCtrl"></param>
    public void DeSpawnRole(RoleCtrl roleCtrl)
    {
        //先执行角色回池的方法 把角色以来的其他零件回池
        roleCtrl.OnClose();
        //然后回池
        GameEntry.Pool.GameObjectDeSpawn(roleCtrl.transform);
        m_RoleList.Remove(roleCtrl);
    }

    /// <summary>
    /// 回池所有角色
    /// </summary>
    public void DeSpawnAllRole()
    {
        for (LinkedListNode<RoleCtrl> curr = m_RoleList.First; curr != null;)
        {
            var next = curr.Next;
            DeSpawnRole(curr.Value);
            curr = next;
        }
    }

    public void Dispose()
    {
        m_RoleList.Clear();
    }

    /// <summary>
    /// 检查需要卸载的角色动画
    /// </summary>
    public void CheckUnLoadRoleAnimation()
    {
        for (var curr = m_RoleList.First; curr != null;)
        {
            var next = curr.Next;
            curr.Value.CheckUnLoadRoleAnimation();
            curr = next;
        }
    }

    /// <summary>
    /// /服务器返回进入游戏消息
    /// </summary>
    public void OnEnterGameComplete()
    {
        EnterSceneApply(GameEntry.Data.UserDataManager.CurrSceneId);
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
            GameEntry.Data.UserDataManager.CurrSceneId = proto.SceneId;
            GameEntry.Data.UserDataManager.CurrPos =
                new UnityEngine.Vector3(proto.CurrPos.X, proto.CurrPos.Y, proto.CurrPos.Z);
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

        m_CurrPVPSceneRoleDic[GameEntry.Data.RoleDataManager.CurrPlayer.ServerRoleId] =
            GameEntry.Data.RoleDataManager.CurrPlayer;
    }


    /// <summary>
    /// 服务器返回场景中已有角色消息
    /// </summary>
    /// <param name="proto"></param>
    public void OnReturnSceneLineRoleList(GS2C_ReturnSceneLineRoleList proto)
    {
        int len = proto.RoleList.Count;
        for (int i = 0; i < len; i++)
        {
            WS2C_SceneLineRole_DATA data = proto.RoleList[i];
            LoadSceneLineRole(data);
        }
    }

    /// <summary>
    /// 服务器返回角色离开场景线
    /// </summary>
    /// <param name="proto"></param>
    public void OnReturnRoleLeaveSceneLine(GS2C_ReturnRoleLeaveSceneLine proto)
    {
        if (m_CurrPVPSceneRoleDic.TryGetValue(proto.RoleId, out var roleCtrl))
        {
            //卸载角色
            roleCtrl.OnClose();
            m_CurrPVPSceneRoleDic.Remove(proto.RoleId);
        }
    }

    /// <summary>
    /// 服务器返回角色进入场景线
    /// </summary>
    /// <param name="proto"></param>
    public void OnReturnRoleEnterSceneLine(GS2C_ReturnRoleEnterSceneLine proto)
    {
        int len = proto.RoleList.Count;
        for (int i = 0; i < len; i++)
        {
            WS2C_SceneLineRole_DATA data = proto.RoleList[i];
            LoadSceneLineRole(data);
        }
    }

    private void LoadSceneLineRole(WS2C_SceneLineRole_DATA data)
    {
        if (data.RoleType == RoleType.Player)
        {
            CreatePlayerByJobId(data.BaseRoleId, (RoleCtrl roleCtrl) =>
            {
                roleCtrl.ServerRoleId = data.RoleId;
                roleCtrl.transform.position = new UnityEngine.Vector3(data.CurrPos.X, data.CurrPos.Y, data.CurrPos.Z);
                roleCtrl.transform.rotation = Quaternion.Euler(0, data.RotationY, 0);
                roleCtrl.OpenAgent();

                //如果这个角色正在跑 继续让他跑
                if (data.Status == (int)MyCommonEnum.RoleFsmState.Run)
                {
                    roleCtrl.ClickMove(new UnityEngine.Vector3() { x = data.TargetPos.X, y = data.TargetPos.Y, z = data.TargetPos.Z });
                }
                else
                {
                    roleCtrl.ChangeState((MyCommonEnum.RoleFsmState)data.Status);
                }

                m_CurrPVPSceneRoleDic[roleCtrl.ServerRoleId] = roleCtrl;
            });
        }
        else
        {
            CreateSprite(data.BaseRoleId, (RoleCtrl roleCtrl) =>
            {
                roleCtrl.ServerRoleId = data.RoleId;
                roleCtrl.transform.position = new UnityEngine.Vector3(data.CurrPos.X, data.CurrPos.Y, data.CurrPos.Z);
                roleCtrl.transform.rotation = Quaternion.Euler(0, data.RotationY, 0);
                roleCtrl.OpenAgent();

                //如果这个角色正在跑 继续让他跑
                if (data.Status == (int)MyCommonEnum.RoleFsmState.Run)
                {
                    try
                    {
                        roleCtrl.ClickMove(new UnityEngine.Vector3() { x = data.TargetPos.X, y = data.TargetPos.Y, z = data.TargetPos.Z });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else
                {
                    roleCtrl.ChangeState((MyCommonEnum.RoleFsmState)data.Status);
                }

                m_CurrPVPSceneRoleDic[roleCtrl.ServerRoleId] = roleCtrl;
            });
        }
    }

    /// <summary>
    /// 点击移动 发送给服务器消息
    /// </summary>
    /// <param name="targetPos"></param>
    public void ClickMove(Vector3 targetPos)
    {
        C2GS_ClickMove proto = new C2GS_ClickMove();
        proto.TargetPos = new YouYou.Proto.Vector3() {X = targetPos.x, Y = targetPos.y, Z = targetPos.z};
        GameEntry.Socket.SendMainMsg(proto);
    }

    /// <summary>
    /// 角色状态修改
    /// </summary>
    /// <param name="proto"></param>
    public void RoleChangeState(GS2C_ReturnRoleChangeState proto)
    {
        //找到角色
        if (m_CurrPVPSceneRoleDic.TryGetValue(proto.RoleId,out RoleCtrl roleCtrl))
        {
            if (this.CurrPlayer.ServerRoleId == proto.RoleId)
            {
                return;
            }
            
            if (proto.Status == (int)MyCommonEnum.RoleFsmState.Idle)
            {
                roleCtrl.ChangeState(MyCommonEnum.RoleFsmState.Idle);
            }
            else if (proto.Status == (int) MyCommonEnum.RoleFsmState.Run)
            {
                roleCtrl.ServerRun(proto.RunSpeed,new UnityEngine.Vector3()
                    {x = proto.TargetPos.X, y = proto.TargetPos.Y, z = proto.TargetPos.Z});
            }
        }
    }

    /// <summary>
    /// 玩家进入AOI区域
    /// </summary>
    /// <param name="areaId"></param>
    public void PlayerEnterAOIArea(int areaId)
    {
        C2GS_Enter_AOIArea proto = new C2GS_Enter_AOIArea();
        proto.AreaId = areaId;
        GameEntry.Socket.SendMainMsg(proto);
    }
}