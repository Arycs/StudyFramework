using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// AOI区域数据
    /// </summary>
    [System.Serializable]
    public class AOIAreaData
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
        /// 关联区域列表
        /// </summary>
        public List<int> ConnectAreaList;
        
        /// <summary>
        /// 包含自己和关联区域的列表
        /// </summary>
        public  List<int> AllAreaList;

        /// <summary>
        /// 左上角坐标
        /// </summary>
        public double TopLeftPos_X;
        public double TopLeftPos_Y;
        public double TopLeftPos_Z;

        /// <summary>
        /// 右下角坐标
        /// </summary>
        public double BottomRightPos_X;
        public double BottomRightPos_Y;
        public double BottomRightPos_Z;
        
        /// <summary>
        /// 可行走区域单元格宽度
        /// </summary>
        public double CellWith = 0.5f;

        /// <summary>
        /// 可行走区域单元格数据
        /// </summary>
        public List<int> CellData;

        public Vector3 TopLeftPos;
        public Vector3 BottomRightPos;
        
        /// <summary>
        /// 本区域单元格列表
        /// </summary>
        public List<AOIAreaDataCell> areaDataCellList;

        public void Init()
        {
            TopLeftPos = new Vector3((float)TopLeftPos_X, (float)TopLeftPos_Y, (float)TopLeftPos_Z);
            BottomRightPos = new Vector3((float)BottomRightPos_X, (float)BottomRightPos_Y, (float)BottomRightPos_Z);
            areaDataCellList = new List<AOIAreaDataCell>();
            
            AllAreaList = new List<int> {AreaId};
            AllAreaList.AddRange(ConnectAreaList);

            int index = 0;
            foreach (var item in CellData)
            {
                bool canArrive = item == 1;
                AOIAreaDataCell aoiAreaDataCell = new AOIAreaDataCell( AreaId,index++, (float)CellWith, canArrive, 30, TopLeftPos);
                areaDataCellList.Add(aoiAreaDataCell);
            }
        }
        
        /// <summary>
        /// 返回目标点是否可到达
        /// </summary>
        /// <param name="currPos"></param>
        /// <returns></returns>
        public bool GetCanArrive(UnityEngine.Vector3 currPos)
        {
            foreach (var item in areaDataCellList)
            {
                if (currPos.x >= item.TopLeftPos.x && currPos.z <= item.TopLeftPos.z
                                                   && currPos.x <= item.BottomRightPos.x &&
                                                   currPos.z >= item.BottomRightPos.z
                )
                {
                    return item.CanArrive;
                }
            }

            return false;
        }

        public class AOIAreaDataCell
        {
            public Vector3 TopLeftPos;
            public Vector3 BottomRightPos;

            //是否可以到达
            public bool CanArrive { get; private set; }

            public AOIAreaDataCell(int areaId, int index, float cellWith, bool canArrive, float aoiAreaWidth,
                Vector3 topLeftPos)
            {
                CanArrive = canArrive;

                //单元格 单行数量
                int rowCount = (int) (aoiAreaWidth / cellWith);
                int columnCount = rowCount;

                int id = index + 1;
                int currRow = (int) Math.Ceiling(id / (double) columnCount);
                int currColumn = id % columnCount;

                BottomRightPos = topLeftPos + new Vector3(currColumn * cellWith, 0, currRow * -1 * cellWith);
                TopLeftPos = BottomRightPos + new Vector3(-1 * cellWith, 0, cellWith);

                // if (areaId == 30 && id == 1)
                // {
                //     Console.WriteLine("currRow=" +currRow);
                //     Console.WriteLine("AreaId=" + areaId + " id" + id + " CanArrive=" + CanArrive);
                //     Console.WriteLine("BottomRightPos=" + BottomRightPos);
                //     Console.WriteLine("TopLeftPos=" + TopLeftPos);
                //     Console.WriteLine("topLeftPos============" + topLeftPos);
                // }
            }
        }
    }
}
