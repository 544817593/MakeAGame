using InventoryQuickslotUI;
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
            this.info = startInfo;
        }
        
        protected override void OnExecute()
        {
            InitMap();
            
            // 界面
            UIKit.OpenPanel<UIHandCard>();
            UIKit.OpenPanel<UIInventoryQuickSlot>();
            UIKit.OpenPanel<UIAbilityPanel>();

            // 怪物
            var spawnSystem = this.GetSystem<ISpawnSystem>();
            spawnSystem.ConstantSpawnMonster(info.monsterSpawnSettings);
        }

        private void InitMap()
        {
            // 地图
            var mapSystem = this.GetSystem<IMapSystem>();
            mapSystem.CreateMapBySO(info.mapDataResPath);
            
            // 镜头位置
            ChangeCameraTargetEvent setCameraCenterEvent = new ChangeCameraTargetEvent()
            {
                target = mapSystem.centerGrid.transform
            };
            this.SendEvent<ChangeCameraTargetEvent>(setCameraCenterEvent);
        }
    }
}