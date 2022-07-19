﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 变量基类
    /// </summary>
    public abstract class VariableBase
    {
        /// <summary>
        /// 获取变量类型
        /// </summary>
        public abstract Type type { get; }

        /// <summary>
        /// 引用计数
        /// </summary>
        private byte ReferenceCount { get; set; }


        /// <summary>
        /// 保留对象 , 基本上用于协程 ,因为同步消息的话 不释放就不会变,协程的话如果先释放了,再在协程中使用就会出现数据错误
        /// </summary>
        public void Retain()
        {
            ReferenceCount++;
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Release()
        {
            ReferenceCount--;
            if (ReferenceCount < 1)
            {
                //回池操作
                GameEntry.Pool.EnqueueVarObject(this);
            }
        }
    }

}