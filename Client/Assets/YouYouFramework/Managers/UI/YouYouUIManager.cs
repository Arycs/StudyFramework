using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YouYou
{
    /// <summary>
    /// UI组件
    /// </summary>
    public class YouYouUIManager : ManagerBase, IDisposable
    {
        
        
        private Dictionary<byte, UIGroup> m_UIGroupDic;
        
        /// <summary>
        /// 标准分辨率比值
        /// </summary>
        private float m_StandardScreen = 0;
        /// <summary>
        /// 当前分辨率比值
        /// </summary>
        private float m_CurrScreen = 0;

        private UIManager m_UIManager;

        private UILayer m_UILayer;

        private UIPool m_UIPool;

        [Header("释放间隔(秒)")]
        [SerializeField]
        private float m_ClearInterval = 120f;

        /// <summary>
        /// UI回池后过期时间
        /// </summary>
        public float UIExpire = 60f;

        /// <summary>
        /// UI对象池中 最大的数量
        /// </summary>
        public int UIPoolMaxCount = 5;

        /// <summary>
        /// 下次运行时间
        /// </summary>
        private float m_NextRunTime = 0f;
        
        public YouYouUIManager()
        {
            m_UIManager = new UIManager();
            m_UILayer = new UILayer();
            m_UILayer.Init(GameEntry.Instance.UIGroups);
            
            m_UIPool = new UIPool();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            m_UIGroupDic = new Dictionary<byte, UIGroup>();

            m_StandardScreen = GameEntry.Instance.m_StandardWidth / (float)GameEntry.Instance.m_StandardHeight;
            m_CurrScreen = Screen.width / (float)Screen.height;
            NormalFormCanvasScaler();

            int len = GameEntry.Instance.UIGroups.Length;
            for (int i = 0; i < len; i++)
            {
                UIGroup group = GameEntry.Instance.UIGroups[i];
                m_UIGroupDic[group.Id] = group;
            }

            m_UILayer.Init(GameEntry.Instance.UIGroups);
        }
        #region UI适配
        /// <summary>
        /// LoadingForm适配缩放
        /// </summary>
        public void UILoadingFormCanvasScaler()
        {

            if (m_CurrScreen > m_StandardScreen)
            {
                //分辨率大于标准分辨率 则设置为0
                GameEntry.Instance.UIRootCanvasScaler.matchWidthOrHeight = 0;
            }
            else
            {
                //分辨率小于标准分辨率, 则用标准减去当前分辨率
                GameEntry.Instance.UIRootCanvasScaler.matchWidthOrHeight = m_CurrScreen - m_StandardScreen;
            }
        }

        /// <summary>
        /// FullForm适配缩放为1
        /// </summary>
        public void FullFormCanvasScaler()
        {
            GameEntry.Instance.UIRootCanvasScaler.matchWidthOrHeight = 1;
        }

        /// <summary>
        /// NormalForm适配缩放
        /// </summary>
        public void NormalFormCanvasScaler()
        {
            GameEntry.Instance.UIRootCanvasScaler.matchWidthOrHeight = m_CurrScreen >= m_StandardScreen ? 1 : 0;
        }
        #endregion

        #region GetUIGroup
        /// <summary>
        /// 根据UI分组编号,获取UI分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UIGroup GetUIGroup(byte id)
        {
            UIGroup group = null;
            m_UIGroupDic.TryGetValue(id, out group);
            return group;
        }
        #endregion

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormId">formId</param>
        /// <param name="userData">用户数据</param>
        public void OpenUIForm(int uiFormId, object userData = null,BaseAction<UIFormBase> onOpen = null)
        {
            m_UIManager.OpenUIForm(uiFormId,userData,onOpen);
            m_UIPool.CheckByOpenUI();
        }

        /// <summary>
        /// 根据uiformId 关闭UI窗口
        /// </summary>
        /// <param name="uiformId"></param>
        internal void CloseUIForm(int uiformId)
        {
            m_UIManager.CloseUIForm(uiformId);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="formBase"></param>
        public void CloseUIForm(UIFormBase formBase)
        {
            m_UIManager.CloseUIForm(formBase);
        }

        /// <summary>
        /// 设置层级
        /// </summary>
        /// <param name="formBase"></param>
        /// <param name="isAdd"></param>
        internal void SetSortingOrder(UIFormBase formBase, bool isAdd)
        {
            m_UILayer.SetSortingOrder(formBase,isAdd);
        }

        /// <summary>
        /// 从UI对象池中获取UI
        /// </summary>
        /// <param name="uiformId"></param>
        /// <returns></returns>
        internal UIFormBase Dequeue(int uiformId)
        {
            return m_UIPool.Dequeue(uiformId);
        }

        /// <summary>
        /// 回到UI对象池
        /// </summary>
        /// <param name="form"></param>
        internal void Enqueue(UIFormBase form)
        {
            m_UIPool.Enqueue(form);
        }

        /// <summary>
        /// 打开提示对话框
        /// </summary>
        /// <param name="sysCode"></param>
        /// <param name=""></param>
        /// <param name="onConfirm"></param>
        /// <param name="onCancel"></param>
        public void OpenDialogFormBySysCode(int sysCode, DialogFormType dialogFormType = DialogFormType.Normal, BaseAction onConfirm = null, BaseAction onCancel = null)
        {
            OpenDialForm(dialogFormType, GameEntry.Data.SysDataManager.GetSysCodeContent(sysCode), null, onConfirm, onCancel);
        }

        /// <summary>
        /// 打开提示框
        /// </summary>
        /// <param name="dialogFormType">提示框类型</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="onConfirm">确认回调</param>
        /// <param name="onCancel">取消回调</param>
        public void OpenDialForm(DialogFormType dialogFormType, string content, string title = null, BaseAction onConfirm = null, BaseAction onCancel = null)
        {
            BaseParams baseParams = GameEntry.Pool.DequeueClassObject<BaseParams>();
            baseParams.Reset();

            baseParams.IntParam1 = (int)dialogFormType;
            baseParams.StringParam1 = content;
            baseParams.StringParam2 = title;
            baseParams.ActionParam1 = onConfirm;
            baseParams.ActionParam2 = onCancel;

            OpenUIForm(2, baseParams);
        }

        public void Dispose()
        {
            //Debug.Log("UI组件关闭");
        }

        public void OnUpdate()
        {

            if (Time.time > m_NextRunTime + m_ClearInterval)
            {
                m_NextRunTime = Time.time;
                //释放 UI对象池
                m_UIPool.CheckClear();
            }

        }
    }
}
