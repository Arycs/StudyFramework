using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 世界地图流程
    /// </summary>
    public partial class ProcedureWorldMap : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GameEntry.Log(LogCategory.Procedure,"OnEnter ProcedureWorldMap");
            //加载场景
            GameEntry.Scene.LoadScene(SysScene.Test,false,(() =>
            {
                GameEntry.Event.CommonEvent.Dispatch(SysEventId.CloseCheckVersionUI);

                LoadWorldMapComplete();
            }));
        }

        /// <summary>
        /// 加载PVP场景完毕
        /// </summary>
        private void LoadWorldMapComplete()
        {
            GameEntry.Data.RoleDataManager.CreatePlayerByJobId(JobId.CiKe);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
