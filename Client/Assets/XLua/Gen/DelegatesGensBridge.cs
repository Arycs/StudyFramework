#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;


namespace XLua
{
    public partial class DelegateBridge : DelegateBridgeBase
    {
		
		public void __Gen_Delegate_Imp0()
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int errFunc = LuaAPI.pcall_prepare(L, errorFuncRef, luaReference);
                
                
                PCall(L, 0, 0, errFunc);
                
                
                
                LuaAPI.lua_settop(L, errFunc - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp1(YouYou.HttpCallBackArgs p0)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int errFunc = LuaAPI.pcall_prepare(L, errorFuncRef, luaReference);
                ObjectTranslator translator = luaEnv.translator;
                translator.Push(L, p0);
                
                PCall(L, 1, 0, errFunc);
                
                
                
                LuaAPI.lua_settop(L, errFunc - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp2(int p0, UnityEngine.GameObject p1)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int errFunc = LuaAPI.pcall_prepare(L, errorFuncRef, luaReference);
                ObjectTranslator translator = luaEnv.translator;
                LuaAPI.xlua_pushinteger(L, p0);
                translator.Push(L, p1);
                
                PCall(L, 2, 0, errFunc);
                
                
                
                LuaAPI.lua_settop(L, errFunc - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp3(int p0)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int errFunc = LuaAPI.pcall_prepare(L, errorFuncRef, luaReference);
                
                LuaAPI.xlua_pushinteger(L, p0);
                
                PCall(L, 1, 0, errFunc);
                
                
                
                LuaAPI.lua_settop(L, errFunc - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp4(object p0)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int errFunc = LuaAPI.pcall_prepare(L, errorFuncRef, luaReference);
                ObjectTranslator translator = luaEnv.translator;
                translator.PushAny(L, p0);
                
                PCall(L, 1, 0, errFunc);
                
                
                
                LuaAPI.lua_settop(L, errFunc - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp5(byte[] p0)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int errFunc = LuaAPI.pcall_prepare(L, errorFuncRef, luaReference);
                
                LuaAPI.lua_pushstring(L, p0);
                
                PCall(L, 1, 0, errFunc);
                
                
                
                LuaAPI.lua_settop(L, errFunc - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		public void __Gen_Delegate_Imp6(UnityEngine.Transform p0, object p1)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
                RealStatePtr L = luaEnv.rawL;
                int errFunc = LuaAPI.pcall_prepare(L, errorFuncRef, luaReference);
                ObjectTranslator translator = luaEnv.translator;
                translator.Push(L, p0);
                translator.PushAny(L, p1);
                
                PCall(L, 2, 0, errFunc);
                
                
                
                LuaAPI.lua_settop(L, errFunc - 1);
                
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
        
		static DelegateBridge()
		{
		    Gen_Flag = true;
		}
		
		public override Delegate GetDelegateByType(Type type)
		{
		
		    if (type == typeof(YouYou.BaseAction))
			{
			    return new YouYou.BaseAction(__Gen_Delegate_Imp0);
			}
		
		    if (type == typeof(UISelectRoleDragView.OnDoubleClickHandler))
			{
			    return new UISelectRoleDragView.OnDoubleClickHandler(__Gen_Delegate_Imp0);
			}
		
		    if (type == typeof(YouYou.LuaForm.OnCloseHandler))
			{
			    return new YouYou.LuaForm.OnCloseHandler(__Gen_Delegate_Imp0);
			}
		
		    if (type == typeof(YouYou.LuaForm.OnBeforDestroyHandler))
			{
			    return new YouYou.LuaForm.OnBeforDestroyHandler(__Gen_Delegate_Imp0);
			}
		
		    if (type == typeof(YouYou.HttpSendDataCallBack))
			{
			    return new YouYou.HttpSendDataCallBack(__Gen_Delegate_Imp1);
			}
		
		    if (type == typeof(PageView.OnItemCreateHandler))
			{
			    return new PageView.OnItemCreateHandler(__Gen_Delegate_Imp2);
			}
		
		    if (type == typeof(UIMultiScroller.OnItemCreateHandler))
			{
			    return new UIMultiScroller.OnItemCreateHandler(__Gen_Delegate_Imp2);
			}
		
		    if (type == typeof(UISelectRoleDragView.OnDragingHandler))
			{
			    return new UISelectRoleDragView.OnDragingHandler(__Gen_Delegate_Imp3);
			}
		
		    if (type == typeof(UISelectRoleDragView.OnDragCompleteHandler))
			{
			    return new UISelectRoleDragView.OnDragCompleteHandler(__Gen_Delegate_Imp3);
			}
		
		    if (type == typeof(YouYou.CommonEvent.OnActionHandler))
			{
			    return new YouYou.CommonEvent.OnActionHandler(__Gen_Delegate_Imp4);
			}
		
		    if (type == typeof(YouYou.LuaForm.OnOpenHandler))
			{
			    return new YouYou.LuaForm.OnOpenHandler(__Gen_Delegate_Imp4);
			}
		
		    if (type == typeof(YouYou.SocketEvent.OnActionHandler))
			{
			    return new YouYou.SocketEvent.OnActionHandler(__Gen_Delegate_Imp5);
			}
		
		    if (type == typeof(YouYou.LuaForm.OnInitHandler))
			{
			    return new YouYou.LuaForm.OnInitHandler(__Gen_Delegate_Imp6);
			}
		
		    return null;
		}
	}
    
}