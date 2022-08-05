using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

public class RoleFsmRun :  RoleFsmBase
{
    private Vector3 m_TargetPos;
    
    public override void OnEnter()
    {
        base.OnEnter();
        CurrFsm.Owner.CurrRoleCtrl.PlayAnimByAnimCategory(MyCommonEnum.RoleAnimCategory.Run);
        m_TargetPos = CurrFsm.GetData<Vector3>(MyConstDefine.TargetPos);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //摇杆拖拽中 禁止滑动摄像机
        if (GameEntry.Input.Joystick != null && GameEntry.Input.Joystick.IsDraging)
        {
            return;
        }
        
        //方向
        Vector3 direction = Vector3.zero;
        //计算方向
        var position = CurrFsm.Owner.CurrRoleCtrl.transform.position;
        direction = m_TargetPos - position;
        direction = direction.normalized; //归一化
        direction = direction * Time.deltaTime * CurrFsm.Owner.CurrRoleCtrl.MoveSpeed;
        direction.y = 0;
        
        //立刻转身
        CurrFsm.Owner.CurrRoleCtrl.transform.rotation = Quaternion.LookRotation(direction);
        CurrFsm.Owner.CurrRoleCtrl.CharacterController.Move(direction);
        
        //判断是否应该向下一个点移动
        float dis = Vector3.Distance(position, m_TargetPos);
        
        //当到达目标点了
        if (dis <= direction.magnitude + 0.1f)
        {
            CurrFsm.Owner.ChangeState(MyCommonEnum.RoleFsmState.Idle);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }

    /// <summary>
    /// 摇杆移动
    /// </summary>
    /// <param name="dir"></param>
    public void JoystickMove(Vector2 dir)
    {
        Vector3 direction = Vector3.zero;
        
        //1. 设置辅助器位置
        var position = CurrFsm.Owner.CurrRoleCtrl.transform.position;
        GameEntry.Data.RoleDataManager.CurrPlayerMoveHelper.transform.position = new Vector3(
            position.x + dir.x,
            position.y,
            position.z + dir.y
        );
        
        //2. 让辅助器进行旋转
        GameEntry.Data.RoleDataManager.CurrPlayerMoveHelper.transform.RotateAround(position,Vector3.up, GameEntry.CameraCtrl.transform.localEulerAngles.y - 90);
        
        //3. 得到真正要移动的方向
        direction = GameEntry.Data.RoleDataManager.CurrPlayerMoveHelper.transform.position - position;
        
        direction.Normalize();//归一化, 确保力度一致
        direction = direction * Time.deltaTime * CurrFsm.Owner.CurrRoleCtrl.MoveSpeed;
        
        CurrFsm.Owner.CurrRoleCtrl.transform.rotation = Quaternion.LookRotation(direction);
        CurrFsm.Owner.CurrRoleCtrl.CharacterController.Move(direction);
    }
}
