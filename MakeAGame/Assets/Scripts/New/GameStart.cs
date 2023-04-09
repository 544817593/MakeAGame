using System;
using QFramework;
using UnityEngine;

namespace Game
{
    public class GameStart: MonoBehaviour, IController
    {
        // 地图数据文件路径
        public string mapDataResPath = "Data/MapTable1";
        
        
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