using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YouYouServer.Common;
using YouYouServer.Model.DataTable;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.SceneManager.PVPScene;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 场景线
    /// </summary>
    public class PVPSceneLine
    {
        /// <summary>
        /// 场景线编号
        /// </summary>
        public int PVPSceneLineId { get; }

        /// <summary>
        /// 所属场景
        /// </summary>
        public PVPScene OwnerPVPScene { get; }

        /// <summary>
        /// AOI区域字典
        /// </summary>
        public Dictionary<int, PVPSceneAOIArea> AOIAreaDic;

        //这个场景线里面 要启动一个线程 用于pvp状态同步
        private Thread m_SyncThread;

        /// <summary>
        /// 角色列表
        /// </summary>
        public LinkedList<RoleClientBase> RoleList { get; }

        /// <summary>
        /// 刷怪点字典
        /// </summary>
        public Dictionary<int, PVPSceneSpawnMonsterPoint> SpawnMonsterPointDic;

        /// <summary>
        /// 到上一帧的时间
        /// </summary>
        public float Deltatime = 0;

        public PVPSceneLine(int pvpSceneLineId, PVPScene ownerPVPScene)
        {
            PVPSceneLineId = pvpSceneLineId;
            OwnerPVPScene = ownerPVPScene;
            AOIAreaDic = new Dictionary<int, PVPSceneAOIArea>();
            RoleList = new LinkedList<RoleClientBase>();
            SpawnMonsterPointDic = new Dictionary<int, PVPSceneSpawnMonsterPoint>();

            InitSceneAOIArea();
            InitSpawnMonsterPoint();

            HotFixHelper.OnLoadAssembly += InitPVPSceneLineSyncHandler;
            InitPVPSceneLineSyncHandler();

            m_SyncThread = new Thread(SyncPVPStatus);
            m_SyncThread.Start();
        }

        /// <summary>
        /// 初始化场景AOI区域
        /// </summary>
        private void InitSceneAOIArea()
        {
            foreach (var item in OwnerPVPScene.CurrSceneAreaDataList)
            {
                PVPSceneAOIArea pvpSceneAOIArea = new PVPSceneAOIArea(item,this);
                AOIAreaDic[pvpSceneAOIArea.CurrAOIData.AreaId] = pvpSceneAOIArea;
            }
            Console.WriteLine("初始化场景 {0} AOI区域", OwnerPVPScene.CurrSysScene.SceneName);
        }

        /// <summary>
        /// 当前的状态同步处理句柄
        /// </summary>
        private IPVPSceneLineSyncHandler m_CurrSyncHandler;

        private void InitPVPSceneLineSyncHandler()
        {
            if (m_CurrSyncHandler != null)
            {
                //把旧的实例释放
                m_CurrSyncHandler.Dispose();
                m_CurrSyncHandler = null;
            }

            m_CurrSyncHandler = Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.PVPSceneLineSyncHandler]) as IPVPSceneLineSyncHandler;
            m_CurrSyncHandler.Init(this);

            Console.WriteLine("InitPlayerForWorldClientHandler");
        }

        /// <summary>
        /// 初始化刷怪点
        /// </summary>
        public void InitSpawnMonsterPoint()
        {
            List<DTPVPSceneMonsterPointEntity> lst = DataTableManager.PVPSceneMonsterPointList.GetListBySceneId(OwnerPVPScene.CurrSysScene.Id);
            foreach (var item in lst)
            {
                PVPSceneSpawnMonsterPoint pvpSceneSpawnMonsterPoint = new PVPSceneSpawnMonsterPoint(this);
                pvpSceneSpawnMonsterPoint.Id = item.Id;
                pvpSceneSpawnMonsterPoint.MonsterId = item.MonsterId;
                pvpSceneSpawnMonsterPoint.IsFixTime = item.IsFixTime;
                pvpSceneSpawnMonsterPoint.FixTime_Hour = item.FixTime_Hour;
                pvpSceneSpawnMonsterPoint.FixTime_Minute = item.FixTime_Minute;
                pvpSceneSpawnMonsterPoint.interval = item.Interval;
                pvpSceneSpawnMonsterPoint.BornPos = new UnityEngine.Vector3(item.BornPos_1, item.BornPos_2, item.BornPos_3);

                //出生点也是一个巡逻点
                pvpSceneSpawnMonsterPoint.PatrolPosList.Add(pvpSceneSpawnMonsterPoint.BornPos);
                
                // 三个巡逻点 
                pvpSceneSpawnMonsterPoint.PatrolPosList.Add(new UnityEngine.Vector3(item.PatrolX_1, item.PatrolY_1, item.PatrolZ_1));
                pvpSceneSpawnMonsterPoint.PatrolPosList.Add(new UnityEngine.Vector3(item.PatrolX_2, item.PatrolY_2, item.PatrolZ_3));
                pvpSceneSpawnMonsterPoint.PatrolPosList.Add(new UnityEngine.Vector3(item.PatrolX_3, item.PatrolY_3, item.PatrolZ_3));
                pvpSceneSpawnMonsterPoint.PatrolPosList.Add(new UnityEngine.Vector3(item.PatrolX_4, item.PatrolY_4, item.PatrolZ_4));

                SpawnMonsterPointDic[pvpSceneSpawnMonsterPoint.Id] = pvpSceneSpawnMonsterPoint;
            }
        }


        /// <summary>
        /// 同步PVP状态
        /// </summary>
        /// <param name="obj"></param>
        private void SyncPVPStatus()
        {
            while (true)
            {
                m_CurrSyncHandler.SyncStatus();
            }
        }

        
        /// <summary>
        /// 告诉其他玩家 我来了
        /// </summary>
        /// <param name="roleClient"></param>
        /// <param name="otherPlayer"></param>
        public void RoleEnterSceneLine(RoleClientBase roleClient, PlayerForGameClient otherPlayer)
        {
            m_CurrSyncHandler.RoleEnterSceneLine(roleClient,otherPlayer);
        }
        
        /// <summary>
        /// 检查角色是否跨区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        public void CheckAreaChange(RoleClientBase roleClientBase)
        {
            m_CurrSyncHandler.CheckAreaChange(roleClientBase);
        }

        /// <summary>
        /// 角色进入新区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="areaId"></param>
        public void RoleEnterNewArea(RoleClientBase roleClientBase, int areaId)
        {
            m_CurrSyncHandler.RoleEnterNewArea(roleClientBase,areaId);
        }
    }
}
