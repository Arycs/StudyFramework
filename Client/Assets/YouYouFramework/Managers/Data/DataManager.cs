using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    /// <summary>
    /// �������
    /// </summary>
    public class DataManager : ManagerBase, IDisposable
    {
        /// <summary>
        /// ��Ϸ��������
        /// </summary>
        public CacheDataManager CacheDataManager
        {
            get; private set;
        }

        /// <summary>
        /// ϵͳ����
        /// </summary>
        public SysDataManager SysDataManager
        {
            get; private set;
        }

        /// <summary>
        /// �û�����
        /// </summary>
        public UserDataManager UserDataManager
        {
            get; private set;
        }

        /// <summary>
        /// PVE��ͼ����
        /// </summary>
        public PVEMapDataManager PVEMapDataManaer
        {
            get; private set;
        }

        public DataManager()
        {
            CacheDataManager = new CacheDataManager();
            SysDataManager = new SysDataManager();
            UserDataManager = new UserDataManager();
            PVEMapDataManaer = new PVEMapDataManager();
        }

        public void Dispose()
        {
            CacheDataManager.Dispose();
            SysDataManager.Dispose();
            UserDataManager.Dispose();
            PVEMapDataManaer.Dispose();
        }

        public override void Init()
        {
            
        }
    }
}
