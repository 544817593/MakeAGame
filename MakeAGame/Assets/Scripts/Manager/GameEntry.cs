using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 游戏架构，包含所有System、Model和Utility
    /// </summary>
    public class GameEntry : Architecture<GameEntry>
    {
        protected override void Init()
        {
            // 注册各模块
            RegisterSystem<IMapSystem>(new MapSystem());
            RegisterSystem<IGridGeneratorSystem>(new GridGeneratorSystem());
            RegisterSystem<IMapSelectSystem>(new MapSelectSystem());
            RegisterSystem<IHandCardSystem>(new HandCardSystem());
            RegisterSystem<ICardGeneratorSystem>(new CardGeneratorSystem());
            RegisterSystem<IPieceSystem>(new PieceSystem());
            RegisterSystem<IPieceGeneratorSystem>(new PieceGeneratorSystem());
            RegisterSystem<ISpawnSystem>(new SpawnSystem());
            RegisterSystem<IMovementSystem>(new MovementSystem());
            RegisterSystem<IInventorySystem>(new InventorySystem());
            RegisterSystem<ISkillSystem>(new SkillSystem());
            RegisterUtility<ICSVImportUtility>(new CSVImportUtility());
            RegisterSystem<IShopSystem>(new ShopSystem());

            // 初始化资源管理
            ResKit.Init();

            // 开启控制台
            UIKit.OpenPanel<UIConsolePanel>();

            Debug.Log("GameEntry: Init");
        }
    }
}