using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AOIManager : MonoBehaviour
{
    /// <summary>
    /// 区域预设
    /// </summary>
    public GameObject AreaPrefab;

    /// <summary>
    /// 区域宽度
    /// </summary>
    public int AreaWidth;

    /// <summary>
    /// 行数
    /// </summary>
    public int Rows;

    /// <summary>
    /// 列数
    /// </summary>
    public int Columns;

    /// <summary>
    /// 区域字典
    /// </summary>
    public static Dictionary<int, AOIArea> AOIAreaDic = new Dictionary<int, AOIArea>();

    [Button("Create AOI Area")]
    public void CreateArea()
    {
        //先删除 子物体
        Transform[] trans = transform.GetComponentsInChildren<Transform>();

        foreach (var item in trans)
        {
            if (item.gameObject.GetInstanceID() != transform.gameObject.GetInstanceID()
            && item.gameObject.GetInstanceID() != AreaPrefab.GetInstanceID())
            {
                DestroyImmediate(item.gameObject);
            }
        }
        AOIAreaDic.Clear();
        
        //重新生成
        int num = 1;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                GameObject areaObj = Instantiate(AreaPrefab, transform, true);
                areaObj.transform.localScale = Vector3.one * AreaWidth;
                areaObj.transform.localPosition = new Vector3(j * AreaWidth, 0, i * AreaWidth * -1);
                areaObj.SetActive(true);
                areaObj.name = "area_" + num.ToString();

                AOIArea aoiArea = areaObj.GetComponent<AOIArea>();
                aoiArea.AreaId = num;
                aoiArea.CurrRow = i + 1;
                aoiArea.CurrColumn = j + 1;

                AOIAreaDic[aoiArea.AreaId] = aoiArea;
                num++;
            }
        }
        
        //计算关联
        foreach (var item in AOIAreaDic)
        {
            item.Value.SetConnectArea();
        }
    }

    [Button("Create AOI Json Data")]
    private void CreateAOIAreaData()
    {
        string path = $"{Application.dataPath}/SceneAOIJsonData/{SceneManager.GetActiveScene().name}.json";

        List<AOIAreaData> lst = new List<AOIAreaData>();

        foreach (var item in AOIAreaDic)
        {
            lst.Add(item.Value.CreateAOIAreaData());
        }

        string json = LitJson.JsonMapper.ToJson(lst);
        IOUtil.CreateTextFile(path,json);
        AssetDatabase.Refresh();
        
        Debug.Log("Create Scene AOI Area Data Successful");
    }
    

}
