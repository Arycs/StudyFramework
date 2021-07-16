//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-03-14 22:50:55
//备    注：
//===================================================

using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using YouYou;

public class Menu
{
    [MenuItem("悠游工具/设置")]
    public static void Settings()
    {
        SettingsWindow win = (SettingsWindow)EditorWindow.GetWindow(typeof(SettingsWindow));
        win.titleContent = new GUIContent("全局设置");
        win.Show();
    }

    [MenuItem("悠游工具/资源管理/资源打包管理")]
    public static void AssetBundleCreate()
    {
        AssetBundleWindow win = EditorWindow.GetWindow<AssetBundleWindow>();
        win.titleContent = new GUIContent("资源打包");
        win.Show();
    }

    [MenuItem("悠游工具/资源管理/初始资源拷贝到StreamingAsstes")]
    public static void AssetBundleCopyToStreamingAsstes()
    {
        string toPath = Application.streamingAssetsPath + "/AssetBundles/";

        if (Directory.Exists(toPath))
        {
            Directory.Delete(toPath, true);
        }
        Directory.CreateDirectory(toPath);
        
        IOUtil.CopyDirectory(Application.persistentDataPath, toPath);

        //重新生成版本文件
        //1. 先读取persistentDataPath里面的版本文件 这个版本文件里 存放了所有的资源包信息
        byte[] buffer = IOUtil.GetFileBuffer(Application.persistentDataPath + "/VersionFile.bytes");
        string version = "";
        Dictionary<string, AssetBundleInfoEntity> dic = ResourceManager.GetAssetBundleVersionList(buffer, ref version);
        Dictionary<string ,AssetBundleInfoEntity> newDic = new Dictionary<string, AssetBundleInfoEntity>();
        
        DirectoryInfo directory = new DirectoryInfo(toPath);
        
        //拿到文件夹下所有文件
        FileInfo[] arrFiles = directory.GetFiles("*", SearchOption.AllDirectories);

        for (int i = 0; i < arrFiles.Length; i++)
        {
            FileInfo file = arrFiles[i];
            string fullName = file.FullName.Replace("\\", "/"); //全名 包含路径扩展名
            string name = fullName.Replace(toPath, "").Replace(".assetbundle", "").Replace(".unity3d", "");

            if (name.Equals("AssetInfo.json",StringComparison.CurrentCultureIgnoreCase) ||
                name.Equals("Windows",StringComparison.CurrentCultureIgnoreCase) ||
                name.Equals("Windows.manifest",StringComparison.CurrentCultureIgnoreCase))
            {
                File.Delete(file.FullName);
                continue;
            }
            
            
            AssetBundleInfoEntity entity = null;
            dic.TryGetValue(name, out entity);
            if (entity != null)
            {
                newDic[name] = entity;
            }
        }
        StringBuilder sbContent = new StringBuilder();
        sbContent.AppendLine(version);
        foreach (var item in newDic)
        {
            AssetBundleInfoEntity entity = item.Value;
            string strLine = string.Format("{0}|{1}|{2}|{3}|{4}", entity.AssetBundleName, entity.MD5, entity.Size,
                entity.IsFirstData, entity.IsEncrypt);
            sbContent.AppendLine(strLine);
        }
        IOUtil.CreateTextFile(toPath + "VersionFile.txt",sbContent.ToString());
        
        //===============
        MMO_MemoryStream ms = new MMO_MemoryStream();
        string str = sbContent.ToString().Trim();
        string[] arr = str.Split('\n');
        int len = arr.Length;
        ms.WriteInt(len);
        for (int i = 0; i < len; i++)
        {
            if (i==0)
            {
                ms.WriteUTF8String(arr[i]);
            }
            else
            {
                string[] arrInner = arr[i].Split('|');
                ms.WriteUTF8String(arrInner[0]);
                ms.WriteUTF8String(arrInner[1]);
                ms.WriteInt(int.Parse(arrInner[2]));
                ms.WriteByte(byte.Parse(arrInner[3]));
                ms.WriteByte(byte.Parse(arrInner[4]));
            }
        }

        string filePath = toPath + "/VersionFile.bytes"; //版本文件路径
        buffer = ms.ToArray();
        buffer = ZlibHelper.CompressBytes(buffer);
        FileStream fs = new FileStream(filePath,FileMode.Create);
        fs.Write(buffer,0,buffer.Length);
        fs.Close();
        
        AssetDatabase.Refresh();
        Debug.Log("初始资源拷贝到StreamingAssets完毕");
    }

