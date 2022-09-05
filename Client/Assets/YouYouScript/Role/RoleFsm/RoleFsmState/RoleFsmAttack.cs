using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleFsmAttack : RoleFsmBase
{
    /// <summary>
    /// 动画长度
    /// </summary>
    private float m_AnimLen = 0;

    /// <summary>
    /// 进入时间
    /// </summary>
    private float m_EnterTime = 0;

    /// <summary>
    /// 设置动画时间
    /// </summary>
    /// <param name="animLen"></param>
    public void SetAnimLen(float animLen)
    {
        m_AnimLen = animLen;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        RoleAnimInfo roleAnimInfo =
            CurrFsm.Owner.CurrRoleCtrl.PlayAnimByAnimCategory(MyCommonEnum.RoleAnimCategory.Attack);
        //这里由 技能编辑器来获取时间, 不必再通过这个方式获取动画长度
        //m_AnimLen = roleAnimInfo.CurrPlayable.GetAnimationClip().length;
        m_EnterTime = Time.time;
        m_AnimLen = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Time.time > m_EnterTime + m_AnimLen)
        {
            CurrFsm.Owner.ChangeState(MyCommonEnum.RoleFsmState.Idle);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
