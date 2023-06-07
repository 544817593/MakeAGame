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
        void CreateMapBySO(SOMapData data);

        /// <summary>
        /// 地图格子数组Getter
        /// </summary>
        /// <returns></returns>
        BoxGrid[,] Grids();

        /// <summary>
        /// 检查格子可否被单位移动到此
        /// </summary>
        /// <param name="boxgrid">需要被检查的格子</param>
        /// <returns></returns>
        bool GridCanMoveTo(BoxGrid boxgrid);

        int GetGridDist(BoxGrid grid1, BoxGrid grid2);

        void Clear();
        void SetNUllMap();

        /// 地图中央位置，用于摄像头设置
        /// </summary>
        BoxGrid centerGrid { get; }

        public int mapRow { get; }
        public int mapCol { get; }
    }

    public class MapSystem : AbstractSystem, IMapSystem
    {
        public BoxGrid[,] mGrids; // 地图格子数组

        public int mapRow => mGrids.GetLength(0);    // 总行数
        public int mapCol => mGrids.GetLength(1);    // 总列数

        protected override void OnInit()
        {
            
        }

        public BoxGrid[,] Grids() { return mGrids; }
        public BoxGrid centerGrid { get; private set; }

        #region 通过SO初始化

        public void CreateMapBySO(string SOResPath)
        {
            if (mGrids != null)
            {
                Debug.LogError($"Map already existed! row: {mapRow} col: {mapCol}");
                return;
            }

            SOMapData data = Resources.Load<SOMapData>(SOResPath);
            if (data == null)
            {
                Debug.LogError("unvalid SO res path!");
                return;
            }

            CreateMapBySO(data);
        }

        public void CreateMapBySO(SOMapData data)
        {
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
                    
                    // 地图格子真正赋值数据的地方
                    newGrid.terrain.Value = data.mapTerrain[i, j];
                    newGrid.timeMultiplier.Value = data.mapTimeMultiplier[i, j];
                    newGrid.edgeRes.Value = data.edgeResources[i, j];
                    
                    mGrids[i, j] = newGrid;
                    Debug.Log(mGrids[i, j]);
                }
            }
            
            // 获取地图中央坐标（若为双数行列，则以左边、上边格子中心为中央）
            centerGrid = mGrids[row / 2, col / 2];

            Debug.Log("CreateMap finished");
        }

        #endregion

        #region 通过CSV初始化

        public void CreateMapByCSV(int row, int col, string mapDataResPath)
        {
            if (mGrids != null)
            {
                Debug.LogError($"Map already existed! row: {mapRow} col: {mapCol}");
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
            Debug.LogError("SetGridData暂时没有做任何事");
            // 默认表内数据按顺序排列
            // boxGrid.timeMultiplier.Value = float.Parse(dataRow[attri["timeMultiplier"]]);
            // boxGrid.terrain.Value = int.Parse(dataRow[attri["terrain"]]);
        }

        #endregion

        public bool GridCanMoveTo(BoxGrid boxgrid)
        {
            if (!boxgrid.IsEmpty()) return false;
            if (boxgrid.terrain.Value == (int)TerrainEnum.Invalid) return false;
            if (boxgrid.terrain.Value == (int)TerrainEnum.Wall) return false;
            if (boxgrid.terrain.Value == (int)TerrainEnum.Edge) return false;
            if (boxgrid.gridStatus.Value == GridStatusEnum.AllyPiece) return false;
            if (boxgrid.gridStatus.Value == GridStatusEnum.MonsterPiece) return false;

            return true;
        }

        public int GetGridDist(BoxGrid grid1, BoxGrid grid2)
        {
            return Mathf.Abs(grid1.row - grid2.row) + Mathf.Abs(grid1.col - grid2.col);
        }
        
        public void Clear()
        {
            foreach (var grid in mGrids)
            {
                GameObject.Destroy(grid.gameObject);
            }
        }

        public void SetNUllMap()
        {
            mGrids = null;
        }
       
    }
}