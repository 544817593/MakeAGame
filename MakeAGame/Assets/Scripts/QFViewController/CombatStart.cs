using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 测试用，模拟进入局内，游戏开始
    /// </summary>
    public class CombatStart: MonoBehaviour, IController
    {
        public string mapDataResPath = "Data/MapTable1";    // 地图数据资源路径
        public SOMonsterSpawnSettings monsterSpawnSettings; // 怪物初始生成参数
        // 亡灵的初始生成位置
        public int undeadSpawnPositionX;
        public int undeadSpawnPositionY;
        public ViewCamera combatCamera;

        private void Start()
        {
            combatCamera.Init();
            FindCombatSettings();
            
            // IController通过Command与其他模块通信
            this.SendCommand(new InitCombatCommand(this));
            
        }

        private void FindCombatSettings()
        {
            switch (SceneFlow.combatSceneCount)
            {
                case 1:
                    mapDataResPath = "ScriptableObjects/Maps/SOMapData1-1a";
                    monsterSpawnSettings = Resources.Load<SOMonsterSpawnSettings>("ScriptableObjects/MonsterSpawnSettings/MonsterSpawnMap1-1a");
                    undeadSpawnPositionX = 10;
                    undeadSpawnPositionY = 6;
                    return;
                case 2:
                    mapDataResPath = "ScriptableObjects/Maps/SOMapData1-2a";
                    monsterSpawnSettings = Resources.Load<SOMonsterSpawnSettings>("ScriptableObjects/MonsterSpawnSettings/MonsterSpawnMap1-2a");
                    undeadSpawnPositionX = 10;
                    undeadSpawnPositionY = 13;
                    return;
            }
        }

        /// <summary>
        /// 获取Architecture 每个IController都要写
        /// </summary>
        /// <returns></returns>
        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}