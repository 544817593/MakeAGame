using QFramework;
using UnityEngine;

namespace Game
{
    public interface IExploreMapSystem : ISystem
    {
        void CreateMapBySO(SOExploreMapData data);
    }
    
    public class ExploreMapSystem: AbstractSystem, IExploreMapSystem
    {
        private BoxGrid[,] mGrids; // 地图格子数组
        
        protected override void OnInit()
        {
            
        }

        public void CreateMapBySO(SOExploreMapData data)
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
                    // newGrid.terrain.Value = data.mapTerrain[i, j];
                    // newGrid.timeMultiplier.Value = data.mapTimeMultiplier[i, j];
                    // newGrid.edgeRes.Value = data.edgeResources[i, j];

                    mGrids[i, j] = newGrid;
                    Debug.Log(mGrids[i, j]);
                }
            }

            // // 获取地图中央坐标（若为双数行列，则以左边、上边格子中心为中央）
            // centerGrid = mGrids[row / 2, col / 2];
            //
            // // 对准背景，限制相机视野
            // var bgGo = GameObject.Find("Background");
            // var bgSr = bgGo.GetComponent<SpriteRenderer>();
            // if (bgGo != null && bgSr != null)
            // {
            //     Vector3 centerPos = centerGrid.transform.position;
            //     bgGo.transform.position = new Vector3(centerPos.x, centerPos.y, bgGo.transform.position.z);
            //
            //     SetCameraBorderEvent e = new SetCameraBorderEvent();
            //     Debug.Log($"bg size {bgSr.size}, centerPos {centerPos}");
            //     float sizeX = bgSr.size.x;
            //     float sizeY = bgSr.size.y;
            //     e.viewCorners = new Vector3[4];
            //     e.viewCorners[0] = centerPos + new Vector3(-sizeX / 2, sizeY / 2, 0);
            //     e.viewCorners[1] = centerPos + new Vector3(sizeX / 2, sizeY / 2, 0);
            //     e.viewCorners[2] = centerPos + new Vector3(-sizeX / 2, -sizeY / 2, 0);
            //     e.viewCorners[3] = centerPos + new Vector3(sizeX / 2, -sizeY / 2, 0);
            //     Debug.Log(
            //         $"leftUp: {e.viewCorners[0]} rightUp: {e.viewCorners[1]} leftDown: {e.viewCorners[2]} rightDown: {e.viewCorners[3]}");
            //
            //     this.SendEvent<SetCameraBorderEvent>(e);
            // }

            Debug.Log("Create Explore Map finished");
        }
    }
}