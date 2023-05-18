using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOCharacterInfo", menuName = "ScriptableObjects/Character")]
    public class SOCharacterInfo: ScriptableObject
    {
        public int characterID; // 角色id
        public string characterName;    // 角色名字
        
        public int rarity;  // 稀有度
        public int sanCost; // 精神值消耗
        public string deathFuncDescription; // 死面描述
        public Sprite cardSprite;   // 卡面图片

        public int hp;  // 血量
        public int attack;  // 攻击力
        public float moveSpd; // 行动速度
        public int defend;  // 防御力

        public List<SOFeature> features;  // 特性
        public SOFeature specialFeature;  // 特殊属性
        
        public Sprite pieceSprite;  // 棋子图片
        public int width;   // 棋子占地宽
        public int height;  // 棋子占地长
        public int[] moveDirections;    // 移动方向

        public float attackSpd;   // 攻速
        public float accracy;   // 命中率
        public int attackRange; // 攻击范围

        public float life; // 寿命
    }
}