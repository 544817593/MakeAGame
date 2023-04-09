using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 进入局内的初始化操作
    /// </summary>
    public class InitGameCommand: AbstractCommand
    {
        private GameStart info;
        
        public InitGameCommand(GameStart startInfo)
        {
            info = startInfo;
        }
        
        protected override void OnExecute()
        {
            var mapSystem = this.GetSystem<IMapSystem>();
            mapSystem.CreateMapBySO(info.mapDataResPath);
        }
    }
}