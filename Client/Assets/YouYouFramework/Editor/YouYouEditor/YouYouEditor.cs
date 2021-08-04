using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class YouYouEditor : OdinMenuEditorWindow
{
    [MenuItem("YouYouTools/YouYouEditor")]
    private static void OpenYouYouEditor()
    {
        var window = GetWindow<YouYouEditor>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.AddAssetAtPath("YouYouFramework", "YouYouFramework/YouYouAssets/AboutUs.asset").AddIcon(EditorIcons.Airplane);
        tree.AddAssetAtPath("MacroSettings", "YouYouFramework/YouYouAssets/MacroSettings.asset").AddIcon(EditorIcons.Airplane);
        tree.AddAssetAtPath("ParamsSettings", "YouYouFramework/YouYouAssets/ParamsSettings.asset").AddIcon(EditorIcons.Airplane);
        tree.AddAssetAtPath("AssetBundleSettings", "YouYouFramework/YouYouAssets/AssetBundleSettings.asset").AddIcon(EditorIcons.AlertCircle);
        return tree;
    }
}
