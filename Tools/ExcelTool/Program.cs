using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//如果要支持xlsx格式表格，请在本机电脑安装这个
//http://download.microsoft.com/download/7/0/3/703ffbcb-dc0c-4e19-b0da-1463960fdcdb/AccessDatabaseEngine.exe

namespace ExcelTool
{
    class Program
    {
        /// <summary>
        /// 源Excel文件路径
        /// </summary>
        private static string SourceExcelPath;
        /// <summary>
        /// 导出bytes文件路径
        /// </summary>
        private static string OutBytesFilePath;
        /// <summary>
        /// 导出C#脚本路径
        /// </summary>
        private static string OutCSharpFilePath;
        /// <summary>
        /// 服务器端表格文件路径
        /// </summary>
        private static string OutBytesFilePath_Server;
        /// <summary>
        /// 服务器端c#脚本路径
        /// </summary>
        private static string OutCSharpFilePath_Server;
        /// <summary>
        /// 客户端生成Sys表格ID路径
        /// </summary>
        private static string OutConfigIdPath_Client;
        /// <summary>
        /// 客户端生成Sys表格ID路径
        /// </summary>
        private static string OutConfigIdPath_Server;

        /// <summary>
        /// 生成对应ID的文件名称列表
        /// </summary>
        private static Dictionary<string, string> OutConfigFileList;

