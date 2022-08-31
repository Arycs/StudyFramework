﻿using System;
using System.Collections.Generic;
using System.Text;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.SceneManager.PVPScene;
using Vector3 = UnityEngine.Vector3;

namespace YouYouServer.Model.ServerManager.Client.MonsterClient
{
    public class MonsterClient : RoleClientBase, IDisposable
    {
        /// <summary>
        /// 死亡委托
        /// </summary>
        public Action OnDie;

        public PVPSceneSpawnMonsterPoint CurrSpawnMonsterPoint;

        /// <summary>
        /// 当前场景编号
        /// </summary>
        public int CurrSceneId => CurrSpawnMonsterPoint.OwnerPVPSceneLine.OwnerPVPScene.CurrSceneConfig.SceneId;

        /// <summary>
        /// 进入待机时间
        /// </summary>
        public float EnterIdleTime = 0;

        /// <summary>
        /// 上一个巡逻点
        /// </summary>
        public int PrevPatrolPosIndex = 0;

        /// <summary>
        /// 是否巡逻中
        /// </summary>
        public bool IsPatrol = false;
        
        public MonsterClient()
        {
            PathPoints = new List<Vector3>();
            CurrFsmManager = new RoleFsm.RoleFsm(this);

            Console.WriteLine("Monster Client ");
            HotFixHelper.OnLoadAssembly += InitHandler;
            InitHandler();
        }

        private void InitHandler()
        {
            if (CurrRoleClientFsmHandler != null)
            {
                //把旧的实例释放
                CurrRoleClientFsmHandler.Dispose();
                CurrRoleClientFsmHandler = null;
            }
            CurrRoleClientFsmHandler = Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.MonsterClientFsmHandler]) as IRoleClientFsmHandler;
            CurrRoleClientFsmHandler?.Init(this);
        }

        public void Dispose()
        {
        }

        public override RoleType CurrRoleType => RoleType.Monster;
    }
}
