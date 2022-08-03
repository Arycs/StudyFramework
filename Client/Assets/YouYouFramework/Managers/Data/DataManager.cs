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
        public CacheDataManager CacheDataManager { get; private set; }

        /// <summary>
        /// ϵͳ����
        /// </summary>
        public SysDataManager SysDataManager { get; private set; }

        /// <summary>
        /// �û�����
        /// </summary>
        public UserDataManager UserDataManager { get; private set; }

        /// <summary>
        /// PVE��ͼ����
        /// </summary>
        public PVEMapDataManager PVEMapDataManaer { get; private set; }

        /// <summary>
        /// ��ɫ���ݹ�����
        /// </summary>
        public RoleDataManager RoleDataManager { get;private set; }

        /// <summary>
        /// �´�����ʱ��
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
            //TODO 30������Ϊ����
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