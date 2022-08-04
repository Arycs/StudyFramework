using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 输入管理器
    /// </summary>
    public class InputManager : ManagerBase, IDisposable
    {
        
        /// <summary>
        /// 摇杆
        /// </summary>
        public Joystick Joystick;

        
        /// <summary>
        /// 输入控制器
        /// </summary>
        private InputCtrlBase m_InputCtrl;

        /// <summary>
        /// 点击回调
        /// </summary>
        public event BaseAction<TouchEventData> OnClick;

        /// <summary>
        /// 开始拖拽
        /// </summary>
        public event BaseAction<TouchEventData> OnBeginDrag;

        /// <summary>
        /// 结束拖拽
        /// </summary>
        public event BaseAction<TouchEventData> OnEndDrag;

        /// <summary>
        /// 拖拽中
        /// </summary>
        public event BaseAction<TouchDirection, TouchEventData> OnDrag;

        /// <summary>
        /// 拖拽中
        /// </summary>
        public event BaseAction<ZoomType> OnZoom;

        public override void Init()
        {
            //此处判断平台
#if UNITY_EDITOR
            m_InputCtrl = new StandalonInputCtrl(
                t => OnClick(t),
                t => OnBeginDrag(t),
                t => OnEndDrag(t),
                (t1, t2) => OnDrag(t1, t2),
                t => OnZoom(t)
            );
#else
            //移动端
#endif
        }

        internal void OnUpdate()
        {
            m_InputCtrl.OnUpdate();
        }

        public void Dispose()
        {
        }
    }
}