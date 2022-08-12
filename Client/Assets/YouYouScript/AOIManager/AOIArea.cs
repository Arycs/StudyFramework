using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOIArea : MonoBehaviour
{
    /// <summary>
    /// 区域编号
    /// </summary>
    public int AreaId;

    /// <summary>
    /// 当前行
    /// </summary>
    public int CurrRow;

    /// <summary>
    /// 当前列
    /// </summary>
    public int CurrColumn;

    /// <summary>
    /// 关联的区域列表, 不包括自己 
    /// </summary>
    public List<AOIArea> ConnectAreaList = new List<AOIArea>();

    /// <summary>
    /// 设置关联区域
    /// </summary>
    public void SetConnectArea()
    {
        ConnectAreaList.Clear();

        //根据区域编号 获取这个区域关联的区域
        //获取这个点,关联的九宫格  左上角行列和右下角行列
        int minRow = CurrRow - 1;
        int minColumn = CurrColumn - 1;
        int maxRow = CurrRow + 1;
        int maxColumn = CurrColumn + 1;

        foreach (var item in AOIManager.AOIAreaDic)
        {
            AOIArea aoiArea = item.Value;

            //排除自己
            if (aoiArea.AreaId == AreaId)
            {
                continue;
            }

            if (aoiArea.CurrRow >= minRow &&
                aoiArea.CurrRow <= maxRow && 
                aoiArea.CurrColumn >= minColumn &&
                aoiArea.CurrColumn <= maxColumn)
            {
                ConnectAreaList.Add(aoiArea);
            }
        }
    }
}