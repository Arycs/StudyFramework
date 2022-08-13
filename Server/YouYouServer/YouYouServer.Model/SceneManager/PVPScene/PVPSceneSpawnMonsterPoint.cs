using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.ServerManager;
using YouYouServer.Model.ServerManager.Client.MonsterClient;
using UnityEngine;
using YouYouServer.Common;
using YouYouServer.Model.IHandler;

namespace YouYouServer.Model.SceneManager.PVPScene
{
    public class PVPSceneSpawnMonsterPoint
    {
        public PVPSceneLine OwnerPVPSceneLine;

        /// <summary>
        /// 刷怪点ID 
        /// </summary>
        public int Id;

        /// <summary>
        /// 怪ID
        /// </summary>
        public int MonsterId;

        /// <summary>
        /// 出生点坐标
        /// </summary>
        public Vector3 BornPos;

        /// <summary>
        /// 巡逻点列表
        /// </summary>
        public List<Vector3> PatrolPosList;

        /// <summary>
        /// 是否固定时间刷怪(比如 20点 刷BOSS)
        /// </summary>
        public bool IsFixTime;

        public int FixTime_Hour;
        public int FixTime_Minute;

        public int interval;

        public MonsterClient CurrMonster;

        /// <summary>
        /// 下次产卵时间 (怪死亡时,设置下次刷怪时间)
        /// </summary>
        public float NextSpwanTime;

        /// <summary>
        /// 累计产卵数量 (每次产卵数量+1 以后统计用)
        /// </summary>
        public int TotalSpwanCount;

        public PVPSceneSpawnMonsterPoint(PVPSceneLine pvpSceneLine)
        {
            OwnerPVPSceneLine = pvpSceneLine;
            //巡逻点, 当前配置表里只有3组Vector3 后续如果做过多扩展则需要增大 List
            PatrolPosList = new List<Vector3>(5);

            HotFixHelper.OnLoadAssembly += InitHandler;
            InitHandler();
        }

        /// <summary>
        /// 当前的状态同步处理句柄
        /// </summary>
        private IPVPSceneSpawnMonsterPointHandler m_CurrHandler;

        private void InitHandler()
        {
            if (m_CurrHandler != null)
            {
                m_CurrHandler.Dispose();
                m_CurrHandler = null;
            }

            m_CurrHandler = Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.PVPSceneSpawnMonsterPointHandler]) as IPVPSceneSpawnMonsterPointHandler;
            m_CurrHandler.Init(this);
        }

        public void OnUpdate()
        {
            m_CurrHandler.OnUpdate();
        }
    }
}
