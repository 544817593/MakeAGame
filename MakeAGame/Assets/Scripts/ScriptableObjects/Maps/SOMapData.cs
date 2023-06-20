using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOMapData", menuName = "ScriptableObjects/MapData")]
    public class SOMapData: SerializedScriptableObject
    {
        public int row; // 地图总行数
        public int col; // 地图总列数
        public int upperRowEdges; // 顶部的地图边界需要行数
        public int lowerRowEdges; // 底部的地图边界需要行数
        public int leftColEdges; // 左边的地图边界需要列数
        public int rightColEdges; // 右边的地图边界需要列数
        public int undeadX; // 亡灵X坐标
        public int undeadY; // 亡灵Y坐标
        [TableMatrix(Transpose = true)] [BoxGroup("terrain")]
        public int[,] mapTerrain;   // 地形类型
        [TableMatrix(Transpose = true)] [BoxGroup("timeMultiplier")]
        public TimeMultiplierEnum[,] mapTimeMultiplier;  // 时间流速倍数
        [TableMatrix(Transpose = true)] [BoxGroup("SpriteResources")]
        public EdgeSprite[,] edgeResources;  // 背景图片资源

        [ButtonGroup("1")]
        [Button("新建地图数据（覆盖旧数据）", ButtonSizes.Large)]
        public void BuildMap()
        {
            mapTerrain = new int[row, col];
            mapTimeMultiplier = new TimeMultiplierEnum[row, col];
            edgeResources = new EdgeSprite[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (i < upperRowEdges || i >= row - lowerRowEdges || j < leftColEdges || j >= col - rightColEdges)
                    {
                        mapTerrain[i, j] = (int)TerrainEnum.Edge;
                        mapTimeMultiplier[i, j] = TimeMultiplierEnum.Normal;
                        edgeResources[i, j] = EdgeSprite.Wall_Mid_Candle;
                    }
                    else
                    {
                        Debug.Log($"{i},{j}");
                        mapTerrain[i, j] = (int)TerrainEnum.Road;
                        mapTimeMultiplier[i, j] = TimeMultiplierEnum.Normal;
                        edgeResources[i, j] = EdgeSprite.None;
                    }
                }
            }
        }
        
    }
}