using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOCharacterInfo", menuName = "ScriptableObjects/Character")]
    public class SOCharacterInfo: ScriptableObject
    {
        public int characterID;         // 角色id
        public string characterName;    // 角色名字

        [FoldoutGroup("Card Information", true)]
        public RarityEnum rarity;       // 稀有度
        [FoldoutGroup("Card Information")] 
        public CardPackEnum cardPack;   //所属卡包
        [FoldoutGroup("Card Information")]
        public int sanCost;             // 精神值消耗
        [FoldoutGroup("Card Information")] [TextArea(1, 2)]
        public string deathFuncDescription; // 死面描述
        [FoldoutGroup("Card Information")]
        public Sprite cardSprite;       // 卡面图片
        [FoldoutGroup("Card Information")]
        public int hp;                // 血量
        [FoldoutGroup("Card Information")]
        public float attack;            // 攻击力
        [FoldoutGroup("Card Information")]
        public float moveSpd;           // 行动速度
        [FoldoutGroup("Card Information")]
        public float defend;            // 防御力
        [FoldoutGroup("Card Information")]
        public string deathFuncName;    // 死面功能类名

        [FoldoutGroup("Features", true)]
        public List<FeatureEnum> features;// 特性
        [FoldoutGroup("Features")]
        public List<FeatureEnum> specialFeatures;  // 特殊属性
        
        [FoldoutGroup("Piece Information", true)]
        public Sprite pieceSprite;      // 棋子图片
        [FoldoutGroup("Piece Information")]
        public int width;               // 棋子占地宽
        [FoldoutGroup("Piece Information")]
        public int height;              // 棋子占地长
        [FoldoutGroup("Piece Information")]
        public List<DirEnum> moveDirections;    // 移动方向
        [FoldoutGroup("Piece Information")]
        public float attackSpd;         // 攻速
        [FoldoutGroup("Piece Information")] [Range(0, 1)]
        public float accuracy;           // 命中率
        [FoldoutGroup("Piece Information")]
        public int attackRange;         // 攻击范围
        [FoldoutGroup("Piece Information")]
        public float life;              // 寿命
        [FoldoutGroup("Piece Information")]
        public GameObject anim;         // 动画预设体

        [FoldoutGroup("Player Stat Bonus", true)]
        public PlayerBonus sanCostBonus;    // 精神消耗玩家属性加成
        [FoldoutGroup("Player Stat Bonus")]
        public PlayerBonus hpBonus;         // 血量玩家属性加成
        [FoldoutGroup("Player Stat Bonus")]
        public PlayerBonus atkBonus;        // 攻击力玩家属性加成
        [FoldoutGroup("Player Stat Bonus")]
        public PlayerBonus atkSpdBonus;     // 攻速玩家属性加成

        public GameObject GetAnim()
        {
            return anim;
        }

        [Serializable]
        public class PlayerBonus
        {
            public PlayerStatsEnum stat;    // 属性
            public float multiple;          // 倍数
        }
    }
}