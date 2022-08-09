using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using YouYou;

public class RoleFsmRun : RoleFsmBase
{
    /// <summary>
    /// 路径点
    /// </summary>
    private Vector3[] m_VectorPath;

    /// <summary>
    /// 当前路径点索引
    /// </summary>
    private int m_CurrPointIndex;

    /// <summary>
    /// 是否通过摇杆播放跑动画
    /// </summary>
    private bool m_IsPlayRunWithJoystick;

    /// <summary>
    /// 是否通过点击地面播放奔跑动画
    /// </summary>
    private bool m_IsPlayRunWithClick;


    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //摇杆拖拽中 禁止滑动摄像机
        if (GameEntry.Input.Joystick != null && GameEntry.Input.Joystick.IsDraging)
        {
            return;
        }

        #region 编辑器模式, 绘路线图

#if UNITY_EDITOR
        Color c = Color.white;
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                c = Color.blue;
                break;
            case NavMeshPathStatus.PathInvalid:
                c = Color.red;
                break;
            case NavMeshPathStatus.PathPartial:
                c = Color.yellow;
                break;
        }

        for (int i = 1; i < path.corners.Length; ++i)
        {
            Debug.DrawLine(path.corners[i - 1], path.corners[i], c);
        }
#endif

        #endregion

        if (m_VectorPath == null)
        {
            return;
        }

        //如果整个路径走完了 切换待机
        if (m_CurrPointIndex >= m_VectorPath.Length)
        {
            m_VectorPath = null;
            m_IsPlayRunWithClick = false;
            CurrFsm.Owner.ChangeState(MyCommonEnum.RoleFsmState.Idle);
            return;
        }

        //方向
        Vector3 direction = Vector3.zero;
        //计算方向
        var position = CurrFsm.Owner.CurrRoleCtrl.transform.position;
        var temp = new Vector3(m_VectorPath[m_CurrPointIndex].x,
            position.y,
            m_VectorPath[m_CurrPointIndex].z);

        direction = temp - position;
        direction = direction.normalized; //归一化
        direction = direction * Time.deltaTime * CurrFsm.Owner.CurrRoleCtrl.MoveSpeed;
        direction.y = 0;

        //立刻转身
        CurrFsm.Owner.CurrRoleCtrl.transform.rotation = Quaternion.LookRotation(direction);
        CurrFsm.Owner.CurrRoleCtrl.Agent.Move(direction);

        //判断是否应该向下一个点移动
        float dis = Vector3.Distance(position, temp);

        //当到达目标点了
        if (dis <= direction.magnitude + 0.1f)
        {
            m_CurrPointIndex++;
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }

    private NavMeshPath path = new NavMeshPath();

    public void ClickMove(Vector3 targetPos)
    {
        m_CurrPointIndex = 1;

        //计算路径
        CurrFsm.Owner.CurrRoleCtrl.Agent.CalculatePath(targetPos, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            m_VectorPath = path.corners;

            if (!m_IsPlayRunWithClick)
            {
                m_IsPlayRunWithClick = true;
                CurrFsm.Owner.CurrRoleCtrl.PlayAnimByAnimCategory(MyCommonEnum.RoleAnimCategory.Run);
            }
        }
    }


    /// <summary>
    /// 摇杆移动
    /// </summary>
    /// <param name="dir"></param>
    public void JoystickMove(Vector2 dir)
    {
        if (!m_IsPlayRunWithJoystick)
        {
            //确保动画只播放一次
            CurrFsm.Owner.CurrRoleCtrl.PlayAnimByAnimCategory(MyCommonEnum.RoleAnimCategory.Run);
            m_IsPlayRunWithJoystick = true;
        }


        Vector3 direction = Vector3.zero;

        //1. 设置辅助器位置
        var position = CurrFsm.Owner.CurrRoleCtrl.transform.position;
        GameEntry.Data.RoleDataManager.CurrPlayerMoveHelper.transform.position = new Vector3(
            position.x + dir.x,
            position.y,
            position.z + dir.y
        );

        //2. 让辅助器进行旋转
        GameEntry.Data.RoleDataManager.CurrPlayerMoveHelper.transform.RotateAround(position, Vector3.up,
            GameEntry.CameraCtrl.transform.localEulerAngles.y - 90);

        //3. 得到真正要移动的方向
        direction = GameEntry.Data.RoleDataManager.CurrPlayerMoveHelper.transform.position - position;

        direction.Normalize(); //归一化, 确保力度一致
        direction = direction * Time.deltaTime * CurrFsm.Owner.CurrRoleCtrl.MoveSpeed;

        CurrFsm.Owner.CurrRoleCtrl.transform.rotation = Quaternion.LookRotation(direction);
        CurrFsm.Owner.CurrRoleCtrl.Agent.Move(direction);
    }

    public void JoystickStop()
    {
        m_IsPlayRunWithJoystick = false;
        CurrFsm.Owner.ChangeState(MyCommonEnum.RoleFsmState.Idle);
    }
}