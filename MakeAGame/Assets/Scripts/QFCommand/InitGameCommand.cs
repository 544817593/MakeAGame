using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 进入局内的初始化操作
    /// </summary>
    public class InitGameCommand: AbstractCommand
    {
        private GameStart info; // 初始化需要的信息
        
        public InitGameCommand(GameStart startInfo)
        {
            info = startInfo;
        }
        
        protected override void OnExecute()
        {
            // 地图
            var mapSystem = this.GetSystem<IMapSystem>();
            mapSystem.CreateMapBySO(info.mapDataResPath);
        }
    }
}