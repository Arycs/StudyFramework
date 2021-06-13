using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YouYou;

namespace YouYou
{
    public class LuaComponent : YouYouBaseComponent
    {
        private LuaManager m_LuaManager;

        /// <summary>
        /// 是否打印协议日志
        /// </summary>
        public bool DebugLogProto = false;

        protected override void OnAwake()
        {
            base.OnAwake();
            m_LuaManager = new LuaManager();
#if DEBUG_LOG_PROTO
            DebugLogProto = true;
#endif
        }

        protected override void OnStart()
        {
            base.OnStart();
            LoadDataTableMS = new MMO_MemoryStream();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            m_LuaManager.Init();
        }

        /// <summary>
        /// 加载数据表的MS
        /// </summary>
        public MMO_MemoryStream LoadDataTableMS { get; private set; }

        /// <summary>
        /// 加载数据表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public void LoadDataTable(string tableName,BaseAction<MMO_MemoryStream> onComplete)
        {
           GameEntry.DataTable.DataTableManager.GetDataTableBuffer(tableName, (byte[] buffer) =>
           {
               LoadDataTableMS.SetLength(0);
               LoadDataTableMS.Write(buffer, 0, buffer.Length);
               LoadDataTableMS.Position = 0;

               if (onComplete != null)
               {
                   onComplete(LoadDataTableMS);
               }
           });
        }

        /// <summary>
        /// 在Lua中加载MemoryStream
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public MMO_MemoryStream LoadSocketReceiveMS(byte[] buffer)
        {
            MMO_MemoryStream ms = GameEntry.Socket.SocketReceiveMS;
            ms.SetLength(0);
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 取池MemoryStream
        /// </summary>
        /// <returns></returns>
        public MMO_MemoryStream DequeueMemoryStream()
        {
            return GameEntry.Pool.DequeueClassObject<MMO_MemoryStream>();
        }

        /// <summary>
        /// 取池MemoryStream并载入Buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public MMO_MemoryStream DequeueMemorystreamAndLoadBuffer(byte[] buffer)
        {
            MMO_MemoryStream ms = GameEntry.Pool.DequeueClassObject<MMO_MemoryStream>();
            ms.SetLength(0);
            ms.Write(buffer,0,buffer.Length);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 回池MemoryStream
        /// </summary>
        /// <param name="ms"></param>
        public void EnqueueMemoryStream(MMO_MemoryStream ms)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }

        /// <summary>
        /// 从ms获取指定的字节数组
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] GetByteArray(MMO_MemoryStream ms, int len)
        {
            byte[] buffer = new byte[len];
            ms.Read(buffer, 0, len);
            return buffer;
        }

        public override void Shutdown()
        {
            LoadDataTableMS.Dispose();
            LoadDataTableMS.Close();
        }
    }
}