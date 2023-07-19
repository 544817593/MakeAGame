using InventoryQuickslotUI;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 游戏架构，包含所有System、Model和Utility
    /// </summary>
    public class GameEntry : Architecture<GameEntry>
    {
        public bool isDev = true;  // 是否为开发测试模式
        
        protected override void Init()
        {
            // 注册各模块
            RegisterSystem<IUpdateSystem>(new UpdateSystem());
            RegisterSystem<IMapSystem>(new MapSystem());
            RegisterSystem<IGridGeneratorSystem>(new GridGeneratorSystem());
            RegisterSystem<IMapSelectSystem>(new MapSelectSystem());
            RegisterSystem<IHandCardSystem>(new HandCardSystem());
            RegisterSystem<ICardGeneratorSystem>(new CardGeneratorSystem());
            RegisterSystem<IPieceSystem>(new PieceSystem());
            RegisterSystem<IPieceGeneratorSystem>(new PieceGeneratorSystem());
            RegisterSystem<IPieceBattleSystem>(new PieceBattleSystem());
            RegisterSystem<ISpawnSystem>(new SpawnSystem());
            RegisterSystem<IMovementSystem>(new MovementSystem());
            RegisterSystem<IInventorySystem>(new InventorySystem());
            RegisterSystem<ISkillSystem>(new SkillSystem());
            RegisterSystem<IShopSystem>(new ShopSystem());
            RegisterSystem<IRoomSystem>(new RoomSystem());
            RegisterSystem<IRelicSystem>(new RelicSystem());
            RegisterSystem<IExploreSystem>(new ExploreSystem());
            RegisterUtility<ICSVImportUtility>(new CSVImportUtility());
            

            // 初始化资源管理
            ResKit.Init();

            if (isDev)
            {
                // 开启控制台
                UIKit.OpenPanel<UIConsolePanel>(UILevel.PopUI);
            }
            UIKit.OpenPanel<PauseUI.Pause>();


            Debug.Log($"GameEntry: Init, isDev {isDev}");
        }
    }
}