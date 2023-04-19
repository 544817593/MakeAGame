using System;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 测试用，模拟进入局内，游戏开始
    /// </summary>
    public class GameStart: MonoBehaviour, IController
    {
        public string mapDataResPath = "Data/MapTable1";    // 地图数据资源路径
        
        
        private void Start()
        {
            // IController通过Command与其他模块通信
            this.SendCommand(new InitGameCommand(this));
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}