        static void Main(string[] args)
        {
            LoadConfig();
            ReadFiles(SourceExcelPath);

            Console.WriteLine("全部生成完毕");
            Console.ReadLine();
        }
        /// <summary>
        /// 加载配置文件
        /// </summary>
        private static void LoadConfig()
        {
            //获取当前路径的config.txt文件
            string configPath = Environment.CurrentDirectory + "/config.txt";
            if (File.Exists(configPath))
            {
                string str = "";
                using (FileStream fs = new FileStream(configPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        str = sr.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(str))
                {
                    string[] arr = str.Split('\n');
                    if (arr.Length >= 4)
                    {
                        SourceExcelPath = arr[0].Trim();
                        OutBytesFilePath = arr[1].Trim();
                        OutCSharpFilePath = arr[2].Trim();
                        OutBytesFilePath_Server = arr[3].Trim();
                        OutCSharpFilePath_Server = arr[4].Trim();
                        OutConfigIdPath_Client = arr[5].Trim();
                        OutConfigIdPath_Server = arr[6].Trim();
                    }
                }
            }

            OutConfigFileList = new Dictionary<string, string>();
            //获取当前路径的GenerateIDConfig.txt文件
            string generateIDConfigPath = Environment.CurrentDirectory + "/GenerateIDConfig.txt";
            if (File.Exists(generateIDConfigPath))
            {
                string str = "";
                using (FileStream fs = new FileStream(generateIDConfigPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        str = sr.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(str))
                {
                    string[] arr = str.Split('\n');
                    int len = arr.Length;
                    for (int i = 0; i < len; i++)
                    {
                        OutConfigFileList.Add(arr[i].Replace("\r",""), arr[i]);
                    }
                }
            }
        }

        public static void ReadFiles(string path)
        {
            // 获取原始路径下文件夹
            string[] arr = Directory.GetDirectories(path);

            int len = arr.Length;
            for (int i = 0; i < len; i++)
            {
                //获取不同文件夹下的文件路径
                string[] dtArr = Directory.GetFiles(arr[i]);
                int fileLen = dtArr.Length;
                for (int j = 0; j < fileLen; j++)
                {
                    string filePath = dtArr[j];
                    FileInfo file = new FileInfo(filePath);
                    if (file.Name.IndexOf("~$") > -1)
                    {
                        continue;
                    }
                    if (file.Extension.Equals(".xls") || file.Extension.Equals(".xlsx"))
                    {
                        ReadData(file.Extension.Equals(".xls"), file.FullName, file.Name.Substring(0, file.Name.LastIndexOf('.')));
                    }
                }
            }

        }


        private static void ReadData(bool isXls, string filePath, string fileName)
        {

            if (string.IsNullOrEmpty(filePath))
                return;

            //把表格复制一下
            string newPath = filePath + ".temp";

            File.Copy(filePath, newPath, true);

            string tableName = "Sheet1";
            string strConn = "";
            if (isXls)
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + newPath + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
            }
            else
            {
                strConn = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + newPath + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1'";
            }

            DataTable dt = null;

            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = string.Format("select * from [{0}$]", tableName);
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            dt = ds.Tables[0];
            myCommand.Dispose();

            File.Delete(newPath);

            if (fileName.Equals("DTSys_Localization", StringComparison.CurrentCultureIgnoreCase))
            {
                CreateLocalization(fileName, dt);
            }
            else
            {
                CreateData(fileName, dt);
            }
        }

        #region 创建普通表
        //表头
        static string[,] tableHeadArr = null;

        private static void CreateData(string fileName, DataTable dt)
        {
            try
            {
                //数据格式 行数 列数 二维数组每项的值 这里不做判断 都用string存储
                tableHeadArr = null;

                //注释行 行数
                int notesRowsNum = -1;

                byte[] buffer = null;

                using (MMO_MemoryStream ms = new MMO_MemoryStream())
                {
                    //表格行数
                    int rows = dt.Rows.Count;
                    //表格列数
                    int columns = dt.Columns.Count;
                    if (OutConfigFileList.ContainsKey(fileName))
                    {
                        tableHeadArr = new string[columns, rows];
                    }
                    else
                    {
                        tableHeadArr = new string[columns, 3];
                    }

                    ms.WriteInt(rows - 3); //减去表头的三行
                    ms.WriteInt(columns);
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            if (i < 3)
                            {
                                tableHeadArr[j, i] = dt.Rows[i][j].ToString().Trim();
                            }
                            else
                            {
                                if (OutConfigFileList.ContainsKey(fileName))
                                {
                                    tableHeadArr[j, i] = dt.Rows[i][j].ToString().Trim();
                                }
                                string type = tableHeadArr[j, 1];
                                string value = dt.Rows[i][j].ToString().Trim();
                                //判断 以#开头的行为注释行, 不用进行数据管理
                                if (value.StartsWith("#"))
                                {
                                    notesRowsNum = i;
                                    continue;
                                }
                                if (notesRowsNum == i)
                                {
                                    continue;
                                }

                                //Console.WriteLine("type=" + type + "||" + "value=" + value);

                                switch (type.ToLower())
                                {
                                    case "int":
                                        ms.WriteInt(string.IsNullOrEmpty(value) ? 0 : int.Parse(value));
                                        break;
                                    case "long":
                                        ms.WriteLong(string.IsNullOrEmpty(value) ? 0 : long.Parse(value));
                                        break;
                                    case "short":
                                        ms.WriteShort(string.IsNullOrEmpty(value) ? (short)0 : short.Parse(value));
                                        break;
                                    case "float":
                                        ms.WriteFloat(string.IsNullOrEmpty(value) ? 0 : float.Parse(value));
                                        break;
                                    case "byte":
                                        ms.WriteByte(string.IsNullOrEmpty(value) ? (byte)0 : byte.Parse(value));
                                        break;
                                    case "bool":
                                        ms.WriteBool((string.IsNullOrEmpty(value) || value == "假") ? false : (value == "真" ? true : bool.Parse(value)));
                                        break;
                                    case "double":
                                        ms.WriteDouble(string.IsNullOrEmpty(value) ? 0 : double.Parse(value));
                                        break;
                                    case "int[]":
                                        ms.WriteIntArr(value);
                                        break;
                                    case "0":
                                        break;
                                    default:
                                        ms.WriteUTF8String(value);
                                        break;

                                }
                            }
                        }
                    }
                    buffer = ms.ToArray();
                }

                //------------------
                //写入文件
                //------------------
                {
                    FileStream fs = new FileStream(string.Format("{0}\\{1}", OutBytesFilePath, fileName + ".bytes"), FileMode.Create);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();

                    Console.WriteLine("客户端表格=>" + fileName + " 生成bytes文件完毕");
                }


                {
                    FileStream fs = new FileStream(string.Format("{0}\\{1}", OutBytesFilePath_Server, fileName + ".bytes"), FileMode.Create);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();

                    Console.WriteLine("服务器端表格=>" + fileName + " 生成bytes文件完毕");
                }

                CreateEntity(fileName, tableHeadArr);
                Console.WriteLine("客户端表格=>" + fileName + " 生成实体脚本完毕");


                CreateServerEntity(fileName, tableHeadArr);
                Console.WriteLine("服务器表格=>" + fileName + " 生成实体脚本完毕");


                CreateDBModel(fileName, tableHeadArr);
                Console.WriteLine("客户端表格=>" + fileName + " 生成数据访问脚本完毕");


                CreateServerDBModel(fileName, tableHeadArr);
                Console.WriteLine("服务器表格=>" + fileName + " 生成数据访问脚本完毕");


                if (OutConfigFileList.ContainsKey(fileName))
                {
                    CreateCSFileId(fileName,tableHeadArr);
                    Console.WriteLine("表格=>" + fileName + " 生成UIID类完成");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("表格=>" + fileName + " 处理失败:" + ex.Message);
            }
        }

