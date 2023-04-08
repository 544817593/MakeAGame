using QFramework;
using UnityEngine;

namespace Game
{
    public class GameEntry : Architecture<GameEntry>
    {
        protected override void Init()
        {
            // 注册各系统
            RegisterSystem<IGridGeneratorSystem>(new GridGeneratorSystem());
            
            
            Debug.Log("GameEntry: Init");
        }
    }
}