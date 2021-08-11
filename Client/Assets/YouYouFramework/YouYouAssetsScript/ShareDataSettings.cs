using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu]
public class ShareDataSettings : ScriptableObject
{
    public enum ShareDataModule
    {
        ShareUserData,
        ShareOtherData
    }

    public enum ShareDataType
    {
        Int,
        Long,
        Double
    }

    [EnumToggleButtons]
    public ShareDataModule Modules;

    [ShowIf("Modules", ShareDataModule.ShareUserData)]
    public ShareData ShareUserData;

    [ShowIf("Modules", ShareDataModule.ShareOtherData)]
    public ShareData ShareOtherrData;

    /// <summary>
    /// ��������
    /// </summary>
    [Serializable]
    public class ShareData
    {
        public string ClassName;

        public string ManagerName;

        /// <summary>
        /// CSharp·��
        /// </summary>
        [FolderPath(ParentFolder = "Assets")]
        public string CSharpScriptPath;

        /// <summary>
        /// Lua·��
        /// </summary>
        [FolderPath(ParentFolder = "Assets")]
        public string LuaScriptPath;

        public ShareDataField[] Fields;

        [Button(ButtonSizes.Medium)]
        [LabelText("�������ݽű�")]
        public void CreateShareDataScript()
        {
            #region ����c#�ű�
            StringBuilder sbrCSharp = new StringBuilder();
            sbrCSharp.AppendFormat("using System;\r\n");
            sbrCSharp.AppendFormat("\r\n");
            sbrCSharp.AppendFormat("public class {0} : IDisposable\r\n", ClassName);
            sbrCSharp.Append("{\r\n");
            sbrCSharp.AppendFormat("    private LuaArrAccess m_CurrArrAccess;\r\n");
            sbrCSharp.AppendFormat("\r\n");
            sbrCSharp.AppendFormat("    /// <summary>\r\n");
            sbrCSharp.AppendFormat("    /// ��ʼ��c#������\r\n");
            sbrCSharp.AppendFormat("    /// </summary>\r\n");
            sbrCSharp.AppendFormat("    /// <param name=\"arrAccess\"></param>\r\n");
            sbrCSharp.AppendFormat("    public void InitLuaArrAccess(LuaArrAccess arrAccess)\r\n");
            sbrCSharp.Append("    {\r\n");
            sbrCSharp.AppendFormat("        m_CurrArrAccess = arrAccess;\r\n");
            sbrCSharp.Append("    }\r\n");
            sbrCSharp.AppendFormat("\r\n");

            int index = 0;
            foreach (ShareDataField shareDataField in Fields)
            {
                index++;
                sbrCSharp.AppendFormat("    /// <summary>\r\n");
                sbrCSharp.AppendFormat("    /// {0}\r\n", shareDataField.FieldDesc);
                sbrCSharp.AppendFormat("    /// </summary>\r\n");
                sbrCSharp.AppendFormat("    public {0} {1} {{ get {{ return m_CurrArrAccess.Get{3}({2}); }} set {{ m_CurrArrAccess.Set{3}({2}, value); }} }}\r\n",
                    shareDataField.Type.ToString().ToLower(), shareDataField.FieldName, index, shareDataField.Type.ToString()
                    );
                sbrCSharp.AppendFormat("\r\n");
            }
            sbrCSharp.AppendFormat("    public void Dispose()\r\n");
            sbrCSharp.Append("    {\r\n");

            foreach (ShareDataField shareDataField in Fields)
            {
                sbrCSharp.AppendFormat("        {0} = 0;\r\n", shareDataField.FieldName);
            }
            sbrCSharp.Append("    }\r\n");
            sbrCSharp.Append("}");

            IOUtil.CreateTextFile(Application.dataPath + "/" + CSharpScriptPath + "/" + ClassName + ".cs", sbrCSharp.ToString());
            #endregion

            #region ����lua�ű�
            StringBuilder sbrLua = new StringBuilder();
            sbrLua.AppendFormat("{0} = {{ }}\r\n", ClassName);
            sbrLua.AppendFormat("local this = {0}\r\n", ClassName);
            sbrLua.AppendFormat("\r\n");
            sbrLua.AppendFormat("local dataArr;\r\n");
            sbrLua.AppendFormat("\r\n");
            sbrLua.AppendFormat("function {0}.Init()\r\n", ClassName);
            sbrLua.AppendFormat("    dataArr = GameInit.CreateLuaCSharpArr({0});\r\n", Fields.Length);
            sbrLua.AppendFormat("    local CSharpAccess = dataArr:GetCSharpAccess()\r\n");
            sbrLua.AppendFormat("    GameEntry.Data.{0}.{1}:InitLuaArrAccess(CSharpAccess);\r\n", ManagerName, ClassName);
            sbrLua.AppendFormat("end\r\n");
            sbrLua.AppendFormat("\r\n");
            index = 0;
            foreach (ShareDataField shareDataField in Fields)
            {
                index++;
                sbrLua.AppendFormat("--��ȡ{0}\r\n", shareDataField.FieldDesc);
                sbrLua.AppendFormat("function {0}.Get{1}()\r\n", ClassName, shareDataField.FieldName);
                sbrLua.AppendFormat("    return dataArr[{0}];\r\n", index);
                sbrLua.AppendFormat("end\r\n");

                sbrLua.AppendFormat("--����{0}\r\n", shareDataField.FieldDesc);
                sbrLua.AppendFormat("function {0}.Set{1}(value)\r\n", ClassName, shareDataField.FieldName);
                sbrLua.AppendFormat("    dataArr[{0}] = value;\r\n", index);
                sbrLua.AppendFormat("end");

                if (index < Fields.Length)
                {
                    sbrLua.AppendFormat("\r\n");
                    sbrLua.AppendFormat("\r\n");
                }
            }

            IOUtil.CreateTextFile(Application.dataPath + "/" + LuaScriptPath + "/" + ClassName + ".bytes", sbrLua.ToString());
            #endregion
            Debug.Log("����=" + ClassName + "���");
        }
    }

    [Serializable]
    /// <summary>
    /// ���������ֶ�
    /// </summary>
    public class ShareDataField
    {
        public ShareDataType Type;
        public string FieldDesc;
        public string FieldName;
    }
}
