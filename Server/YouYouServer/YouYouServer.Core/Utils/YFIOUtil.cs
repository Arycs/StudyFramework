using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YouYouServer.Core.Utils
{
   public  class YFIOUtil
    {
        public static byte[] GetBuffer(string path)
        {
            byte[] buffer = null;

            using (FileStream fs = new FileStream(path,FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}
