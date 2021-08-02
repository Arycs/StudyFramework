using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace YouYou
{
    public static class LuaGenConfig
    {
        //Lua�� Ҫʹ�õ�C#������ã�����C#��׼�⣬����Unity API����������ȡ�
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>() { 
            typeof(YouYou.SocketEvent),
            typeof(YouYou.CommonEvent),
            typeof(YouYou.BaseParams),
            typeof(BaseParams),
            typeof(HttpCallBackArgs),
        };

        //C# ��̬����Lua������ �������¼���ԭ�ͣ��� ��������delegate, interface
        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(HttpSendDataCallBack),
        };
    }
}
