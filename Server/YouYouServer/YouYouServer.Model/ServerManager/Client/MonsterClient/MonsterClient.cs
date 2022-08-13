using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YouYouServer.Common;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.SceneManager.PVPScene;

namespace YouYouServer.Model.ServerManager.Client.MonsterClient
{
    public class MonsterClient :RoleClientBase, IDisposable
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
        /// 当前坐标
        /// </summary>
        public Vector3 CurrPos;

        /// <summary>
        /// 目标坐标
        /// </summary>
        public Vector3 TargetPos;

        public MonsterClient()
        {
            CurrFsmManager = new RoleFsm.RoleFsm(this);

            Console.WriteLine("Monster Client ");
            HotFixHelper.OnLoadAssembly += InitHandler;
            InitHandler();
        }

        private void InitHandler()
        {
            if (CurrRoleClientHandler != null)
            {
                //把旧的实例释放
                CurrRoleClientHandler.Dispose();
                CurrRoleClientHandler = null;
            }
            CurrRoleClientHandler = Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.RoleClientHandler]) as IRoleClientHandler;
            CurrRoleClientHandler.Init(this);
        }

        public void Dispose()
        {
        }

    }
}
