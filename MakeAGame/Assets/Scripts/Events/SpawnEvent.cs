using UnityEngine;

namespace Game
{
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

    /// <summary>
    /// 由SpawnSystem发出的生成卡牌事件
    /// </summary>
    public struct SpawnCardEvent
    {
        public int cardId;
    }
}