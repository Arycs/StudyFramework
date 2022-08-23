using System;
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
        /// 路径点
        /// </summary>
        public List<Vector3> PathPoints;

        /// <summary>
        /// 当前路径点索引
        /// </summary>
        public int CurrWayPointIndex = 0;

        /// <summary>
        /// 移动起始点
        /// </summary>
        public Vector3 RunBeginPos;

        /// <summary>
        /// 移动结束点
        /// </summary>
        public Vector3 RunEndPos;

        /// <summary>
        /// 移动方向
        /// </summary>
        public Vector3 RunDir;

        /// <summary>
        /// 移动速度
        /// </summary>
        public float RunSpeed = 10f;

        /// <summary>
        /// 移动时间
        /// </summary>
        public float RunTime = 0;

        /// <summary>
        /// 转身完毕标志
        /// </summary>
        public bool TurnComplete = false;

        public MonsterClient()
        {
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
