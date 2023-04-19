using QFramework;
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
        public BindableProperty<int> statusType = new BindableProperty<int>();  // todo
        // todo 不知道为什么BindableProperty<enum>不好使，以后看看能不能换
        public BindableProperty<int> terrain = new BindableProperty<int>((int)TerrainEnum.Road);    // 地形类型
        public BindableProperty<float> timeMultiplier = new BindableProperty<float>();  // 时间流逝倍数
        public (int, int) status;   // todo 应该会删除
        
        // components
        private SpriteRenderer srFloor; // 地形图片

        private void Start()
        {
            srFloor = transform.Find("Root/SpriteFloor").GetComponent<SpriteRenderer>();
            
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

        // 获取Architecture 每个IController都要写
        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }

        // 输出一些信息
        public override string ToString()
        {
            return $"row: {row} col: {col} statusType: {statusType.Value} timeMultiplier: {timeMultiplier.Value} " +
                   $"status: {status} terrain: {terrain}";
        }
    }
}