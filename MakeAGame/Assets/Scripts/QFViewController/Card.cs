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
        public SOFeature specialFeature { get;  set; }
        public int width { get;  set; }
        public int height { get;  set; }
        public DirEnum[] moveDirections { get;  set; }
        public int atkRange { get;  set; }
        public float atkSpd { get;  set; }

        // 会有改动的数据  // 外部可以读取，但不可以直接改动
        public int rarity { get;  set; }
        public float sanCost { get;  set; }
        public int hp { get;  set; }
        public float moveSpd { get;  set; }
        public int damage { get;  set; }
        public int defend { get;  set; }
        public int enhancement { get;  set; }
        public string charaName { get;  set; }
        public float maxLife { get;  set; }
        public float currLife { get;  set; }
        public List<SOFeature> features { get;  set; }

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
            specialFeature = so.specialFeature;
            width = so.width;
            height = so.height;
            moveDirections = so.moveDirections;
            atkRange = so.attackRange;
            atkSpd = so.attackSpd;
            rarity = so.rarity;
            sanCost = so.sanCost;
            hp = so.hp;
            moveSpd = so.moveSpd;
            damage = so.attack;
            defend = so.defend;
            charaName = so.characterName;
            maxLife = so.life;
            currLife = so.life;
            features = so.features;
            PrintData();
        }

        void Func()
        {
            // todo 获取初始数值后，根据各种影响获得这张卡的最终数值
        }

        /// <summary>
        /// 添加特性
        /// </summary>
        /// <param name="feature">要添加的特性</param>
        public void AddFeature(SOFeature feature)
        {
            features.Add(feature);
            // 更新画面
        }

        /// <summary>
        /// 删除特性
        /// </summary>
        /// <param name="feature">要删除的特性</param>
        public void RemoveFeature(SOFeature feature)
        {
            features.Remove(feature);
            // 更新画面
        }

        /// <summary>
        /// 检查是否有某个特性
        /// </summary>
        /// <param name="featureId">特性ID</param>
        /// <returns></returns>
        public bool HasFeature(int featureId)
        {
            foreach (SOFeature feature in features)
            {
                if (feature.featureID == featureId) return true;
            }
            return false;
        }

        /// <summary>
        /// 删除所有特性
        /// </summary>
        public void RemoveAllFeatures()
        {
            foreach ( SOFeature feature in features) RemoveFeature(feature);
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

        /// <summary>
        /// 更改当前寿命，只有*棋子*才会调用
        /// </summary>
        /// <param name="value"></param>
        public void AddCurrLife(float value)
        {
            currLife += value;
        }

        public void PrintData()
        {
            string ret = String.Empty;
            ret += $"characterID: {charaID}\n" + $"instID: {instID}\n" + $"characterName: {charaName}\n" +
                   $"deathFuncDescription: {deathFuncDesc}\n" + $"rarity: {rarity}\n" + $"sanCost: {sanCost}\n";
            ret += "feature: ";
            if (features.Count > 0)
            {
                foreach (var soFeature in features)
                {
                    ret += $"{soFeature.featureName} ";
                }
            }
            ret += "\nspecial feature: ";
            if (specialFeature != null)
                ret += specialFeature.featureName;
            
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