using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Game
{
    public interface ISpawnSystem : ISystem
    {
        /// <summary>
        /// 在第row行col列生成怪物
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <param name="name">怪物名字</param>
        void SpawnMonster(int row, int col, string name);

        /// <summary>
        /// 在第row行col列生成关卡大门
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        void SpawnDoor(int row, int col);

        /// <summary>
        /// 在区域内随机生成怪物，持续一段时间
        /// </summary>
        /// <param name="settings">该地图的初始怪物生成SO数据</param>
        /// <returns></returns>
        void ConstantSpawnMonster(SOMonsterSpawnSettings settings);

        /// <summary>
        /// 返回应该设置的棋子独一份的ID
        /// </summary>
        /// <returns></returns>
        int GetPieceIdCounter();

        /// <summary>
        /// 增加棋子独一份ID的计数器
        /// </summary>
        void IncrementPieceIdCounter();

        List<Monster> GetMonsterList();

        List<TempAllyScript> GetAllyList();


    }


    public class SpawnSystem : AbstractSystem, ISpawnSystem
    {
        // 棋子ID，每个棋子独一份，设置为从1开始，因为BoxGrid.isEmpty()插空为格子上的occupation是否为0
        private int pieceIdCounter = 1; 

        // 怪物和友军的列表
        private List<Monster> monsterList = new List<Monster>();
        private List<TempAllyScript> allyList = new List<TempAllyScript>();
        // interactableList待添加

        protected override void OnInit()
        {
            
        }

        public void SpawnMonster(int row, int col, string name)
        {
            var grid = this.GetSystem<IMapSystem>().Grids();
            if (grid[row,col].IsEmpty())
            {
                var spawnMonsterEvent = new SpawnMonsterEvent 
                    {row = row, col = col, name = name, pieceId = pieceIdCounter};
                pieceIdCounter++;
                this.SendEvent(spawnMonsterEvent);
            }
        }

        public void SpawnDoor(int row, int col)
        {
            throw new System.NotImplementedException();
        }

        public void ConstantSpawnMonster(SOMonsterSpawnSettings settings)
        {
            for (int i = 0; i < settings.spawnPoints.Count; i++)
            {
                var constantSpawnMonsterEvent = new ConstantSpawnMonsterEvent
                {
                    spawnPoint = settings.spawnPoints[i],
                    spawnProbability = settings.spawnProbability[i],
                    duration = settings.spawnDuration[i],
                    cooldown = settings.spawnCooldown[i],
                    name = settings.monsterName[i]
                };
                this.SendEvent(constantSpawnMonsterEvent);
            }
        }

        public int GetPieceIdCounter()
        {
            return pieceIdCounter;
        }

        public void IncrementPieceIdCounter()
        {
            pieceIdCounter++;
        }

        public List<Monster> GetMonsterList()
        {
            return monsterList;
        }

        public List<TempAllyScript> GetAllyList()
        {
            return allyList;
        }
    }

    /// <summary>
    /// 由SpawnSystem发出的怪物生成事件
    /// </summary>
    public struct SpawnMonsterEvent
    {
        public int row;
        public int col;
        public string name;
        public int pieceId;
    }

    /// <summary>
    /// 由SpawnSystem发出的怪物持续生成事件
    /// </summary>
    public struct ConstantSpawnMonsterEvent
    {
        public Vector2Int spawnPoint;
        public int spawnProbability;
        public int duration;
        public int cooldown;
        public string name;
    }
}