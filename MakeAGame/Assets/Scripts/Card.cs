using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 存放卡牌数据（而不是实际表现）
    /// </summary>
    public class Card
    {
        public int charaID { get; } // 角色id
        private int instID = -1;    // 卡牌实体id，可能根据该卡牌位于背包中还是手牌中，分别由背包和手牌系统管理    // todo 可能是没用的，暂时不要给它赋值

        // 应该不会变动的数据，就不另外存储了，直接用so
        // name, death desc, sprite, feature, special feature
        private SOCharacterInfo so;
        public string charaName => so.characterName;
        public string deathFuncDesc => so.deathFuncDescription;
        public Sprite cardSprite => so.cardSprite;
        public Sprite pieceSprite => so.pieceSprite;
        public SOFeature[] features => so.features;
        public SOFeature specialFeature => so.specialFeature;

        // 会有改动的数据  // 外部可以读取，但不可以直接改动
        public int rarity { get; private set; }
        public float sanCost { get; private set; }
        public int hp { get; private set; }
        public float moveSpd { get; private set; }
        public int damage { get; private set; }
        public int defend { get; private set; }

        public Card(int _charaID)
        {
            charaID = _charaID;
            InitData();
        }

        void InitData()
        {
            so = Extensions.GetCharacterInfo(charaID, false);
            rarity = so.rarity;
            sanCost = so.sanCost;
            hp = so.hp;
            moveSpd = so.moveSpd;
            damage = so.attack;
            defend = so.defend;
            PrintData();
        }

        void Func()
        {
            // todo 获取初始数值后，根据各种影响获得这张卡的最终数值
        }

        void PrintData()
        {
            string ret = String.Empty;
            ret += $"characterID: {charaID}\n" + $"instID: {instID}\n" + $"characterName: {charaName}\n" +
                   $"deathFuncDescription: {deathFuncDesc}\n" + $"rarity: {rarity}\n" + $"sanCost: {sanCost}\n";
            ret += "feature: ";
            if (features.Length > 0)
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
    }
}