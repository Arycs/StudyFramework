using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 世界地图流程
    /// </summary>
    public partial class ProcedureWorldMap : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GameEntry.Log(LogCategory.Procedure, "OnEnter ProcedureWorldMap");
            //加载场景
            GameEntry.Scene.LoadScene(SysScene.DaLi, false, (() =>
            {
                GameEntry.Event.CommonEvent.Dispatch(SysEventId.CloseCheckVersionUI);

                LoadWorldMapComplete();
            }));

            GameEntry.Input.OnClick += Input_OnClick;
            GameEntry.Input.OnBeginDrag += Input_OnBeginDrag;
            GameEntry.Input.OnEndDrag += Input_OnEndDrag;
            GameEntry.Input.OnDrag += Input_OnDrag;
            GameEntry.Input.OnZoom += Input_OnZoom;
        }


        /// <summary>
        /// 加载PVP场景完毕
        /// </summary>
        private void LoadWorldMapComplete()
        {
            GameEntry.Data.RoleDataManager.CreatePlayerByJobId(JobId.CiKe, (roleCtrl =>
            {
                //打开摄像机
                var position = roleCtrl.transform.position;
                GameEntry.CameraCtrl.transform.position = position;
                GameEntry.CameraCtrl.AutoLookAt(position);
                GameEntry.CameraCtrl.SetCameraOpen(true);
            }));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            GameEntry.Input.OnClick -= Input_OnClick;
            GameEntry.Input.OnBeginDrag -= Input_OnBeginDrag;
            GameEntry.Input.OnEndDrag -= Input_OnEndDrag;
            GameEntry.Input.OnDrag -= Input_OnDrag;
            GameEntry.Input.OnZoom -= Input_OnZoom;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        #region 输入事件

        private void Input_OnClick(TouchEventData t1)
        {
        }

        private void Input_OnBeginDrag(TouchEventData t1)
        {
        }

        private void Input_OnEndDrag(TouchEventData t1)
        {
            GameEntry.CameraCtrl.IsOnDrag = false;
            GameEntry.CameraCtrl.OnDragEndDistance = t1.Delta.x;
        }

        private void Input_OnDrag(TouchDirection t1, TouchEventData t2)
        {
            GameEntry.CameraCtrl.IsOnDrag = true;
            switch (t1)
            {
                case TouchDirection.MoveLeft:
                    GameEntry.CameraCtrl.SetCameraRotate(0);
                    break;
                case TouchDirection.MoveRight:
                    GameEntry.CameraCtrl.SetCameraRotate(1);
                    break;
                case TouchDirection.MoveUp:
                    GameEntry.CameraCtrl.SetCameraUpAndDown(1);
                    break;
                case TouchDirection.MoveDown:
                    GameEntry.CameraCtrl.SetCameraUpAndDown(0);
                    break;
                case TouchDirection.MoveNone:
                    break;
            }
        }

        private void Input_OnZoom(ZoomType t1)
        {
            switch (t1)
            {
                case ZoomType.In:
                    GameEntry.CameraCtrl.SetCameraZoom(0);
                    break;
                case ZoomType.Out:
                    GameEntry.CameraCtrl.SetCameraZoom(1);
                    break;
            }
        }

        #endregion
    }
}