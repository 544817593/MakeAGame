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
            mapDataResPath = "ScriptableObjects/Maps/SOMapData1-" + SceneFlow.combatSceneCount + "a";

            monsterSpawnSettings = Resources.Load<SOMonsterSpawnSettings>
                ("ScriptableObjects/MonsterSpawnSettings/MonsterSpawnMap1-" + SceneFlow.combatSceneCount + "a");

            undeadSpawnPositionX = Resources.Load<SOMapData>
                ("ScriptableObjects/Maps/SOMapData1-" + SceneFlow.combatSceneCount + "a").undeadX;

            undeadSpawnPositionY = Resources.Load<SOMapData>
                ("ScriptableObjects/Maps/SOMapData1-" + SceneFlow.combatSceneCount + "a").undeadY;            
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