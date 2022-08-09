using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class RoleFsmBase : FsmState<RoleFsmManager>
{
    /// <summary>
    /// 让角色贴着地面方向
    /// </summary>
    private Vector3 m_MoveGroundDir = new Vector3(0, -10, 0);
    
    /// <summary>
    /// 角色是否贴着地面
    /// </summary>
    protected virtual bool IsGround => false;

    public override void OnEnter()
    {
        
    }

    public override void OnUpdate()
    {
        // //让角色贴地面
        // if (IsGround && !CurrFsm.Owner.CurrRoleCtrl.Agent.gr)
        // {
        //     CurrFsm.Owner.CurrRoleCtrl.Agent.Move(m_MoveGroundDir);
        // }
    }

    public override void OnLeave()
    {
        
    }

    public override void OnDestroy()
    {
        
    }
}
