using System.Collections.Generic;
using DG.Tweening;
using Ink.Parsed;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IExploreSystem : ISystem
    {
        int leftMove { get; set; }
        void CreateMapBySO(SOExploreMapData data);
        bool MovePlayer(DirEnum dir);
    }
    
    public class ExploreSystem: AbstractSystem, IExploreSystem
    {
        private const string rewardPrefabName = "ExploreReward";
        
        private BoxGrid[,] mGrids; // 地图格子数组
        private List<ExploreReward> rewards = new List<ExploreReward>();
        private List<ViewExploreReward> viewRewards = new List<ViewExploreReward>();
        private ViewExploreReward rewardToGet;
        private int crtRow;
        private int crtCol;
        private Transform player;
        public int leftMove { get; set; }

        protected override void OnInit()
        {
            
        }

        public void CreateMapBySO(SOExploreMapData data)
        {
            int row = data.row;
            int col = data.col;
            mGrids = new BoxGrid[row, col];
            rewards = data.rewards;
            crtRow = data.undeadRow;
            crtCol = data.undeadCol;
            leftMove = 5;

            var gridGenerator = this.GetSystem<IGridGeneratorSystem>();
            Transform root = GameObject.Find("MapRoot").transform;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    var newGrid = gridGenerator.CreateGrid(i, j, root);

                    // 地图格子真正赋值数据的地方
                    newGrid.terrain.Value = data.mapTerrain[i, j];
                    // newGrid.timeMultiplier.Value = data.mapTimeMultiplier[i, j];
                    // newGrid.edgeRes.Value = data.edgeResources[i, j];

                    mGrids[i, j] = newGrid;
                    Debug.Log(mGrids[i, j]);
                }
            }
            
            CreateRewards();

            player = GameObject.Find("Player").transform;
            SetPlayerPos(data.undeadRow, data.undeadCol);

            Debug.Log("Create Explore Map finished");
        }

        private void CreateRewards()
        {
            var prefab = (GameObject) Resources.Load($"Prefabs/{rewardPrefabName}");
            Transform rewardRoot = GameObject.Find("GameRoot/RewardRoot").transform;
            foreach (var reward in rewards)
            {
                GameObject rewardGO = GameObject.Instantiate(prefab, rewardRoot);
                Vector3 pos = mGrids[reward.row, reward.col].transform.position;
                rewardGO.transform.position = pos;
                var viewReward = rewardGO.GetComponent<ViewExploreReward>();
                viewReward.SetReward(reward);
                viewRewards.Add(viewReward);
                Debug.Log($"explore reward put at row {reward.row}, col {reward.col}, info: {reward.addStats} {reward.addNum} {reward.relicID}");
            }
        }

        public bool MovePlayer(DirEnum dir)
        {
            if (leftMove <= 0)
                return false;
            
            int nextCol = -1;
            int nextRow = -1;
            GetNextPos(dir, out nextRow, out nextCol);
            // Debug.Log($"next row: {nextRow} col: {nextCol}");
            if (nextRow < 0 || nextRow >= mGrids.GetLength(0) || nextCol < 0 || nextCol >= mGrids.GetLength(1))
            {
                return false;
            }

            if (mGrids[nextRow, nextCol].terrain.Value == (int)TerrainEnum.Wall)
            {
                return false;
            }

            leftMove--;
            crtCol = nextCol;
            crtRow = nextRow;
            
            // 奖励获取判断
            ViewExploreReward viewReward = null;
            viewReward = viewRewards.Find(item => item.reward.row == nextRow && item.reward.col == nextCol);
            if (viewReward != null)
            {
                Debug.Log("has reward");
                rewardToGet = viewReward;
            }
            
            SetPlayerPos(crtRow, crtCol, true);
            
            return true;
        }

        private void SetPlayerPos(int row, int col, bool hasAnim = false)
        {
            Vector3 newPos = mGrids[row, col].transform.position;
            if (!hasAnim)
            {
                player.transform.position = newPos;
            }
            else
            {
                player.transform.DOMove(newPos, 0.5f).OnComplete(GetReward);
            }
        }

        private void GetNextPos(DirEnum dir, out int row, out int col)
        {
            row = crtRow;
            col = crtCol;
            switch (dir)
            {
                case DirEnum.Top:
                    row = crtRow - 1;
                    break;
                case DirEnum.Right:
                    col = crtCol + 1;
                    break;
                case DirEnum.Down:
                    row = crtRow + 1;
                    break;
                case DirEnum.Left:
                    col = crtCol - 1;
                    break;
                default:
                    break;
            }
        }

        private void GetReward()
        {
            Debug.Log($"try get reward, isnull {rewardToGet == null}");
            if (rewardToGet != null)
            {
                rewardToGet.reward.OnGetReward();
                rewardToGet.gameObject.SetActive(false);
                viewRewards.Remove(rewardToGet);

                rewardToGet = null;
            }
        }
    }
}