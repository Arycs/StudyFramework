using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class UIManager : ManagerBase
    {
        /// <summary>
        /// 已经打开的UI链表
        /// </summary>
        private LinkedList<UIFormBase> m_OpenUIFormList;

        public UIManager()
        {
            m_OpenUIFormList = new LinkedList<UIFormBase>();
        }

        #region  OpenUIForm 打开UI窗体

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormId">formId</param>
        /// <param name="userData">用户数据</param>
        internal void OpenUIForm(int uiFormId, object userData = null,BaseAction<UIFormBase> onOpen = null)
        {
            if (IsExists(uiFormId))
            {
                return;
            }

            //1. 读表
            Sys_UIFormEntity entity = GameEntry.DataTable.Sys_UIFormDBModel.Get(uiFormId);
            if (entity == null)
            {
                Debug.LogError("表格中没有对应UI窗体数据 : " + uiFormId);
                return;
            }

            UIFormBase formBase = GameEntry.UI.Dequeue(uiFormId); //以后从对象池获取
            if (formBase == null)
            {
                //TODO : 异步加载UI需要时间 此处需要处理过滤加载中的UI

                string assetPath = string.Empty;
                switch (GameEntry.Localization.CurrLanguage)
                {
                    case YouYouLanguage.Chinese:
                        assetPath = entity.AssetPath_Chinese;
                        break;
                    case YouYouLanguage.English:
                        assetPath = entity.AssetPath_English;
                        break;
                }

                LoadUIAsset(assetPath, (ResourceEntity resourceEntity) =>
                {
                    GameObject uiObj = Object.Instantiate((Object)resourceEntity.Target) as GameObject;

                    //把克隆出的资源 加入实例资源池
                    GameEntry.Pool.RegisterInstanceResource(uiObj.GetInstanceID(), resourceEntity);
                    
                    uiObj.transform.SetParent(GameEntry.UI.GetUIGroup(entity.UIGroupId).Group);
                    uiObj.transform.localPosition = Vector3.zero;
                    uiObj.transform.localScale = Vector3.one;

                    formBase = uiObj.GetComponent<UIFormBase>();
                    formBase.Init(uiFormId, entity.UIGroupId, entity.DisableUILayer == 1, entity.IsLock == 1, userData);
                    m_OpenUIFormList.AddLast(formBase);

                    if (onOpen != null)
                    {
                        onOpen(formBase);
                    }
                });
            }
            else
            {
                formBase.gameObject.SetActive(true);
                formBase.Open(userData);
                m_OpenUIFormList.AddLast(formBase);

                if (onOpen != null)
                {
                    onOpen(formBase);
                }
            }
        }

        #endregion

        #region LoadUIAsset 加载UI资源

        private void LoadUIAsset(string assetPath, BaseAction<ResourceEntity> onComplete)
        {
            GameEntry.Resource.ResourceLoaderManager.LoadMainAsset(AssetCategory.UIPrefab,
                string.Format("Assets/Download/UI/UIPrefab/{0}.prefab", assetPath),
                (ResourceEntity resourceEntity) =>
                {
                    if (onComplete != null)
                    {
                        onComplete(resourceEntity);
                    }
                });
        }

        #endregion

        /// <summary>
        /// 检查UI是否已经打开
        /// </summary>
        /// <returns></returns>
        public bool IsExists(int uiformId)
        {
            for (LinkedListNode<UIFormBase> curr = m_OpenUIFormList.First; curr != null; curr = curr.Next)
            {
                if (curr.Value.UIFormId == uiformId)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 根据uiformId 关闭UI窗口
        /// </summary>
        /// <param name="uiformId"></param>
        internal void CloseUIForm(int uiformId)
        {
            for (LinkedListNode<UIFormBase> curr = m_OpenUIFormList.First; curr != null; curr = curr.Next)
            {
                if (curr.Value.UIFormId == uiformId)
                {
                    CloseUIForm(curr.Value);
                    break;
                }
            }
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="formBase"></param>
        internal void CloseUIForm(UIFormBase formBase)
        {
            formBase.ToClose();
            m_OpenUIFormList.Remove(formBase);
        }

        public override void Init()
        {
           
        }
    }
}