using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Model.IHandler;

namespace YouYouServer.Model.ServerManager.Client.MonsterClient
{
    public class MonsterClient :RoleClientBase,IDisposable
    {
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
