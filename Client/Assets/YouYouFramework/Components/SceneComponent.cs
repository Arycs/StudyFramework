using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    /// <summary>
    /// 场景组件
    /// </summary>
    public class SceneComponent : YouYouBaseComponent,IUpdateComponent
    {
        private YouYouSceneManager m_YouYouSceneManager;

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
            m_YouYouSceneManager = new YouYouSceneManager();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneId"></param>
        public void LoadScene(int sceneId,bool showLoadingForm = false,BaseAction onComplete = null)
        {
            m_YouYouSceneManager.LoadScene(sceneId,showLoadingForm,onComplete);
        }

        public override void Shutdown()
        {
            GameEntry.RemoveUpdateComponent(this);
        }

        public void OnUpdate()
        {
            m_YouYouSceneManager.OnUpdate();
        }
    }
}
