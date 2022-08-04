using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 输入控制器基类
    /// </summary>
    public abstract class InputCtrlBase
    {
        /// <summary>
        /// 是否开始拖拽
        /// </summary>
        protected bool m_IsBeginDrag;

        /// <summary>
        /// 是否结束拖拽
        /// </summary>
        protected bool m_IsEndDrag;

        /// <summary>
        /// 是否拖拽中
        /// </summary>
        protected bool m_IsDraging;

        /// <summary>
        /// 触屏事件数据
        /// </summary>
        private TouchEventData m_TouchEventData = null;

        /// <summary>
        /// 触屏事件数据
        /// </summary>
        protected TouchEventData TouchEventData => m_TouchEventData ?? (m_TouchEventData = new TouchEventData());

        /// <summary>
        /// 滑动方向
        /// </summary>
        protected TouchDirection Direction { get; private set; }

        /// <summary>
        /// 滑动距离
        /// </summary>
        protected Vector2 Distance { get; private set; }

        /// <summary>
        /// 点击回调
        /// </summary>
        protected BaseAction<TouchEventData> OnClick;
        
        /// <summary>
        /// 开始拖拽事件
        /// </summary>
        protected BaseAction<TouchEventData> OnBeginDrag;

        /// <summary>
        /// 结束拖拽事件
        /// </summary>
        protected BaseAction<TouchEventData> OnEndDrag;

        /// <summary>
        /// 拖拽中
        /// </summary>
        protected BaseAction<TouchDirection, TouchEventData> OnDrag;

        /// <summary>
        /// 缩放事件
        /// </summary>
        protected BaseAction<ZoomType> OnZoom;
        
        public InputCtrlBase(
            BaseAction<TouchEventData> onClick, 
            BaseAction<TouchEventData> onBeginDrag,
            BaseAction<TouchEventData> onEndDrag,
            BaseAction<TouchDirection, TouchEventData> onDrag,
            BaseAction<ZoomType> onZoom)
        {
            OnClick = onClick;
            OnBeginDrag = onBeginDrag;
            OnEndDrag = onEndDrag;
            OnDrag = onDrag;
            OnZoom = onZoom;
        }

        internal abstract void OnUpdate();

        /// <summary>
        /// 点击
        /// </summary>
        /// <returns></returns>
        protected abstract bool Click();

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <returns></returns>
        protected abstract bool BeginDrag();

        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <returns></returns>
        protected abstract bool EndDrag();

        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <returns></returns>
        protected abstract bool Drag();
        
        /// <summary>
        /// 滑动
        /// </summary>
        /// <returns></returns>
        protected abstract bool Move(TouchDirection touchDirection, TouchEventData touchEventData);

        /// <summary>
        /// 缩放
        /// </summary>
        /// <returns></returns>
        protected abstract bool Zoom();
    }
}