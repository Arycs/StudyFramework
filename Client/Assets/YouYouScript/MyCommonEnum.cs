public class MyCommonEnum 
{
    /// <summary>
    /// 角色状态机状态
    /// </summary>
    public enum RoleFsmState
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,
        /// <summary>
        /// 待机
        /// </summary>
        Idle = 1,
        /// <summary>
        /// 跑
        /// </summary>
        Run = 2,
        /// <summary>
        /// 攻击状态
        /// </summary>
        Attack = 3,
        /// <summary>
        /// 受伤
        /// </summary>
        Hurt = 4,
        /// <summary>
        /// 死亡
        /// </summary>
        Die = 5
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
    
    public enum Sex
    {
        /// <summary>
        /// 女
        /// </summary>
        Woman = 0,

        /// <summary>
        /// 男
        /// </summary>
        Male = 1
    }
}
