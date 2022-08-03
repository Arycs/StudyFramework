public class MyCommonEnum 
{
    /// <summary>
    /// 角色状态机状态
    /// </summary>
    public enum RoleFsmState
    {
        /// <summary>
        /// 待机
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 跑
        /// </summary>
        Run = 1,
        /// <summary>
        /// 攻击状态
        /// </summary>
        Attack,
    }

    /// <summary>
    /// 角色动画分类
    /// </summary>
    public enum RoleAnimCategory
    {
        /// <summary>
        /// 普通待机
        /// </summary>
        IdleNormal,
        
        /// <summary>
        /// 跑
        /// </summary>
        Run,
        
        /// <summary>
        /// 攻击
        /// </summary>
        Attack,
    }
}
