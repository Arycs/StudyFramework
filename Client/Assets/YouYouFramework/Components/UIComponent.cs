using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YouYou
{
    /// <summary>
    /// UI组件
    /// </summary>
    public class UIComponent : YouYouBaseComponent,IUpdateComponent
    {
        [Header("标准分辨率的宽度")]
        [SerializeField]
        private int m_StandardWidth = 1280;
        [Header("标准分辨率的高度")]
        [SerializeField]
        private int m_StandardHeight = 720;
        
        [Header("UI摄像机")]
        [SerializeField]
        public Camera UICamera;

        [Header("根画布")]
        [SerializeField]
        private Canvas m_UIootCanvas;

        [Header("根画布的缩放")]
        [SerializeField]
        private CanvasScaler UIRootCanvasScaler;
        
        [Header("UI分组")]
        [SerializeField]
        private UIGroup[] UIGroups;

        
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
        
        protected override void OnAwake()
        {
            base.OnAwake();
            m_UIGroupDic = new Dictionary<byte, UIGroup>();
            GameEntry.RegisterUpdateComponent(this);

            m_StandardScreen =  m_StandardWidth / (float) m_StandardHeight;
            m_CurrScreen = Screen.width / (float) Screen.height;
            NormalFormCanvasScaler();

            int len = UIGroups.Length;
            for (int i = 0; i < len; i++)
            {
                UIGroup group = UIGroups[i];
                m_UIGroupDic[group.Id] = group;
            }
            m_UIManager = new UIManager();
            m_UILayer = new UILayer();
            m_UILayer.Init(UIGroups);
            
            m_UIPool = new UIPool();
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
                UIRootCanvasScaler.matchWidthOrHeight = 0;
            }
            else
            {
                //分辨率小于标准分辨率, 则用标准减去当前分辨率
                UIRootCanvasScaler.matchWidthOrHeight = m_CurrScreen - m_StandardScreen;
            }
        }

        /// <summary>
        /// FullForm适配缩放为1
        /// </summary>
        public void FullFormCanvasScaler()
        {
            UIRootCanvasScaler.matchWidthOrHeight = 1;
        }

        /// <summary>
        /// NormalForm适配缩放
        /// </summary>
        public void NormalFormCanvasScaler()
        {
            UIRootCanvasScaler.matchWidthOrHeight = m_CurrScreen >= m_StandardScreen ? 1 : 0;
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
            m_UIPool.CheckByOpenUI();
            m_UIManager.OpenUIForm(uiFormId,userData,onOpen);
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

        public override void Shutdown()
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
