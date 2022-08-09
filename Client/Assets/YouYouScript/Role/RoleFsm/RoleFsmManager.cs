using UnityEngine;
using YouYou;
using static MyCommonEnum;

/// <summary>
/// 角色状态管理器
/// </summary>
public class RoleFsmManager : ManagerBase
{
    /// <summary>
    /// 角色状态机
    /// </summary>
    private Fsm<RoleFsmManager> m_CurrFsm;

    /// <summary>
    /// 当权角色状态机
    /// </summary>
    public Fsm<RoleFsmManager> CurrFsm => m_CurrFsm;

    /// <summary>
    /// 当前的角色状态
    /// </summary>
    public RoleFsmState CurrRoleFsmState
    {
        get
        {
            if (m_CurrFsm == null)
            {
                return RoleFsmState.Idle;
            }

            return (RoleFsmState) m_CurrFsm.CurrStateType;
        }
    }

    /// <summary>
    /// 当前的角色状态
    /// </summary>
    public FsmState<RoleFsmManager> CurrRoleFsm => m_CurrFsm.GetState(m_CurrFsm.CurrStateType);

    /// <summary>
    /// 跑步状态
    /// </summary>
    private RoleFsmRun m_RoleFsmRun;
    
    /// <summary>
    /// 当前角色控制器
    /// </summary>
    public RoleCtrl CurrRoleCtrl { get; private set; }
    
    public RoleFsmManager(RoleCtrl roleCtrl)
    {
        CurrRoleCtrl = roleCtrl;
    }

    public override void Init()
    {
        FsmState<RoleFsmManager>[] states = new FsmState<RoleFsmManager>[3];
        states[(sbyte)RoleFsmState.Idle] = new RoleFsmIdle();

        m_RoleFsmRun = new RoleFsmRun();
        states[(sbyte)RoleFsmState.Run] = m_RoleFsmRun;
        states[(sbyte)RoleFsmState.Attack] = new RoleFsmAttack();
        m_CurrFsm = GameEntry.Fsm.Create(this, states);
    }

    public void OnUpdate()
    {
        m_CurrFsm.OnUpdate();
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(RoleFsmState state)
    {
        m_CurrFsm.ChangeState((sbyte)state);
    }

    /// <summary>
    /// 设置参数值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="TData"></typeparam>
    public void SetData<TData>(string key, TData value)
    {
        CurrFsm.SetData<TData>(key,value);
    }

    /// <summary>
    /// 获取参数值
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="TData"></typeparam>
    /// <returns></returns>
    public TData GetData<TData>(string key)
    {
        return CurrFsm.GetData<TData>(key);
    }

    public void ClickMove(Vector3 targetPos)
    {
        ChangeState(RoleFsmState.Run);
        m_RoleFsmRun.ClickMove(targetPos);
    }

    public void JoystickMove(Vector2 dir)
    {
        ChangeState(RoleFsmState.Run);
        m_RoleFsmRun.JoystickMove(dir);
    }

    public void JoystickStop()
    {
        m_RoleFsmRun.JoystickStop();
    }
}
