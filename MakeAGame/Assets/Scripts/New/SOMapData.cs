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
        public int[,] mapTerrain;   // 地形
        [TableMatrix(Transpose = true)] [BoxGroup("timeMultiplier")]
        public float[,] mapTimeMultiplier;  // 时间流速

        [ButtonGroup("1")]
        [Button("新建地图数据（覆盖旧数据）", ButtonSizes.Large)]
        public void BuildMap()
        {
            mapTerrain = new int[row, col];
            mapTimeMultiplier = new float[row, col];
            for(int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                mapTimeMultiplier[i, j] = 1f;
        }
        
    }
}