using QFramework;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 地图格子相关的常量
    /// </summary>
    public struct GridConst
    {
        public static string TerrainResPathPrefix = "Sprites/Grids/terrain";    // 地块图片Resources路径前缀
    }
    
    /// <summary>
    /// 地图格子
    /// </summary>
    public class BoxGrid: MonoBehaviour, IController
    {
        public int row; // 行
        public int col; // 列
        public BindableProperty<int> terrain = new BindableProperty<int>((int)TerrainEnum.Road);    // 地形类型
        public BindableProperty<float> timeMultiplier = new BindableProperty<float>();  // 时间流逝倍数
        public int occupation; // 当前格子上的单位的ID
        public BindableProperty<GridStatus> gridStatus = new BindableProperty<GridStatus>(); // 格子状态
        
        // components
        private SpriteRenderer srFloor; // 地形图片

        // A*寻路算法需要的变量
        public int gCost; // 从初始点到目前点已经耗费的点数
        public int hCost; // 预计到终点还需要的点数
        public int fCost; // 总点数
        public BoxGrid cameFrom; // 上一个格子

        private void Start()
        {
            srFloor = transform.Find("Root/SpriteFloor").GetComponent<SpriteRenderer>();
            gridStatus.Value = GridStatus.Unoccupied;
            
            // 注册属性改变时会触发的方法
            terrain.RegisterWithInitValue(terr => OnTerrainChanged(terr));
            timeMultiplier.RegisterWithInitValue(time => OnTimeMultiplierChanged(time));
        }


        private void OnTerrainChanged(int terr)
        {
            // 改变地形图片
            var sprite = Resources.Load<Sprite>(GridConst.TerrainResPathPrefix + terr);
            srFloor.sprite = sprite;
        }

        private void OnTimeMultiplierChanged(float time)
        {
            // todo 速度变化触发的效果
            
        }

        /// <summary>
        /// 获取Architecture 每个IController都要写
        /// </summary>
        /// <returns></returns>
        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }

        // 输出一些信息
        public override string ToString()
        {
            return $"row: {row} col: {col} timeMultiplier: {timeMultiplier.Value} " +
                   $"terrain: {terrain}";
        }

        /// <summary>
        /// 检查格子是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return occupation == 0;
        }
    }
}