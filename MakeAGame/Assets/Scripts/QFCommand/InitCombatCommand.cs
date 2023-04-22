using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 进入局内的初始化操作
    /// </summary>
    public class InitCombatCommand: AbstractCommand
    {
        private CombatStart info; // 初始化需要的信息
        
        public InitCombatCommand(CombatStart startInfo)
        {
            info = startInfo;
        }
        
        protected override void OnExecute()
        {
            // 地图
            var mapSystem = this.GetSystem<IMapSystem>();
            mapSystem.CreateMapBySO(info.mapDataResPath);

            // 怪物
            var spawnSystem = this.GetSystem<ISpawnSystem>();
            spawnSystem.SpawnMonster(3, 3, "Ghast");
        }
    }
}