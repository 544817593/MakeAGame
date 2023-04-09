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
            RegisterUtility<ICSVImportUtility>(new CSVImportUtility());
            
            Debug.Log("GameEntry: Init");
        }
    }
}