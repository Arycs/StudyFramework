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
    public class YouYouLuaComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(YouYou.LuaComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 2, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadDataTable", _m_LoadDataTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSocketReceiveMS", _m_LoadSocketReceiveMS);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DequeueMemoryStream", _m_DequeueMemoryStream);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DequeueMemorystreamAndLoadBuffer", _m_DequeueMemorystreamAndLoadBuffer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnqueueMemoryStream", _m_EnqueueMemoryStream);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetByteArray", _m_GetByteArray);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendHttpData", _m_SendHttpData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Shutdown", _m_Shutdown);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadDataTableMS", _g_get_LoadDataTableMS);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DebugLogProto", _g_get_DebugLogProto);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "DebugLogProto", _s_set_DebugLogProto);
            
			
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
					
					YouYou.LuaComponent gen_ret = new YouYou.LuaComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to YouYou.LuaComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadDataTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _tableName = LuaAPI.lua_tostring(L, 2);
                    YouYou.BaseAction<MMO_MemoryStream> _onComplete = translator.GetDelegate<YouYou.BaseAction<MMO_MemoryStream>>(L, 3);
                    
                    gen_to_be_invoked.LoadDataTable( _tableName, _onComplete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSocketReceiveMS(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                        MMO_MemoryStream gen_ret = gen_to_be_invoked.LoadSocketReceiveMS( _buffer );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DequeueMemoryStream(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        MMO_MemoryStream gen_ret = gen_to_be_invoked.DequeueMemoryStream(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DequeueMemorystreamAndLoadBuffer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                        MMO_MemoryStream gen_ret = gen_to_be_invoked.DequeueMemorystreamAndLoadBuffer( _buffer );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnqueueMemoryStream(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    MMO_MemoryStream _ms = (MMO_MemoryStream)translator.GetObject(L, 2, typeof(MMO_MemoryStream));
                    
                    gen_to_be_invoked.EnqueueMemoryStream( _ms );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetByteArray(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    MMO_MemoryStream _ms = (MMO_MemoryStream)translator.GetObject(L, 2, typeof(MMO_MemoryStream));
                    int _len = LuaAPI.xlua_tointeger(L, 3);
                    
                        byte[] gen_ret = gen_to_be_invoked.GetByteArray( _ms, _len );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendHttpData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    YouYou.HttpSendDataCallBack _callBack = translator.GetDelegate<YouYou.HttpSendDataCallBack>(L, 3);
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.SendHttpData( _url, _callBack, _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Shutdown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Shutdown(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadDataTableMS(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadDataTableMS);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DebugLogProto(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DebugLogProto);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DebugLogProto(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                YouYou.LuaComponent gen_to_be_invoked = (YouYou.LuaComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DebugLogProto = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
