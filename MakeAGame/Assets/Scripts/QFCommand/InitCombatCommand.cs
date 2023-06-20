using BagUI;
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
            Debug.LogError("Init combat command execute");
            InitMap();
            
            // 界面
            UIKit.OpenPanel<UIHandCard>();
            UIKit.OpenPanel<UIInventoryQuickSlot>();
            UIKit.OpenPanel<UIAbilityPanel>();
            UIKit.OpenPanel<UIRelic>();

            // 亡灵
            this.SendEvent(new SpawnUndeadEvent { undeadSpawnPositionX = info.undeadSpawnPositionX, 
                undeadSpawnPositionY = info.undeadSpawnPositionY });

            // 怪物
            if (info.monsterSpawnSettings != null)
            {
                var spawnSystem = this.GetSystem<ISpawnSystem>();
                spawnSystem.ConstantSpawnMonster(info.monsterSpawnSettings);
            }

            // 从背包里抽出七张手牌
            var inventorySystem = this.GetSystem<IInventorySystem>();
            for (int i = 0; i < this.GetSystem<IHandCardSystem>().maxCardCount; i++)
            {
                if (inventorySystem.GetBagCardList().Count != 0)
                {
                    Card card = inventorySystem.DrawCard();
                    this.SendCommand<AddHandCardCommand>(new AddHandCardCommand(card));
                }
                else
                {
                    break;
                }
            }

            // 持续抽卡协程
            var refillHandCardEvent = new RefillHandCardEvent
            {
                drawCardCooldown = 5f
            };
            this.SendEvent(refillHandCardEvent);
            
            // 计时测试
            var updateSystem = this.GetSystem<IUpdateSystem>();
            updateSystem.Reset();
            updateSystem.ScheduleExecute(CountTest, false, 1f);
            
            // 遗物系统开始接受计时
            this.GetSystem<IRelicSystem>().StartCountTime();
            
            this.SendEvent<RoomCombatStartEvent>();
        }

        void CountTest()
        {
            this.SendEvent<CountTimeEvent>();
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