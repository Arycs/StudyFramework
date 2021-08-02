using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace YouYou
{
    public static class LuaGenConfig
    {
        //Lua中 要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>() { 
            typeof(YouYou.SocketEvent),
            typeof(YouYou.CommonEvent),
            typeof(YouYou.BaseParams),
            typeof(BaseParams),
            typeof(HttpCallBackArgs),
        };

        //C# 静态调用Lua的配置 （包括事件的原型）， 仅可以配delegate, interface
        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(HttpSendDataCallBack),
        };
    }
}
