using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class BaseSprite : MonoBehaviour
    {
        private void Awake()
        {
            OnAwake();
        }

        

        // Start is called before the first frame update
        void Start()
        {
            OnInit();
            OnOpen();
        }

        private void OnDestroy()
        {
            OnBeforDestroy();
        }

        /// <summary>
        /// 自动初始化  注意与Init的区别
        /// </summary>
        protected virtual void OnInit()
        {
            
        }
        
        protected virtual void OnAwake()
        {
            
        }
        
        /// <summary>
        /// 打开
        /// </summary>
        public virtual void OnOpen()
        {
            
        }


        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void OnClose()
        {
            
        }

        /// <summary>
        /// 销毁
        /// </summary>
        protected virtual void OnBeforDestroy()
        {
            
        }
    }
}