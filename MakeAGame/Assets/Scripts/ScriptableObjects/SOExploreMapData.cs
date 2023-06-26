using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOExploreMapData", menuName = "ScriptableObjects/ExploreMapData")]
    public class SOExploreMapData: SerializedScriptableObject
    {
        public int row; // 地图总行数
        public int col; // 地图总列数
        public int undeadX; // 亡灵X坐标
        public int undeadY; // 亡灵Y坐标
        public List<ExploreReward> rewards = new List<ExploreReward>();
        [TableMatrix(Transpose = true)] [BoxGroup("terrain")]
        public int[,] mapTerrain;   // 地形类型

        [ButtonGroup("1")]
        [Button("新建地图数据（覆盖旧数据）", ButtonSizes.Large)]
        public void BuildMap()
        {
            mapTerrain = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mapTerrain[i, j] = (int)TerrainEnum.Road;
                }
            }
        }
    }

    /// <summary>
    /// 探索房间奖励类型
    /// </summary>
    public struct ExploreReward
    {
        public int row; // 行
        public int col; // 列
        public PlayerStatsEnum addStats;    // 增加属性
        public float addNum;    // 增加多少
        public bool hasChest;   // 是否还有别的东西，宝箱什么的
    }
}