        /// <summary>
        /// 创建客户端实体
        /// </summary>
        private static void CreateEntity(string fileName, string[,] dataArr)
        {
            if (dataArr == null)
                return;

            StringBuilder sbr = new StringBuilder();
            sbr.Append("\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("//作    者：边涯  http://www.u3dol.com\r\n");
            sbr.Append("//备    注：此代码为工具生成 请勿手工修改\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("using System.Collections;\r\n");
            sbr.Append("using YouYou;\r\n");
            sbr.Append("\r\n");
            sbr.Append("/// <summary>\r\n");
            sbr.AppendFormat("/// {0}实体\r\n", fileName);
            sbr.Append("/// </summary>\r\n");
            sbr.AppendFormat("public partial class {0}Entity : DataTableEntityBase\r\n", fileName);
            sbr.Append("{\r\n");

            for (int i = 0; i < dataArr.GetLength(0); i++)
            {
                if (i == 0 || dataArr[i, 1] == "0")
                    continue;
                sbr.Append("    /// <summary>\r\n");
                sbr.AppendFormat("    /// {0}\r\n", dataArr[i, 2]);
                sbr.Append("    /// </summary>\r\n");
                sbr.AppendFormat("    public {0} {1};\r\n", dataArr[i, 1], dataArr[i, 0]);
                sbr.Append("\r\n");
            }

            sbr.Append("}\r\n");


            using (FileStream fs = new FileStream(string.Format("{0}/{1}Entity.cs", OutCSharpFilePath, fileName), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }

        /// <summary>
        /// 创建服务器端实体
        /// </summary>
        private static void CreateServerEntity(string fileName, string[,] dataArr)
        {
            if (dataArr == null)
                return;

            StringBuilder sbr = new StringBuilder();
            sbr.Append("//===================================================\r\n");
            sbr.Append("//备    注：此代码为工具生成 请勿手工修改\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("\r\n");
            sbr.Append("namespace YouYouServer.Model.DataTable\r\n");
            sbr.Append("{\r\n");
            sbr.Append("    /// <summary>\r\n");
            sbr.AppendFormat("    /// {0}实体\r\n", fileName);
            sbr.Append("    /// </summary>\r\n");
            sbr.AppendFormat("    public partial class {0}Entity : DataTableEntityBase\r\n", fileName);
            sbr.Append("    {\r\n");

            for (int i = 0; i < dataArr.GetLength(0); i++)
            {
                if (i == 0 || dataArr[i, 1] == "0")
                    continue;
                sbr.Append("        /// <summary>\r\n");
                sbr.AppendFormat("        /// {0}\r\n", dataArr[i, 2]);
                sbr.Append("        /// </summary>\r\n");
                sbr.AppendFormat("        public {0} {1};\r\n", dataArr[i, 1], dataArr[i, 0]);
                sbr.Append("\r\n");
            }

            sbr.Append("    }\r\n");
            sbr.Append("}");

            using (FileStream fs = new FileStream(string.Format("{0}/{1}Entity.cs", OutCSharpFilePath_Server, fileName), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }

        /// <summary>
        /// 创建客户端数据管理类
        /// </summary>
        private static void CreateDBModel(string fileName, string[,] dataArr)
        {
            if (dataArr == null)
                return;

            StringBuilder sbr = new StringBuilder();
            sbr.Append("\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("//作    者：边涯  http://www.u3dol.com\r\n");
            sbr.Append("//备    注：此代码为工具生成 请勿手工修改\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("using System.Collections;\r\n");
            sbr.Append("using System.Collections.Generic;\r\n");
            sbr.Append("using System;\r\n");
            sbr.Append("using YouYou;\r\n");
            sbr.Append("\r\n");
            sbr.Append("/// <summary>\r\n");
            sbr.AppendFormat("/// {0}数据管理\r\n", fileName);
            sbr.Append("/// </summary>\r\n");
            sbr.AppendFormat("public partial class {0}DBModel : DataTableDBModelBase<{0}DBModel, {0}Entity>\r\n", fileName);
            sbr.Append("{\r\n");

            sbr.Append("    /// <summary>\r\n");
            sbr.Append("    /// 文件名称\r\n");
            sbr.Append("    /// </summary>\r\n");
            sbr.AppendFormat("    public override string DataTableName {{ get {{ return \"{0}\"; }} }}\r\n", fileName);
            sbr.Append("\r\n");


            sbr.Append("    /// <summary>\r\n");
            sbr.Append("    /// 加载列表\r\n");
            sbr.Append("    /// </summary>\r\n");
            sbr.Append("    protected override void LoadList(MMO_MemoryStream ms)\r\n");
            sbr.Append("    {\r\n");
            sbr.Append("        int rows = ms.ReadInt();\r\n");
            sbr.Append("        int columns = ms.ReadInt();\r\n");
            sbr.Append("\r\n");
            sbr.Append("        for (int i = 0; i < rows; i++)\r\n");
            sbr.Append("        {\r\n");
            sbr.AppendFormat("            {0}Entity entity = new {0}Entity();\r\n", fileName);

            for (int i = 0; i < dataArr.GetLength(0); i++)
            {
                if ("0" == dataArr[i, 1])
                {
                    continue;
                }
                if (dataArr[i, 1].Equals("byte", StringComparison.CurrentCultureIgnoreCase))
                {
                    sbr.AppendFormat("            entity.{0} = (byte)ms.Read{1}();\r\n", dataArr[i, 0], ChangeTypeName(dataArr[i, 1]));
                }
                else
                {
                    sbr.AppendFormat("            entity.{0} = ms.Read{1}();\r\n", dataArr[i, 0], ChangeTypeName(dataArr[i, 1]));
                }
            }

            sbr.Append("\r\n");
            sbr.Append("            m_List.Add(entity);\r\n");
            sbr.Append("            m_Dic[entity.Id] = entity;\r\n");
            sbr.Append("        }\r\n");
            sbr.Append("    }\r\n");

            sbr.Append("}");
            using (FileStream fs = new FileStream(string.Format("{0}/{1}DBModel.cs", OutCSharpFilePath, fileName), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }

        /// <summary>
        /// 创建服务器端数据管理类
        /// </summary>
        private static void CreateServerDBModel(string fileName, string[,] dataArr)
        {
            if (dataArr == null)
                return;

            StringBuilder sbr = new StringBuilder();
            sbr.Append("//===================================================\r\n");
            sbr.Append("//备    注：此代码为工具生成 请勿手工修改\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("using YouYouServer.Common;\r\n");
            sbr.Append("\r\n");
            sbr.Append("namespace YouYouServer.Model.DataTable\r\n");
            sbr.Append("{\r\n");
            sbr.Append("    /// <summary>\r\n");
            sbr.AppendFormat("    /// {0}数据管理\r\n", fileName);
            sbr.Append("    /// </summary>\r\n");
            sbr.AppendFormat("    public partial class {0}DBModel : DataTableDBModelBase<{0}DBModel, {0}Entity>\r\n", fileName);
            sbr.Append("    {\r\n");

            sbr.Append("        /// <summary>\r\n");
            sbr.Append("        /// 数据表完整路径\r\n");
            sbr.Append("        /// </summary>\r\n");
            sbr.AppendFormat("        public override string DataTableName => \"{0}\";\r\n", fileName);
            sbr.Append("\r\n");


            sbr.Append("        /// <summary>\r\n");
            sbr.Append("        /// 加载列表\r\n");
            sbr.Append("        /// </summary>\r\n");
            sbr.Append("        protected override void LoadList(MMO_MemoryStream ms)\r\n");
            sbr.Append("        {\r\n");
            sbr.Append("            int rows = ms.ReadInt();\r\n");
            sbr.Append("            int columns = ms.ReadInt();\r\n");
            sbr.Append("\r\n");
            sbr.Append("            for (int i = 0; i < rows; i++)\r\n");
            sbr.Append("            {\r\n");
            sbr.AppendFormat("                {0}Entity entity = new {0}Entity();\r\n", fileName);

            for (int i = 0; i < dataArr.GetLength(0); i++)
            {
                if ("0" == dataArr[i, 1])
                {
                    continue;
                }
                if (dataArr[i, 1].Equals("byte", StringComparison.CurrentCultureIgnoreCase))
                {
                    sbr.AppendFormat("                entity.{0} = (byte)ms.Read{1}();\r\n", dataArr[i, 0], ChangeTypeName(dataArr[i, 1]));
                }
                else
                {
                    sbr.AppendFormat("                entity.{0} = ms.Read{1}();\r\n", dataArr[i, 0], ChangeTypeName(dataArr[i, 1]));
                }
            }

            sbr.Append("\r\n");
            sbr.Append("                m_List.Add(entity);\r\n");
            sbr.Append("                m_Dic[entity.Id] = entity;\r\n");
            sbr.Append("            }\r\n");
            sbr.Append("        }\r\n");

            sbr.Append("    }\r\n");
            sbr.Append("}");
            using (FileStream fs = new FileStream(string.Format("{0}/{1}DBModel.cs", OutCSharpFilePath_Server, fileName), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }

        private static string ChangeTypeName(string type)
        {
            string str = string.Empty;

            switch (type)
            {
                case "byte":
                    str = "Byte";
                    break;
                case "int":
                    str = "Int";
                    break;
                case "short":
                    str = "Short";
                    break;
                case "long":
                    str = "Long";
                    break;
                case "float":
                    str = "Float";
                    break;
                case "string":
                    str = "UTF8String";
                    break;
                case "bool":
                    str = "Bool";
                    break;
                case "int[]":
                    str = "IntArr";
                    break;
            }

            return str;
        }
        #endregion

        #region 创建多语言表
        private static void CreateLocalization(string fileName, DataTable dt)
        {
            try
            {
                int rows = dt.Rows.Count;
                int columns = dt.Columns.Count;

                int newcolumns = columns - 3; //减去前三列 后面表示有多少种语言

                int currKeyColumn = 2; //当前的Key列
                int currValueColumn = 3; //当前的值列

                tableHeadArr = new string[columns, 3];

                while (newcolumns > 0)
                {
                    newcolumns--;

                    #region 写入文件
                    byte[] buffer = null;

                    using (MMO_MemoryStream ms = new MMO_MemoryStream())
                    {
                        ms.WriteInt(rows - 3); //减去表头的三行
                        ms.WriteInt(2); //多语言表 只有2列 Key Value

                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                if (i < 3)
                                {
                                    tableHeadArr[j, i] = dt.Rows[i][j].ToString().Trim();
                                }
                                else
                                {
                                    if (j == currKeyColumn)
                                    {
                                        //写入key
                                        string value = dt.Rows[i][j].ToString().Trim();
                                        ms.WriteUTF8String(value);
                                    }
                                    else if (j == currValueColumn)
                                    {
                                        //写入value
                                        string value = dt.Rows[i][j].ToString().Trim();
                                        ms.WriteUTF8String(value);
                                    }
                                }
                            }
                        }
                        buffer = ms.ToArray();
                    }

                    //------------------
                    //写入文件
                    //------------------
                    FileStream fs = new FileStream(string.Format("{0}/Localization/{1}", OutBytesFilePath, tableHeadArr[currValueColumn, 0] + ".bytes"), FileMode.Create);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();

                    currValueColumn++;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("表格=>" + fileName + " 处理失败:" + ex.Message);
            }
        }
        #endregion

        private static void CreateCSFileId(string fileName, string[,] dataArr)
        {
            if (dataArr == null)
                return;
            var csFileName =  CheckCSFileName(fileName);
            StringBuilder sbr = new StringBuilder();
            sbr.Append("\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("//作    者：ZnArycs \r\n");
            sbr.Append($"//备    注：此代码基于{fileName}生成对应ID\r\n");
            sbr.Append("//===================================================\r\n");
            sbr.Append("\r\n");
            sbr.Append("/// <summary>\r\n");
            sbr.AppendFormat("/// UI界面ID \r\n");
            sbr.Append("/// </summary>\r\n");
            sbr.AppendFormat($"public static class {csFileName}\r\n");
            sbr.Append("{\r\n");

            for (int i = 3; i < dataArr.GetLength(1); i++)
            {
                sbr.Append("    /// <summary>\r\n");
                sbr.AppendFormat("    ///{0} \r\n", dataArr[1, i]);
                sbr.Append("    /// </summary>\r\n");
                sbr.AppendFormat("    public const int {0} = {1};\r\n", dataArr[2, i], dataArr[0, i]);
                sbr.Append("\r\n");
            }
            sbr.Append("}");
            using (FileStream fs = new FileStream(string.Format("{0}/{1}.cs", OutConfigIdPath_Server, csFileName), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
            using (FileStream fs = new FileStream(string.Format("{0}/{1}.cs", OutConfigIdPath_Client, csFileName), FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(sbr.ToString());
                }
            }
        }


        public static string CheckCSFileName(string name)
        {
            string res = "";
            res = name.Replace("DT", "");
            res = res.Replace('_', ' ').Replace(" ","");
            if (name.EndsWith("Job") || name.EndsWith("UIForm") || name.EndsWith("Prefab"))
            {
                res += "Id";
            }
            return res;
        }
    }
}