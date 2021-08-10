using System;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace YouYou
{
    /// <summary>
    /// Lua组件的类型
    /// </summary>
    public enum LuaComType
    {
        GameObject = 0,
        Transform = 1,
        Button = 2,
        Image = 3,
        YouYouImage = 4,
        Text = 5,
        YouYouText = 6,
        RawImage = 7,
        InputField = 8,
        Scrollbar = 9,
        ScrollView = 10,
        MulityScroller = 11,
    }

    /// <summary>
    /// Lua窗口Form
    /// </summary>
    [LuaCallCSharp]
    public class LuaForm : UIFormBase
    {
        [CSharpCallLua]
        public delegate void OnInitHandler(Transform transform, object userData);

        private OnInitHandler onInit;

        [CSharpCallLua]
        public delegate void OnOpenHandler(object userData);

        private OnOpenHandler onOpen;

        [CSharpCallLua]
        public delegate void OnCloseHandler();

        private OnCloseHandler onClose;

        [CSharpCallLua]
        public delegate void OnBeforDestroyHandler();

        private OnBeforDestroyHandler onBeforDestroy;

        public TextAsset luaScript;
        [Header("Lua组件")]
        [SerializeField]
        public LuaComGroup[] m_LuaComGroups;

        //public LuaCom[] LuaComs
        //{
        //    get { return m_LuaComs; }
        //}

        /// <summary>
        /// 根据索引来查找组件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void SetLuaComs()
        {
            int len = m_LuaComGroups.Length;
            for (int i = 0; i < len; i++)
            {
                LuaComGroup group = m_LuaComGroups[i];
                int lenCom = group.LuaComs.Length;
                for (int j = 0; j < lenCom; j++)
                {
                    LuaCom com = group.LuaComs[j];
                    object obj = null;
                    switch (com.Type)
                    {
                        case LuaComType.GameObject:
                            obj = com.Trans.gameObject;
                            break;
                        case LuaComType.Transform:
                            obj = com.Trans;
                            break;
                        case LuaComType.Button:
                            obj = com.Trans.GetComponent<Button>();
                            break;
                        case LuaComType.Image:
                            obj = com.Trans.GetComponent<Image>();
                            break;
                        case LuaComType.YouYouImage:
                            obj = com.Trans.GetComponent<YouYouImage>();
                            break;
                        case LuaComType.Text:
                            obj = com.Trans.GetComponent<Text>();
                            break;
                        case LuaComType.YouYouText:
                            obj = com.Trans.GetComponent<YouYouText>();
                            break;
                        case LuaComType.RawImage:
                            obj = com.Trans.GetComponent<RawImage>();
                            break;
                        case LuaComType.InputField:
                            obj = com.Trans.GetComponent<InputField>();
                            break;
                        case LuaComType.Scrollbar:
                            obj = com.Trans.GetComponent<Scrollbar>();
                            break;
                        case LuaComType.ScrollView:
                            obj = com.Trans.GetComponent<ScrollRect>();
                            break;
                        case LuaComType.MulityScroller:
                            obj = com.Trans.GetComponent<UIMultiScroller>();
                            break;
                    }
                    scriptEnv.Set(com.Name, obj);
                }
            }
        }

        private LuaTable scriptEnv;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            if (luaScript == null)
            {
                return;
            }

            if (LuaManager.luaEnv == null)
            {
                return;
            }
            scriptEnv = LuaManager.luaEnv.NewTable();

            //为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量，函数冲突
            LuaTable meta = LuaManager.luaEnv.NewTable();
            meta.Set("__index", LuaManager.luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
            SetLuaComs();

            LuaManager.luaEnv.DoString(luaScript.text, "LuaBehaviour", scriptEnv);
            onInit = scriptEnv.Get<OnInitHandler>("OnInit");
            onOpen = scriptEnv.Get<OnOpenHandler>("OnOpen");
            onClose = scriptEnv.Get<OnCloseHandler>("OnClose");
            onBeforDestroy = scriptEnv.Get<OnBeforDestroyHandler>("OnBeforDestroy");

            onInit?.Invoke(transform, userData);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (onOpen != null)
            {
                onOpen(userData);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (onClose != null)
            {
                onClose();
            }
        }

        protected override void OnBeforDestroy()
        {
            base.OnBeforDestroy();
            if (onBeforDestroy != null)
            {
                onBeforDestroy();
            }

            onInit = null;
            onOpen = null;
            onClose = null;
            onBeforDestroy = null;

            //卸载图片

            int len = m_LuaComGroups.Length;

            for (int i = 0; i < len; i++)
            {
                LuaComGroup group = m_LuaComGroups[i];
                int lenCom = group.LuaComs.Length;
                for (int j = 0; j < lenCom; j++)
                {
                    group.LuaComs[j].Trans = null;
                    group.LuaComs[j] = null;
                }
                group = null;
            }
        }
    }

    /// <summary>
    /// Lua组件分组
    /// </summary>
    [Serializable]
    public class LuaComGroup
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// Lua组件数组
        /// </summary>
        public LuaCom[] LuaComs;
    }

    /// <summary>
    /// Lua组件
    /// </summary>
    [Serializable]
    public class LuaCom
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        public LuaComType Type;

        /// <summary>
        /// 变化
        /// </summary>
        public Transform Trans;
    }
}