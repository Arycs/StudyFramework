using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class RoleFsmIdle : RoleFsmBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        CurrFsm.Owner.CurrRoleCtrl.PlayAnimByAnimCategory(MyCommonEnum.RoleAnimCategory.IdleNormal);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
