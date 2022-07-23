using System;
using System.Collections.Generic;
using System.Reflection;
using YouYouServer.Common;
using YouYouServer.Core;

namespace YouYouServer.Model
{
    public sealed class HotFixHelper
    {
        public static Dictionary<string, Type> HandlerTypeDic = new Dictionary<string, Type>();

        public static Action OnLoadAssembly;

        /// <summary>
        /// 加载热更新程序集
        /// </summary>
        public static void LoadHotFixAssembly()
        {
            string assemblyName = HotFixConfig.GetParams("HotFixAssemblyName");
            //根据路径加载程序集
            Assembly assembly = Assembly.LoadFile(System.AppDomain.CurrentDomain.BaseDirectory + assemblyName);
            //获取程序集中的类型
            Type[] types = assembly.GetTypes(); 

            int len = types.Length;
            for (int i = 0; i < len; i++)
            {
                Type type = types[i];
                //获取指定类型的属性标记 -- 需要获取到的 提前标记一下
                object[] objects = type.GetCustomAttributes(typeof(HandlerAttribute), true);
                if (objects.Length == 0)
                {
                    continue;
                }

                HandlerAttribute handlerAttribute = (HandlerAttribute)objects[0];
                HandlerTypeDic[handlerAttribute.TypeName] = type;
            }

            OnLoadAssembly?.Invoke();

            Console.WriteLine("LoadHotFixAssembly Success");
        }
    }
}
