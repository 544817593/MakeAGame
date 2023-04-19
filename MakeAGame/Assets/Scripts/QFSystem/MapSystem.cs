using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 地图
    /// </summary>
    public interface IMapSystem : ISystem
    {
        /// <summary>
        /// 通过CSV文件初始化
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="mapDataResPath"></param>
        void CreateMapByCSV(int row, int col, string mapDataResPath);
        /// <summary>
        /// 通过SO文件初始化
        /// </summary>
        /// <param name="SOResPath"></param>
        void CreateMapBySO(string SOResPath);
    }

    public class MapSystem: AbstractSystem, IMapSystem
    {
        private BoxGrid[,] mGrids;  // 地图格子数组
        private int mRow => mGrids.GetLength(0);    // 总行数
        private int mCol => mGrids.GetLength(1);    // 总列数

        protected override void OnInit()
        {
            
        }

        #region 通过SO初始化

        public void CreateMapBySO(string SOResPath)
        {
            if (mGrids != null)
            {
                Debug.LogError($"Map already existed! row: {mRow} col: {mCol}");
                return;
            }

            SOMapData data = Resources.Load<SOMapData>(SOResPath);
            if (data == null)
            {
                Debug.LogError("unvalid SO res path!");
                return;
            }

            int row = data.row;
            int col = data.col;
            mGrids = new BoxGrid[row, col];
            
            var gridGenerator = this.GetSystem<IGridGeneratorSystem>();
            Transform root = GameObject.Find("MapRoot").transform;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var newGrid = gridGenerator.CreateGrid(i, j, root);
                    
                    // todo 地图格子真正赋值数据的地方
                    newGrid.terrain.Value = data.mapTerrain[i, j];
                    newGrid.timeMultiplier.Value = data.mapTimeMultiplier[i, j];
                    
                    
                    mGrids[i, j] = newGrid;
                    Debug.Log(mGrids[i, j]);
                }
            }
            
            Debug.Log("CreateMap finished");
        }

        #endregion

        #region 通过CSV初始化

        public void CreateMapByCSV(int row, int col, string mapDataResPath)
        {
            if (mGrids != null)
            {
                Debug.LogError($"Map already existed! row: {mRow} col: {mCol}");
                return;
            }

            mGrids = new BoxGrid[row, col];
            
            var gridGenerator = this.GetSystem<IGridGeneratorSystem>();
            var csvImporter = this.GetUtility<ICSVImportUtility>();
            Dictionary<string, int> attribute_idx = new Dictionary<string, int>();
            List<List<string>> data_list = new List<List<string>>();
            csvImporter.ParseCSV(mapDataResPath, ref attribute_idx, ref data_list);
            
            Transform root = GameObject.Find("MapRoot").transform;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var newGrid = gridGenerator.CreateGrid(i, j, root);
                    SetGridData(newGrid, data_list[row * i + j], attribute_idx);
                    mGrids[i, j] = newGrid;
                    Debug.Log(mGrids[i, j]);
                }
            }
            
            Debug.Log("CreateMap finished");
        }

        /// <summary>
        /// 获取CSV文件数据后，转为属性
        /// </summary>
        /// <param name="boxGrid"></param>
        /// <param name="dataRow"></param>
        /// <param name="attri"></param>
        public void SetGridData(BoxGrid boxGrid, List<string> dataRow, Dictionary<string, int> attri)
        {
            // 默认表内数据按顺序排列
            boxGrid.timeMultiplier.Value = float.Parse(dataRow[attri["timeMultiplier"]]);
            boxGrid.terrain.Value = int.Parse(dataRow[attri["terrain"]]);
            boxGrid.statusType.Value = int.Parse(dataRow[attri["statusType"]]);
        }

        #endregion
    }
}