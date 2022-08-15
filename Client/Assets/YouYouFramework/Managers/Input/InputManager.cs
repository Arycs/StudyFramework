using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
                t => OnClick?.Invoke(t),
                t => OnBeginDrag?.Invoke(t),
                t => OnEndDrag?.Invoke(t),
                (t1, t2) => OnDrag?.Invoke(t1, t2),
                t => OnZoom?.Invoke(t)
            );
#else
            //移动端
#endif
        }

        private List<RaycastResult> raycastResults = new List<RaycastResult>();

        /// <summary>
        /// 判断UI穿透
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public bool IsPointerOverGameObject(Vector2 screenPosition)
        {
            //实例化点击事件
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            //将点击事件的屏幕坐标赋值给点击事件
            eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

            raycastResults.Clear();
            //向点击处发射射线
            EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastResults);
            return raycastResults.Count > 0;
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