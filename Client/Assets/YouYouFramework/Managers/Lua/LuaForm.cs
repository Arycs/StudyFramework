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

        private LuaTable scriptEnv;
        private LuaEnv luaEnv;

        [Header("Lua组件")] 
        [SerializeField] 
        private LuaCom[] m_LuaComs;

        public LuaCom[] LuaComs
        {
            get { return m_LuaComs; }
        }

        /// <summary>
        /// 根据索引来查找组件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetLuaComs(int index)
        {
            LuaCom com = m_LuaComs[index];
            switch (com.Type)
            {
                case LuaComType.GameObject:
                    return com.Trans.gameObject;
                case LuaComType.Transform:
                    return com.Trans;
                case LuaComType.Button:
                    return com.Trans.GetComponent<Button>();
                case LuaComType.Image:
                    return com.Trans.GetComponent<Image>();
                case LuaComType.YouYouImage:
                    return com.Trans.GetComponent<YouYouImage>();
                case LuaComType.Text:
                    return com.Trans.GetComponent<Text>();
                case LuaComType.YouYouText:
                    return com.Trans.GetComponent<YouYouText>();  
                case LuaComType.RawImage:
                    return com.Trans.GetComponent<RawImage>();
                case LuaComType.InputField:
                    return com.Trans.GetComponent<InputField>();
                case LuaComType.Scrollbar:
                    return com.Trans.GetComponent<Scrollbar>();
                case LuaComType.ScrollView:
                    return com.Trans.GetComponent<ScrollRect>();
                case LuaComType.MulityScroller:
                    return com.Trans.GetComponent<UIMultiScroller>();
            }

            return com.Trans;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            luaEnv = LuaManager.luaEnv; //此处要从LuaManager上获取 全局只有一个
            if (luaEnv == null)
            {
                return;
            }

            string prefabName = name;
            if (prefabName.Contains("(Clone)"))
            {
                prefabName = prefabName.Split(new string[] {"(Clone)"}, StringSplitOptions.RemoveEmptyEntries)[0] +
                             "View";
            }

            onInit = luaEnv.Global.GetInPath<OnInitHandler>(prefabName + ".OnInit");
            onOpen = luaEnv.Global.GetInPath<OnOpenHandler>(prefabName + ".OnOpen");
            onClose = luaEnv.Global.GetInPath<OnCloseHandler>(prefabName + ".OnClose");
            onBeforDestroy = luaEnv.Global.GetInPath<OnBeforDestroyHandler>(prefabName + ".OnBeforDestroy");
           
            if (onInit != null)
            {
                onInit(transform, userData);
            }
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
            int len = m_LuaComs.Length;
            for (int i = 0; i < len; i++)
            {
                LuaCom com = m_LuaComs[i];
                com.Trans = null;
                com = null;
            }
        }
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