using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    [CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]

    public class SOPlayer : ScriptableObject
    {
        [SerializeField] int gold; // 玩家金币
        [SerializeField] float goldAcquisitionMultiplier; // 玩家获取金币倍率
        [SerializeField] int san; // 精神值
        [SerializeField] int maxSan; // 最大精神值
        [SerializeField] float sanRegenSpeed; // 精神值恢复速度
        [SerializeField] int movePieceTime; // 移动棋子次数


        // 玩家属性，力量，精神，技巧，体力，魅力
        [SerializeField] int strength;
        [SerializeField] int spirit;
        [SerializeField] int skill;
        [SerializeField] int stamina;
        [SerializeField] int chrisma;

        /// <summary>
        /// 调整玩家精神值恢复速度
        /// </summary>
        /// <param name="amount">要调整的额度</param>
        public void AddSanRegenSpeed(float amount)
        {
            sanRegenSpeed += amount;
        }

        /// <summary>
        /// 获取玩家精神值恢复速度
        /// </summary>
        /// <returns></returns>
        public float GetSanRegenSpeed()
        {
            return sanRegenSpeed;
        }

        /// <summary>
        /// 获取玩家最大精神值
        /// </summary>
        /// <returns></returns>
        public int GetMaxSan()
        {
            return maxSan;
        }

        /// <summary>
        /// 调整玩家最大精神值
        /// </summary>
        /// <param name="amount">要调整的精神值额度</param>
        public void AddMaxSan(int amount)
        {
            maxSan += amount;
        }

        /// <summary>
        /// 获得玩家现有金币数量
        /// </summary>
        /// <returns>玩家金币数量</returns>
        public int GetGold()
        {
            return gold;
        }

        /// <summary>
        /// 调整玩家金币
        /// </summary>
        /// <param name="amount">金币数量</param>
        public void AddGold(int amount)
        {
            gold += amount;
        }

        /// <summary>
        /// 获得玩家现在精神值
        /// </summary>
        /// <returns></returns>
        public int GetSan()
        {
            return san;
        }

        /// <summary>
        /// 调整玩家精神值
        /// </summary>
        /// <param name="amount">精神值数量</param>
        public void AddSan(int amount)
        {
            san += amount;
        }

        public int GetMovePieceTime()
        {
            return movePieceTime;
        }

        public void SetMovePieceTime(int amount)
        {
            movePieceTime = amount;
        }

        /// <summary>
        /// 获得玩家五维其中之一
        /// </summary>
        /// <param name="stats">需要返回的属性，力量，精神，技巧，体力，魅力</param>
        /// <returns>五维其中之一，错误时返回-1</returns>
        public int GetStats(PlayerStatsEnum stats)
        {
            switch (stats)
            {
                case PlayerStatsEnum.Strength: return strength;
                case PlayerStatsEnum.Spirit: return spirit;
                case PlayerStatsEnum.Skill: return skill;
                case PlayerStatsEnum.Stamina: return stamina;
                case PlayerStatsEnum.Charisma: return chrisma;
            }
            return -1;
        }

        /// <summary>
        /// 改变玩家五维其中之一
        /// </summary>
        /// <param name="stats">需要改变的属性，力量，精神，技巧，体力，魅力</param>
        /// <param name="amount">要改变的数值</param>
        public void ModifyStats(PlayerStatsEnum stats, int amount)
        {
            switch (stats)
            {
                case PlayerStatsEnum.Strength: strength += amount; return;
                case PlayerStatsEnum.Spirit: spirit += amount; return;
                case PlayerStatsEnum.Skill: skill += amount; return;
                case PlayerStatsEnum.Stamina: stamina += amount; return;
                case PlayerStatsEnum.Charisma: chrisma += amount; return;
            }
        }

        public void SetStartStats(PlayerStatsEnum stats, int amount)
        {
            switch (stats)
            {
                case PlayerStatsEnum.Strength: strength = amount; return;
                case PlayerStatsEnum.Spirit: spirit = amount; return;
                case PlayerStatsEnum.Skill: skill = amount; return;
                case PlayerStatsEnum.Stamina: stamina = amount; return;
                case PlayerStatsEnum.Charisma: chrisma = amount; return;
            }
        }
        public void SetStats()
        {
            strength = 0;
            spirit = 0;
            skill = 0;
            stamina = 0;
            chrisma = 0;
        }

    }
}
