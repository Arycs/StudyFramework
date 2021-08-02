﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    public enum YouYouLanguage
    {
        /// <summary>
        /// 中文
        /// </summary>
        Chinese = 0,
        /// <summary>
        /// 英文
        /// </summary>
        English = 1
    }

    public class LocalizationManager : ManagerBase
    {
        [SerializeField]
        private YouYouLanguage m_CurrLanguage;
        /// <summary>
        /// 当前语言(要和本地化表的语言字段一致)
        /// </summary>
        public YouYouLanguage CurrLanguage
        {
            get
            {
                return m_CurrLanguage;
            }
        }

        private LocalizationManager m_LocalizationManager;

        /// <summary>
        /// 获取本地化文本内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetString(string key,params object[] args)
        {
            string value = null;
            if (GameEntry.DataTable.LocalizationDBModel.LocalizationDic.TryGetValue(key,out value))
            {
                return string.Format(value,args);
            }

            return value;
        }
    }
}
