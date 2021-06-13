using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YouYou
{
    [CustomEditor(typeof(PoolComponent),true)]
    public class PoolComponentInspector : Editor
    {
        /// <summary>
        /// 释放间隔  属性
        /// </summary>
        private SerializedProperty ReleaseClassObjectInterval = null;
        
        /// <summary>
        /// 释放间隔  属性
        /// </summary>
        private SerializedProperty m_GameObjectPoolGroups = null;

        /// <summary>
        /// 释放资源池间隔  属性
        /// </summary>
        private SerializedProperty ReleaseResourceInterval = null;

        /// <summary>
        /// 是否显示资源分类池
        /// </summary>
        private SerializedProperty ShowAssetPool = null;
        public override void OnInspectorGUI()
        {
            //在该方法中对对应属性都进行了重新显示 
            //这里如果在调用基类的OnInspectorGUI则会导致出现 重复绘制,比如PoolComponent中的数组,Public变量即会被多次显示
            //base.OnInspectorGUI();
            
            serializedObject.Update();
            PoolComponent component = base.target as PoolComponent;
            
            //绘制滑动条
            int clearInterval = (int)EditorGUILayout.Slider("释放类对象池间隔", ReleaseClassObjectInterval.intValue, 10, 1800);
            if (clearInterval != ReleaseClassObjectInterval.intValue)
            {
                component.ReleaseClassObjectInterval = clearInterval;
            }
            else
            {
                ReleaseClassObjectInterval.intValue = clearInterval;
            }
            

            //===============================类对象池开始=========================================
            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            
            GUILayout.Label("类名");
            GUILayout.Label("池中数量",GUILayout.Width(50));
            GUILayout.Label("常驻数量",GUILayout.Width(50));
            
            GUILayout.EndHorizontal();

            if (component != null && component.PoolManager != null) 
            {
                foreach (var item in component.PoolManager.ClassObjectPool.InspectorDic)
                {
                    GUILayout.BeginHorizontal("box");
                
                    GUILayout.Label(item.Key.Name);
                    GUILayout.Label(item.Value.ToString(),GUILayout.Width(50));
                
                    int key = item.Key.GetHashCode();
                    byte resideCount = 0;
                    component.PoolManager.ClassObjectPool.ClassObjectCount.TryGetValue(key, out resideCount);
                    GUILayout.Label(resideCount.ToString(),GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            //==============================类对象池结束==========================================
            
            //==============================变量计数开始==========================================
            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("变量");
            GUILayout.Label("数量",GUILayout.Width(50));
            
            GUILayout.EndHorizontal();

            if (component != null) 
            {
                foreach (var item in component.VarObjectInspectorDic)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(item.Key.Name);
                    GUILayout.Label(item.Value.ToString(),GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            //==============================变量计数结束==========================================
            
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(m_GameObjectPoolGroups,true);
            
            GUILayout.Space(10);
            //绘制滑动条
            int releaseAssetBundleInterval = (int)EditorGUILayout.Slider("释放资源池间隔", ReleaseResourceInterval.intValue, 10, 1800);
            if (releaseAssetBundleInterval != ReleaseResourceInterval.intValue)
            {
                component.ReleaseResourceInterval = releaseAssetBundleInterval;
            }
            else
            {
                ReleaseResourceInterval.intValue = releaseAssetBundleInterval;
            }
            //======================资源包统计开始============================================
            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("资源包");
            GUILayout.Label("数量",GUILayout.Width(50));
            GUILayout.EndHorizontal();

            if (component != null && component.PoolManager != null)
            {
                foreach (var item in component.PoolManager.AssetBundlePool.InspectorDic)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(item.Key);
                    GUILayout.Label(item.Value.ToString(),GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            //======================资源包统计结束===========================================
            
            //======================资源统计开始============================================
            GUILayout.Space(10);
            bool showAssetPool = EditorGUILayout.ToggleLeft("显示分类资源池", ShowAssetPool.boolValue);
            if (showAssetPool != ShowAssetPool.boolValue)
            {
                component.ShowAssetPool = showAssetPool;
            }
            else
            {
                ShowAssetPool.boolValue = showAssetPool;
            }

            if (showAssetPool)
            {
                GUILayout.Space(10);
                var enumerator = Enum.GetValues(typeof(AssetCategory)).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    AssetCategory assetCategory = (AssetCategory) enumerator.Current;
                    if (assetCategory == AssetCategory.None)
                    {
                        continue;;
                    }
                    
                    GUILayout.Space(10);
                    GUILayout.BeginVertical("box");
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label("资源分类-" + assetCategory.ToString());
                    GUILayout.Label("计数",GUILayout.Width(50));
                    GUILayout.EndHorizontal();

                    if (component != null && component.PoolManager != null)
                    {
                        foreach (var item in component.PoolManager.AssetPool[assetCategory].InspectorDic)
                        {
                            GUILayout.BeginHorizontal("box");
                            GUILayout.Label(item.Key);
                            GUILayout.Label(item.Value.ToString(),GUILayout.Width(50));
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            //=======================资源统计结束 
            
            serializedObject.ApplyModifiedProperties();
            //重新绘制
            Repaint();
        }

        private void OnEnable()
        {
            //建立属性关系
            ReleaseClassObjectInterval = serializedObject.FindProperty("ReleaseClassObjectInterval");
            m_GameObjectPoolGroups = serializedObject.FindProperty("m_GameObjectPoolGroups");

            ReleaseResourceInterval = serializedObject.FindProperty("ReleaseResourceInterval");
            ShowAssetPool = serializedObject.FindProperty("ShowAssetPool");
            serializedObject.ApplyModifiedProperties();
        }
    }

}
