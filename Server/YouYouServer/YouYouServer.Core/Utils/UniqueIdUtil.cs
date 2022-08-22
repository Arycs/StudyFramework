using System;

namespace YouYouServer.Core.Utils
{
    public class UniqueIdUtil
    {
        /// <summary>
        /// 获取唯一编号
        /// </summary>
        /// <returns></returns>
        public static long GetUniqueId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}