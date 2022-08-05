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
            
            //加载摇杆
            GameEntry.UI.OpenUIForm(UIFormId.UI_Joystick,null,(form =>
            {
                GameEntry.Input.Joystick.OnChanged = v =>
                {
                    GameEntry.Data.RoleDataManager.CurrPlayer.JoystickMove(v);
                };
                GameEntry.Input.Joystick.OnUp = v =>
                {
                    GameEntry.Data.RoleDataManager.CurrPlayer.JoystickStop(v);
                };
            }));
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
            
            //TODO 这里调用会导致 游戏关闭时 UI层级调用出问题 
            //关闭摇杆
            //GameEntry.UI.CloseUIForm(UIFormId.UI_Joystick);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        #region 输入事件

        private void Input_OnClick(TouchEventData t1)
        {
            if (GameEntry.Data.RoleDataManager.CurrPlayer == null)
            {
               return;
            }

            Ray ray = GameEntry.CameraCtrl.MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray,out hitInfo, 1000f, 1 << LayerMask.NameToLayer("Ground")))
            {
                GameEntry.Data.RoleDataManager.CurrPlayer.MoveTo(hitInfo.point);
            }
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
            //防止UI 穿透
            if (GameEntry.Input.IsPointerOverGameObject(Input.mousePosition))
            {
                return;
            }
            
            //摇杆拖拽中, 禁止滑动摄像机
            if (GameEntry.Input.Joystick.IsDraging)
            {
                return;
            }
            
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
            //防止UI 穿透
            if (GameEntry.Input.IsPointerOverGameObject(Input.mousePosition))
            {
                return;
            }
            
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