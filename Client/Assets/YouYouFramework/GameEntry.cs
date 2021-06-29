using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class GameEntry : MonoBehaviour
    {
        #region 组件属性

        /// <summary>
        /// 事件组件
        /// </summary>
        public static EventComponent Event { get; private set; }

        /// <summary>
        /// 时间组件
        /// </summary>
        public static TimeComponent Time { get; private set; }

        /// <summary>
        /// 状态机组件
        /// </summary>
        public static FsmComponent Fsm { get; private set; }

        /// <summary>
        /// 流程组件
        /// </summary>
        public static ProcedureComponent Procedure { get; private set; }

        /// <summary>
        /// 数据表组件
        /// </summary>
        public static DataTableComponent DataTable { get; private set; }

        /// <summary>
        /// Socket组件
        /// </summary>
        public static SocketComponent Socket { get; private set; }

        /// <summary>
        /// Http组件
        /// </summary>
        public static HttpComponent Http { get; private set; }

        /// <summary>
        /// 数据组件
        /// </summary>
        public static DataComponent Data { get; private set; }

        /// <summary>
        /// 本地化组件
        /// </summary>
        public static LocalizationComponent Localization { get; private set; }

        /// <summary>
        /// 对象池组件
        /// </summary>
        public static PoolComponent Pool { get; private set; }

        /// <summary>
        /// 场景组件
        /// </summary>
        public static SceneComponent Scene { get; private set; }

        /// <summary>
        /// 设置组件
        /// </summary>
        public static SettingComponent Setting { get; private set; }

        /// <summary>
        /// 对象组件
        /// </summary>
        public static GameObjComponent GameObj { get; private set; }

        /// <summary>YouYouSceneManager
        /// 资源加载组件
        /// </summary>
        public static ResourceComponent Resource { get; private set; }

        /// <summary>
        /// 下载组件
        /// </summary>
        public static DownloadComponent Download { get; private set; }

        /// <summary>
        /// UI组件
        /// </summary>
        public static UIComponent UI { get; private set; }

        /// <summary>
        /// Lua组件
        /// </summary>
        public static LuaComponent Lua { get; private set; }

        /// <summary>
        /// Audio组件
        /// </summary>
        public static AudioComponent Audio { get; private set; }

        #endregion

        #region 基础组件管理

        /// <summary>
        /// 基础组件的列表
        /// </summary>
        private static readonly LinkedList<YouYouBaseComponent> m_BaseComponent = new LinkedList<YouYouBaseComponent>();

        #region RegisterBaseComponent 注册基础组件

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="component"></param>
        internal static void RegisterBaseComponent(YouYouBaseComponent component)
        {
            //获取到组件类型
            Type type = component.GetType();

            LinkedListNode<YouYouBaseComponent> curr = m_BaseComponent.First;
            while (curr != null)
            {
                if (curr.Value.GetType() == type)
                {
                    return;
                }

                curr = curr.Next;
            }

            //把组件加入最后一个节点
            m_BaseComponent.AddLast(component);
        }

        #endregion

        #region GetBaseComponent 获取基础组件

        public static T GetBaseComponent<T>() where T : YouYouBaseComponent
        {
            return (T) GetBaseComponent(typeof(T));
        }

        internal static YouYouBaseComponent GetBaseComponent(Type type)
        {
            LinkedListNode<YouYouBaseComponent> curr = m_BaseComponent.First;
            while (curr != null)
            {
                if (curr.Value.GetType() == type)
                {
                    return curr.Value;
                }

                curr = curr.Next;
            }

            return null;
        }

        #endregion

        #endregion

        #region 更新组件管理

        /// <summary>
        /// 更新组件的列表
        /// </summary>
        private static readonly LinkedList<IUpdateComponent> m_UpdateComponent = new LinkedList<IUpdateComponent>();

        #region RegisterUpdateComponent 注册更新组件

        /// <summary>
        /// 注册更新组件
        /// </summary>
        /// <param name="component"></param>
        public static void RegisterUpdateComponent(IUpdateComponent component)
        {
            m_UpdateComponent.AddLast(component);
        }

        #endregion

        #region RemoveUpdateComponent 移除更新组件

        /// <summary>
        /// 移除更新组件
        /// </summary>
        /// <param name="component"></param>
        public static void RemoveUpdateComponent(IUpdateComponent component)
        {
            m_UpdateComponent.Remove(component);
        }

        #endregion

        #endregion

        private static void InitBaseComponents()
        {
            Event = GetBaseComponent<EventComponent>();
            Time = GetBaseComponent<TimeComponent>();
            Fsm = GetBaseComponent<FsmComponent>();
            Procedure = GetBaseComponent<ProcedureComponent>();
            DataTable = GetBaseComponent<DataTableComponent>();
            Socket = GetBaseComponent<SocketComponent>();
            Http = GetBaseComponent<HttpComponent>();
            Data = GetBaseComponent<DataComponent>();
            Localization = GetBaseComponent<LocalizationComponent>();
            Pool = GetBaseComponent<PoolComponent>();
            Scene = GetBaseComponent<SceneComponent>();
            Setting = GetBaseComponent<SettingComponent>();
            GameObj = GetBaseComponent<GameObjComponent>();
            Resource = GetBaseComponent<ResourceComponent>();
            Download = GetBaseComponent<DownloadComponent>();
            UI = GetBaseComponent<UIComponent>();
            Lua = GetBaseComponent<LuaComponent>();
            Audio = GetBaseComponent<AudioComponent>();
        }

        void Start()
        {
            InitBaseComponents();
        }

        void Update()
        {
            //循环更新组件
            for (LinkedListNode<IUpdateComponent> curr = m_UpdateComponent.First; curr != null; curr = curr.Next)
            {
                curr.Value.OnUpdate();
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        private void OnDestroy()
        {
            //关闭所有的基础组件
            for (LinkedListNode<YouYouBaseComponent> curr = m_BaseComponent.First; curr != null; curr = curr.Next)
            {
                curr.Value.Shutdown();
            }
        }

        /// <summary>
        /// 打印日志 
        /// </summary>
        /// <param name="catetory"></param>
        /// <param name="message"></param>
        public static void Log(LogCategory catetory, string message, params object[] args)
        {
            switch (catetory)
            {
                default:
                case LogCategory.Normal:
#if DEBUG_LOG_NORMAL
                    Debug.Log(args.Length == 0? message : string.Format(message,args));
#endif
                    break;
                case LogCategory.Procedure:
#if DEBUG_LOG_PROCEDURE
                     Debug.Log(string.Format("<color=#ffffff>{0}</color>",args.Length == 0? message : string.Format(message,args)));
#endif
                    break;
                case LogCategory.Resource:
#if DEBUG_LOG_PROCEDURE
                    Debug.Log(string.Format("<color=#ACE44A>{0}</color>",args.Length == 0? message : string.Format(message,args)));
#endif
                    break;
                case LogCategory.Proto:
#if DEBUG_LOG_PROTO
                    Debug.Log(args.Length == 0? message : string.Format(message,args));
#endif
                    break;
            }
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void LogError(string message, params object[] args)
        {
#if DEBUG_LOG_ERROR
            Debug.LogError(args.Length == 0 ? message :string.Format(message,args));
#endif
        }
    }
}