using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 存放卡牌数据（而不是实际表现）
    /// </summary>
    public class Card: ICloneable
    {
        public int charaID { get; } // 角色id
         int instID = -1;    // 卡牌实体id，可能根据该卡牌位于背包中还是手牌中，分别由背包和手牌系统管理    // todo 可能是没用的，暂时不要给它赋值

        public int enhanceID = 1; // 临时用来测试强化界面的数据 

        // （已改动）应该不会变动的数据，就不另外存储了，直接用so
        // 卡牌数据与so解绑，所有卡牌维护自己的一份数据
        public SOCharacterInfo so;
        public string deathFuncDesc { get;  set; }
        public Sprite cardSprite { get;  set; }
        public Sprite pieceSprite { get;  set; }
        public List<FeatureEnum> specialFeature { get;  set; }
        public int width { get;  set; }
        public int height { get;  set; }
        public List<DirEnum> moveDirections { get;  set; }
        public int atkRange { get;  set; }
        public float atkSpd { get;  set; }

        // 会有改动的数据  // 外部可以读取，但不可以直接改动
        public RarityEnum rarity { get;  set; }
        public float sanCost { get;  set; }
        public int hp { get;  set; }
        public int maxHp { get; set; }
        public float moveSpd { get;  set; }
        public float damage { get;  set; }
        public float defend { get;  set; }
        public int enhancement { get;  set; }
        public string charaName { get;  set; }
        public float maxLife { get;  set; }
        public float currLife { get;  set; }
        public List<FeatureEnum> features { get;  set; }
        public float accuracy { get; set; }

        public DeathEnhancement deathEnhancement;

        public struct DeathEnhancement
        {
            public int damageIncrease; // 增加的伤害
            public int healthIncrease; // 增加的回复量
            public int statusTimeIncrease; // 增加的持续时间
            public bool extraDamageEffect; // 是否有“对随机三个敌人造成恢复量的伤害”的效果，只有可以恢复的死面才有可能获得
            /// <summary>
            /// 初始化默认的死面强化效果
            /// </summary>
            /// <param name="_damageIncrease"></param>
            /// <param name="_healthIncrease"></param>
            /// <param name="_statusTimeIncrease"></param>
            /// <param name="_extraDamageEffect"></param>
            public DeathEnhancement(int _damageIncrease = 0, int _healthIncrease = 0, int _statusTimeIncrease = 0, bool _extraDamageEffect = false)
            {
                damageIncrease = _damageIncrease;
                healthIncrease = _healthIncrease;
                statusTimeIncrease = _statusTimeIncrease;
                extraDamageEffect = _extraDamageEffect;
            }
        }

        public Card(int _charaID)
        {
            charaID = _charaID;
            InitData();
        }

        void InitData()
        {
            so = IdToSO.FindCardSOByID(charaID);
            deathFuncDesc = so.deathFuncDescription;
            cardSprite = so.cardSprite;
            pieceSprite = so.pieceSprite;
            specialFeature = so.specialFeatures;
            width = so.width;
            height = so.height;
            moveDirections = so.moveDirections;
            atkRange = so.attackRange;
            atkSpd = so.attackSpd + (GameManager.Instance.playerMan.player.GetStats(so.atkSpdBonus.stat) *
                so.atkSpdBonus.multiple);
            rarity = so.rarity;
            sanCost = so.sanCost + (int)(GameManager.Instance.playerMan.player.GetSumStats() *
                so.sanCostBonus.multiple);
            hp = so.hp + (int)(GameManager.Instance.playerMan.player.GetStats(so.hpBonus.stat) *
                so.hpBonus.multiple);
            maxHp = so.hp + (int)(GameManager.Instance.playerMan.player.GetStats(so.hpBonus.stat) *
                so.hpBonus.multiple);
            moveSpd = so.moveSpd;
            damage = so.attack + (GameManager.Instance.playerMan.player.GetStats(so.atkBonus.stat) *
                so.atkBonus.multiple);
            defend = so.defend;
            charaName = so.characterName;
            maxLife = so.life;
            features = so.features;
            accuracy = so.accuracy;
            PrintData();
        }

        /// <summary>
        /// 添加特性
        /// </summary>
        /// <param name="feature">要添加的特性</param>
        public void AddFeature(FeatureEnum feature)
        {
            features.Add(feature);
            // 更新画面
        }

        /// <summary>
        /// 删除特性
        /// </summary>
        /// <param name="feature">要删除的特性</param>
        public void RemoveFeature(FeatureEnum feature)
        {
            features.Remove(feature);
            // 更新画面
        }

        /// <summary>
        /// 检查是否有某个特性
        /// </summary>
        /// <param name="featureName">特性名字</param>
        /// <returns></returns>
        public bool HasFeature(FeatureEnum fea)
        {
            if (features.Contains(fea)) return true;
            return false;
        }

        /// <summary>
        /// 删除所有特性
        /// </summary>
        public void RemoveAllFeatures()
        {
            foreach (FeatureEnum feature in features) RemoveFeature(feature);
        }

        /// <summary>
        /// 设置卡牌的名称
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            charaName = name;
        }

        /// <summary>
        /// 卡牌强化后更改名称
        /// </summary>
        /// <param name="previousEnhancementLevel">卡牌原强化等级</param>
        public void SetNameAfterEnhancement(int previousEnhancementLevel)
        {
            if (previousEnhancementLevel == 0)
            {
                charaName += " (+1)";
            }
            else
            {
                charaName = charaName.Substring(0, charaName.Length - 5) + " (+" + (previousEnhancementLevel + 1) + ")";
            }
        }

        /// <summary>
        /// 设置卡牌的强化等级
        /// </summary>
        /// <param name="value"></param>
        public void SetEnhancement(int value)
        {
            enhancement = value;
        }

        /// <summary>
        /// 更改卡牌的攻击力
        /// </summary>
        /// <param name="value"></param>
        public void AddDamage(int value)
        {
            damage += value;
        }

        /// <summary>
        /// 更改卡牌的防御力
        /// </summary>
        /// <param name="value"></param>
        public void AddDefense(int value)
        {
            defend += value;
        }

        /// <summary>
        /// 更改卡牌的移动速度，最高不超过2
        /// </summary>
        /// <param name="value"></param>
        public void AddMoveSpeed(float value)
        {
            moveSpd -= value;
            if (moveSpd < 2) moveSpd = 2;
        }
        
        /// <summary>
        /// 更改卡牌的寿命
        /// </summary>
        /// <param name="value"></param>
        public void AddLife(float value)
        {
            maxLife += value;
        }


        public void PrintData()
        {
            string ret = String.Empty;
            ret += $"characterID: {charaID}\n" + $"instID: {instID}\n" + $"characterName: {charaName}\n" +
                   $"deathFuncDescription: {deathFuncDesc}\n" + $"rarity: {rarity}\n" + $"sanCost: {sanCost}\n";
            ret += "feature: ";
            if (features.Count > 0)
            {
                foreach (var featureEnum in features)
                {
                    ret += $"{featureEnum} ";
                }
            }
            ret += "\nspecial feature: ";

            if (specialFeature != null)
                foreach (var sf in specialFeature)
                {
                    ret += $"{sf} ";
                }

            Debug.Log(ret);
        }

        public object Clone()
        {
            Card newCard = (Card) MemberwiseClone();
            // newCard.so = so;
            return newCard;
        }
    }
}