#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class UIMultiScrollerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UIMultiScroller);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 11, 11);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetScroller", _m_ResetScroller);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnValueChange", _m_OnValueChange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPosition", _m_GetPosition);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "DataCount", _g_get_DataCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_movement", _g_get__movement);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maxPerLine", _g_get_maxPerLine);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "spacingX", _g_get_spacingX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "spacingY", _g_get_spacingY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellWidth", _g_get_cellWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellHeight", _g_get_cellHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "viewCount", _g_get_viewCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "itemPrefab", _g_get_itemPrefab);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_content", _g_get__content);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnItemCreate", _g_get_OnItemCreate);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "DataCount", _s_set_DataCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_movement", _s_set__movement);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maxPerLine", _s_set_maxPerLine);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "spacingX", _s_set_spacingX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "spacingY", _s_set_spacingY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellWidth", _s_set_cellWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellHeight", _s_set_cellHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "viewCount", _s_set_viewCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "itemPrefab", _s_set_itemPrefab);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_content", _s_set__content);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnItemCreate", _s_set_OnItemCreate);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UIMultiScroller gen_ret = new UIMultiScroller();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UIMultiScroller constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetScroller(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetScroller(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnValueChange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _pos;translator.Get(L, 2, out _pos);
                    
                    gen_to_be_invoked.OnValueChange( _pos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _i = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GetPosition( _i );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DataCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DataCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__movement(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._movement);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxPerLine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.maxPerLine);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_spacingX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.spacingX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_spacingY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.spacingY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.cellWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.cellHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_viewCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.viewCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_itemPrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.itemPrefab);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__content(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._content);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnItemCreate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnItemCreate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DataCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DataCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__movement(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                UIMultiScroller.Arrangement gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked._movement = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maxPerLine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maxPerLine = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_spacingX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.spacingX = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_spacingY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.spacingY = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cellWidth = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cellHeight = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_viewCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.viewCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_itemPrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.itemPrefab = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__content(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._content = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnItemCreate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UIMultiScroller gen_to_be_invoked = (UIMultiScroller)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnItemCreate = translator.GetDelegate<UIMultiScroller.OnItemCreateHandler>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
