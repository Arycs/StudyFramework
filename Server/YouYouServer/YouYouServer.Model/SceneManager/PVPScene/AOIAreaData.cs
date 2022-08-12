using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace YouYouServer.Model.ServerManager
{
    [Serializable]
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


        public Vector3 TopLeftPos;
        public Vector3 BottomRightPos;

        public void Init()
        {
            TopLeftPos = new Vector3((float)TopLeftPos_X, (float)TopLeftPos_Y, (float)TopLeftPos_Z);
            BottomRightPos = new Vector3((float)BottomRightPos_X, (float)BottomRightPos_Y, (float)BottomRightPos_Z);
        }
    }
}