    [MenuItem("悠游工具/资源管理/打开persistentDataPath")]
    public static void AssetBundleOpenPersistentDataPath()
    {
        string output = Application.persistentDataPath;
        if (!Directory.Exists(output))
        {
            Directory.CreateDirectory(output);
        }

        output = output.Replace("/", "\\");
        System.Diagnostics.Process.Start("explorer.exe", output);
    }

    [MenuItem("悠游工具/资源管理/清空本地AssetBundle")]
    public static void ClearLocalAssetBundle()
    {
        Directory.Delete(Application.persistentDataPath, true);
    }
    
    #region CreateLuaScript 生成Lua脚本
    [MenuItem("悠游工具/生成Lua脚本")]
    public static void CreateLuaView()
    {
        if (Selection.transforms.Length == 0)
        {
            return;
        }

        Transform trans = Selection.transforms[0];

        LuaForm luaForm = trans.GetComponent<LuaForm>();
        if (luaForm == null)
        {
            Debug.LogError("该UI上没有LuaForm脚本");
            return;
        }

        string viewName = trans.gameObject.name;

        LuaCom[] luaComs = luaForm.LuaComs;
        int len = luaComs.Length;
        
        StringBuilder sbrView = new StringBuilder();
        StringBuilder sbrCtrl = new StringBuilder();
        sbrView.AppendFormat("");
        sbrView.AppendFormat("{0}View = {{}};\n", viewName);
        sbrView.AppendFormat("local this = {0}View;\n", viewName);
        sbrView.AppendFormat("\n");
        for (int i = 0; i < len; i++)
        {
            LuaCom com = luaComs[i];
            sbrView.AppendFormat("local {0}Index = {1};\n", com.Name, i);
        }

        sbrView.AppendFormat("\n");
        sbrView.AppendFormat("function {0}View.OnInit(transform,userData)\n", viewName);
        sbrView.AppendFormat("    this.InitView(transform);\n");
        sbrView.AppendFormat("    {0}Ctrl.OnInit(userData);\n", viewName);
        sbrView.AppendFormat("end\n");
        sbrView.AppendFormat("\n");
        sbrView.AppendFormat("function {0}View.InitView(transform)\n", viewName);
        sbrView.AppendFormat("    this.LuaForm = transform:GetComponent(typeof(CS.YouYou.LuaForm));\n");
        for (int i = 0; i < len; i++)
        {
            LuaCom com = luaComs[i];
            sbrView.AppendFormat("    this.{0} = this.LuaForm:GetLuaComs({0}Index);\n", com.Name);
        }
        sbrView.AppendFormat("end\n");
        sbrView.AppendFormat("\n");
        sbrView.AppendFormat("function {0}View.OnOpen(userData)\n", viewName);
        sbrView.AppendFormat("    {0}Ctrl.OnOpen(userData);\n", viewName);
        sbrView.AppendFormat("end\n");
        sbrView.AppendFormat("\n");
        sbrView.AppendFormat("function {0}View.OnClose()\n", viewName);
        sbrView.AppendFormat("    {0}Ctrl.OnClose();\n", viewName);
        sbrView.AppendFormat("end\n");
        sbrView.AppendFormat("\n");
        sbrView.AppendFormat("function {0}View.OnBeforDestroy()\n", viewName);
        sbrView.AppendFormat("    {0}Ctrl.OnBeforDestroy();\n", viewName);
        sbrView.AppendFormat("    this.LuaForm = nil;\n", viewName);
        for (int i = 0; i < len; i++)
        {
            LuaCom com = luaComs[i];
            sbrView.AppendFormat("    this.{0} = nil;\n", com.Name);
        }
        sbrView.AppendFormat("end");


        sbrCtrl.AppendFormat("{0}Ctrl = {{}};\n",viewName);
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("local this = {0}Ctrl;\n", viewName);
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("function {0}Ctrl.OnInit(userData)\n", viewName);
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("end\n");
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("function {0}Ctrl.OnOpen(userData)\n", viewName);
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("end\n");
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("function {0}Ctrl.OnClose()\n", viewName);
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("end\n");
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("function {0}Ctrl.OnBeforDestroy()\n", viewName);
        sbrCtrl.AppendFormat("\n");
        sbrCtrl.AppendFormat("end\n");
        
        
        string pathView = Application.dataPath + "/Download/xLuaLogic/Modules/Temp/" + viewName + "View.bytes";
        string pathCtrl = Application.dataPath + "/Download/xLuaLogic/Modules/Temp/" + viewName + "Ctrl.bytes";

        using (FileStream fs = new FileStream(pathView, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbrView.ToString());
            }
        }
        using (FileStream fs = new FileStream(pathCtrl, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbrCtrl.ToString());
            }
        }
        AssetDatabase.Refresh();
    }
    #endregion

    
}