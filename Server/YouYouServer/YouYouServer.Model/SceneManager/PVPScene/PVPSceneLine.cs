using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YouYouServer.Common;
using YouYouServer.Model.IHandler;

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

        public PVPSceneLine(int pvpSceneLineId, PVPScene ownerPVPScene)
        {
            PVPSceneLineId = pvpSceneLineId;
            OwnerPVPScene = ownerPVPScene;
            AOIAreaDic = new Dictionary<int, PVPSceneAOIArea>();
            RoleList = new LinkedList<RoleClientBase>();

            InitSceneAOIArea();

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
                PVPSceneAOIArea pvpSceneAOIArea = new PVPSceneAOIArea(item);
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

    }
}
