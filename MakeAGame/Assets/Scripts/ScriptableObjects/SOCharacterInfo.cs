using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOCharacterInfo", menuName = "ScriptableObjects/Character")]
    public class SOCharacterInfo: ScriptableObject
    {
        public int characterID; // 角色id
        public string characterName;    // 角色名字
        
        [FoldoutGroup("Card Information", true)]
        public RarityEnum rarity;  // 稀有度
        [FoldoutGroup("Card Information")]
        public int sanCost; // 精神值消耗
        [FoldoutGroup("Card Information")] [TextArea(1, 2)]
        public string deathFuncDescription; // 死面描述
        [FoldoutGroup("Card Information")]
        public Sprite cardSprite;   // 卡面图片
        [FoldoutGroup("Card Information")]
        public float hp;  // 血量
        [FoldoutGroup("Card Information")]
        public float attack;  // 攻击力
        [FoldoutGroup("Card Information")]
        public float moveSpd; // 行动速度
        [FoldoutGroup("Card Information")]
        public float defend;  // 防御力

        [FoldoutGroup("Features", true)]
        public List<SOFeature> features;  // 特性
        [FoldoutGroup("Features")]
        public SOFeature specialFeature;  // 特殊属性
        
        [FoldoutGroup("Piece Information", true)]
        public Sprite pieceSprite;  // 棋子图片
        [FoldoutGroup("Piece Information")]
        public int width;   // 棋子占地宽
        [FoldoutGroup("Piece Information")]
        public int height;  // 棋子占地长
        [FoldoutGroup("Piece Information")]
        public DirEnum[] moveDirections;    // 移动方向
        [FoldoutGroup("Piece Information")]
        public float attackSpd;   // 攻速
        [FoldoutGroup("Piece Information")]
        public float accracy;   // 命中率
        [FoldoutGroup("Piece Information")]
        public int attackRange; // 攻击范围
        [FoldoutGroup("Piece Information")]
        public float life; // 寿命

        [FoldoutGroup("Player Stat Bonus", true)]
        public PlayerBonus sanCostBonus;
        [FoldoutGroup("Player Stat Bonus")]
        public PlayerBonus hpBonus;
        [FoldoutGroup("Player Stat Bonus")]
        public PlayerBonus atkBonus;
        [FoldoutGroup("Player Stat Bonus")]
        public PlayerBonus atkSpdBonus;
        

        [Serializable]
        public class PlayerBonus
        {
            public PlayerStatsEnum stat;
            public float multiple;
        }
    }
}