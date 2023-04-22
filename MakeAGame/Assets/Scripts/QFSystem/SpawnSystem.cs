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

       
    }


    public class SpawnSystem : AbstractSystem, ISpawnSystem
    {
        protected override void OnInit()
        {

        }

        public void SpawnMonster(int row, int col, string name)
        {
            var grid = this.GetSystem<MapSystem>().mGrids;
            if (grid[row,col].IsEmpty())
            {
                var spawnMonsterEvent = new SpawnMonsterEvent {row = row, col = col, name = name};
                this.SendEvent(spawnMonsterEvent);
            }
        }
        public void SpawnDoor(int row, int col)
        {
            throw new System.NotImplementedException();
        }

    }

    /// <summary>
    /// 由SpawnSystem发出的怪物生成事件
    /// </summary>
    public class SpawnMonsterEvent : EnumEventSystem
    {
        public int row;
        public int col;
        public string name;
    }
}