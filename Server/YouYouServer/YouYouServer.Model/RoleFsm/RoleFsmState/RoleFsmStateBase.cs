using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.RoleFsm.RoleFsmState
{
    /// <summary>
    /// 角色状态基类
    /// </summary>
    public abstract class RoleFsmStateBase
    {
        protected RoleFsm CurrFsm;

        public RoleFsmStateBase(RoleFsm roleFsm)
        {
            CurrFsm = roleFsm;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnLeave() { }

    }
}
