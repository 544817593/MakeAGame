using QFramework;
using UnityEngine;

namespace Game
{
    public struct GridConst
    {
        // 地块图片路径前缀
        public static string TerrainResPathPrefix = "Sprites/Grids/terrain";
    }
    
    public class BoxGrid: MonoBehaviour, IController
    {
        public int row;
        public int col;
        public BindableProperty<int> statusType = new BindableProperty<int>();
        public BindableProperty<int> terrain = new BindableProperty<int>((int)TerrainEnum.Road);
        public BindableProperty<float> timeMultiplier = new BindableProperty<float>();
        public (int, int) status;   // todo 应该会删除
        
        // components
        private SpriteRenderer srFloor;

        private void Start()
        {
            srFloor = transform.Find("Root/SpriteFloor").GetComponent<SpriteRenderer>();
            
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
            // 速度变化触发的效果
            
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }

        public override string ToString()
        {
            return $"row: {row} col: {col} statusType: {statusType.Value} timeMultiplier: {timeMultiplier.Value} " +
                   $"status: {status} terrain: {terrain}";
        }
    }

    public enum TerrainEnum
    {
        Road, Wall, Water, Fire, Poision
    }
}