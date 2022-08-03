using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class RoleFsmIdle : RoleFsmBase
{
    public override void OnEnter()
    {
        base.OnEnter();
        //TODO 此处应该关联角色,不应该写死,后续增加对应表格进行修改
        curFsm.Owner.CurrRoleCtrl.PlayAnimByAnimId(100030);
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
