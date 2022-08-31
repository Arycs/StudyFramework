using System.Collections.Generic;
using UnityEngine;

namespace YouYouServer.Core.Utils
{
    public class GameUtil
    {
        /// <summary>
        /// 计算路径的长度
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static float GetPathLen(List<UnityEngine.Vector3> path)
        {
            float pathLen = 0f; //路径的总长度 计算出路径
            int len = path.Count;
            for (int i = 0; i < len; i++)
            {
                if (i == len - 1) continue;

                float dis = Vector3.Distance(path[i], path[i + 1]);
                pathLen += dis;
            }

            return pathLen;
        }
    }
}