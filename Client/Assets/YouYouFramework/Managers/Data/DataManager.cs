using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 数据组件
    /// </summary>
    public class DataManager : ManagerBase, IDisposable
    {
        /// <summary>
        /// 游戏缓存数据
        /// </summary>
        public CacheDataManager CacheDataManager { get; private set; }

        /// <summary>
        /// 系统数据
        /// </summary>
        public SysDataManager SysDataManager { get; private set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public UserDataManager UserDataManager { get; private set; }

        /// <summary>
        /// PVE地图数据
        /// </summary>
        public PVEMapDataManager PVEMapDataManaer { get; private set; }

        /// <summary>
        /// 角色数据管理器
        /// </summary>
        public RoleDataManager RoleDataManager { get;private set; }

        /// <summary>
        /// 下次运行时间
        /// </summary>
        private float m_NextRunTime = 0f;
        
        public DataManager()
        {
            CacheDataManager = new CacheDataManager();
            SysDataManager = new SysDataManager();
            UserDataManager = new UserDataManager();
            PVEMapDataManaer = new PVEMapDataManager();
            RoleDataManager = new RoleDataManager();
        }

        public void OnUpdate()
        {
            //TODO 30后续改为配置
            if (Time.time > m_NextRunTime + 30)
            {
                m_NextRunTime = Time.time;
                RoleDataManager.CheckUnLoadRoleAnimation();
            }
        }


        public void Dispose()
        {
            CacheDataManager.Dispose();
            SysDataManager.Dispose();
            UserDataManager.Dispose();
            PVEMapDataManaer.Dispose();
            RoleDataManager.Dispose();
        }

        public override void Init()
        {
        }
    }
}