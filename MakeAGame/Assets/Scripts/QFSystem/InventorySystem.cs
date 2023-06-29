using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using BagUI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace Game
{
    public interface IInventorySystem : ISystem
    {
        /// <summary>
        /// 添加物品到背包里
        /// </summary>
        /// <param name="item">要添加的物品</param>
        void AddItem(Item item);

        /// <summary>
        /// 从背包里删除一个物品
        /// </summary>
        /// <param name="item"></param>
        void RemoveItem(Item item);

        /// <summary>
        /// 使用一个物品
        /// </summary>
        /// <param name="item"></param>
        void UseItem(Item item);

        /// <summary>
        /// 返回背包里的物品列表
        /// </summary>
        List<Item> GetItemList();


        // 目前SpawnSystem.SpawnCard返还的是viewBagCard不是viewCard，不能放入手牌，暂时不确定是否需要这个函数
        /// <summary>
        /// 创建一个卡牌实体Prefab，并放入背包内
        /// </summary>
        /// <param name="cardId">卡牌id</param>
        //void SpawnHandCard(int cardId);


        /// <summary>
        /// 创建一个卡牌实体Prefab，并放入背包中
        /// </summary>
        /// <param name="m_card">卡牌</param>
        void SpawnBagCardInBag(Card m_card);

        /// <summary>
        /// 返回背包里的卡牌ViewBagCard列表
        /// </summary>
        /// <returns></returns>
        List<ViewBagCard> GetBagCardList();

        /// <summary>
        /// 整理itemList也就是背包道具的顺序，优先级为：战斗中使用道具>任何时间使用的道具>其它可使用道具>不可使用道具。
        /// 同一道具类型下稀有度低的在前，稀有度高的在后。每次背包道具有变动自动调用。
        /// </summary>
        void SortItemList();

        /// <summary>
        /// Knuth-Durstenfeld Shuffle算法，给背包中的牌随机排序
        /// </summary>
        void ShuffleCard();

        /// <summary>
        /// 从背包里抽取一张牌，返还抽到的卡牌数据
        /// </summary>
        /// <returns></returns>
        Card DrawCard();

        /// <summary>
        /// 在玩家属性改变后更新卡牌数据
        /// </summary>
        void UpdateCardStats(PlayerStatsEnum stat, int value);

    }

    public class InventorySystem : AbstractSystem, IInventorySystem
    {
        public BindableProperty<List<Item>> itemList = new BindableProperty<List<Item>>(); // 物品列表
        private BindableProperty<List<ViewBagCard>> cardBagList = new BindableProperty<List<ViewBagCard>>();// 背包卡牌列表
        public Transform inventoryRoot; // 生成的物品Prefab悬挂的父物体位置

        protected override void OnInit()
        {
            itemList.SetValueWithoutEvent(new List<Item>());
            itemList.Register((newItemList) => OnItemListChanged());
            inventoryRoot = GameObject.Find("InventoryRoot")?.transform;

            cardBagList.SetValueWithoutEvent(new List<ViewBagCard>());

            UIKit.OpenPanel<BagUIPanel>();
            UIKit.HidePanel<BagUIPanel>();

            // 测试用代码
            SOItemBase IntermediateSapphirePotion = Resources.Load<SOItemBase>("ScriptableObjects/Items/Intermediate Sapphire Potion");
            SOItemBase MinorSapphirePotion = Resources.Load<SOItemBase>("ScriptableObjects/Items/Minor Sapphire Potion");
            SOItemBase MinorEmeraldPotion = Resources.Load<SOItemBase>("ScriptableObjects/Items/Minor Emerald Potion");
            AddItem(new Item { amount = 3, data = MinorSapphirePotion });
            AddItem(new Item { amount = 1, data = IntermediateSapphirePotion });
            AddItem(new Item { amount = 1, data = MinorEmeraldPotion });





        }



        private void OnItemListChanged()
        {
            SortItemList();
            UIKit.GetPanel("UIInventoryQuickSlot")?.Invoke("RefreshInventoryItems", 0f);
        }

        public void AddItem(Item item)
        {
            // 如果已经有物品，那么则数量+1
            for (int i = 0; i < itemList.Value.Count; i++)
            {
                if (itemList.Value[i].data.name == item.data.name)
                {
                    // 因为是双层数据类型，需要重新给itemList.Value赋值，不然不会调用OnItemListChanged()
                    List<Item> tempList = new List<Item>(itemList.Value);
                    tempList[i].amount += item.amount;
                    itemList.Value = tempList;
                    return;
                }
               
            }
            List<Item> newList = new List<Item>(itemList.Value);
            newList.Add(item);
            itemList.Value = newList;
        }

        public void RemoveItem(Item item)
        {
            List<Item> newList = new List<Item>(itemList.Value);
            newList.Remove(item);
            itemList.Value = newList;
        }

        public List<Item> GetItemList()
        {
            return itemList;
        }


        public void SpawnBagCardInBag(Card m_card)
        {
            GameObject card_Object;
            ViewBagCard cardItem;
            ICardGeneratorSystem CardSystem = this.GetSystem<ICardGeneratorSystem>();
            cardItem = CardSystem.CreateBagCard(m_card);
            card_Object = cardItem.gameObject;
            cardItem.OnFocusAction = () =>
                {
                    UIKit.GetPanel<BagUIPanel>().CardDescription.Show();

                    UIKit.GetPanel<BagUIPanel>().CardDescription.GetComponent<LoadCardDetail>()?.ShowDetail(cardItem.card);
                };
            cardItem.OnUnFocusAction = () =>
                {
                    UIKit.GetPanel<BagUIPanel>().CardDescription.Hide();
                };
   
            cardBagList.Value.Add(cardItem);
           // Debug.LogError(cardBagList.Value.Count);
            //ShuffleCard();
            UIKit.GetPanel<BagUIPanel>().RefreshLayout();
        }
        public List<ViewBagCard> GetBagCardList()
        {
            return cardBagList.Value;
        }

        public void SortItemList()
        {
            List<Item> items = itemList.Value;

            items.Sort((item1, item2) =>
            {
                // 比较道具使用时间
                int useTimeComparison = item1.data.itemUseTime.CompareTo(item2.data.itemUseTime);
                if (useTimeComparison != 0)
                {
                    return useTimeComparison;
                }

                // 如果道具使用时间相同，则比较稀有度
                int rarityComparison = item1.data.rarity.CompareTo(item2.data.rarity);
                return rarityComparison;
            });

            itemList.SetValueWithoutEvent(items);
        }

        public void UseItem(Item item)
        {
            var useItemEvent = new UseItemEvent { item = item };
            GameEntry.Interface.SendCommand(new UseItemCommand(useItemEvent));
            GameManager.Instance.soundMan.Play_itemUse_sound();
        }

        public void ShuffleCard()
        {
            for (int i = 0; i < cardBagList.Value.Count; i++)
            {
                var index = Random.Range(0, cardBagList.Value.Count - i);
                var tempValue = cardBagList.Value[index];
                cardBagList.Value[index] = cardBagList.Value[cardBagList.Value.Count - i - 1];
                cardBagList.Value[cardBagList.Value.Count - i - 1] = tempValue;
            }
        }

        public Card DrawCard()
        {
            var index = cardBagList.Value.Count - 1;
            var card = (Card)cardBagList.Value[index].card.Clone();
            cardBagList.Value.RemoveAt(index);
            return card;
        }

        public void UpdateCardStats(PlayerStatsEnum stat, int value)
        {

            foreach (ViewBagCard viewBagCard in cardBagList.Value)
            {
                SOCharacterInfo so = IdToSO.FindCardSOByID(viewBagCard.card.charaID);
                if (stat == so.atkSpdBonus.stat)
                {
                    viewBagCard.card.atkSpd += value * so.atkSpdBonus.multiple;
                }
                if (stat == so.atkBonus.stat)
                {
                    viewBagCard.card.damage += value * so.atkBonus.multiple;
                }
                if (stat == so.hpBonus.stat)
                {
                    viewBagCard.card.hp += (int)(value * so.hpBonus.multiple);
                    viewBagCard.card.maxHp += (int)(value * so.hpBonus.multiple);
                }
                viewBagCard.card.sanCost += value * so.sanCostBonus.multiple;
            }
        }
    }
}

