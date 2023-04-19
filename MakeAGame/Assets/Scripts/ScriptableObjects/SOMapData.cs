using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOMapData", menuName = "New")]
    public class SOMapData: SerializedScriptableObject
    {
        public int row; // 地图总行数
        public int col; // 地图总列数
        [TableMatrix(Transpose = true)] [BoxGroup("terrain")]
        public int[,] mapTerrain;   // 地形类型
        [TableMatrix(Transpose = true)] [BoxGroup("timeMultiplier")]
        public int[,] mapTimeMultiplier;  // 时间流速倍数 // todo 改为enum

        [ButtonGroup("1")]
        [Button("新建地图数据（覆盖旧数据）", ButtonSizes.Large)]
        public void BuildMap()
        {
            mapTerrain = new int[row, col];
            mapTimeMultiplier = new int[row, col];
            for(int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                mapTerrain[i, j] = (int) TerrainEnum.Road;
                mapTimeMultiplier[i, j] = 1;   
            }
        }
        
    }
}