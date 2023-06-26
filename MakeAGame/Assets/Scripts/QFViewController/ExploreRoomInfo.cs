using System;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 探索房初始化信息
    /// </summary>
    public class ExploreRoomInfo: MonoBehaviour, IController
    {
        // public string mapDataResPath = "Data/MapTable1";    // 地图数据资源路径
        public SOExploreMapData soExploreMap;

        private void Start()
        {
            Debug.Log($"ExploreRoomInfo Start, so: {soExploreMap.name}");

            IExploreMapSystem mapSys = this.GetSystem<IExploreMapSystem>();
            mapSys.CreateMapBySO(soExploreMap);
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}