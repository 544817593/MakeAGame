using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        /// <summary>
        /// 创建一个卡牌实体Prefab，并放入背包内
        /// </summary>
        /// <param name="cardId">卡牌id</param>
        void SpawnCardInBag(int cardId);

        /// <summary>
        /// 返回背包里的卡牌列表
        /// </summary>
        /// <returns></returns>
        List<ViewCard> GetCardList();

        /// <summary>
        /// 整理itemList也就是背包道具的顺序，优先级为：战斗中使用道具>任何时间使用的道具>其它可使用道具>不可使用道具。
        /// 同一道具类型下稀有度低的在前，稀有度高的在后。每次背包道具有变动自动调用。
        /// </summary>
        void SortItemList();



    }

    public class InventorySystem : AbstractSystem, IInventorySystem
    {
        public BindableProperty<List<Item>> itemList = new BindableProperty<List<Item>>(); // 物品列表
        public BindableProperty<List<ViewCard>> cardList = new BindableProperty<List<ViewCard>>(); // 卡牌列表
        public Transform inventoryRoot; // 生成的物品Prefab悬挂的父物体位置

        protected override void OnInit()
        {
            itemList.SetValueWithoutEvent(new List<Item>());
            itemList.Register((newItemList) => OnItemListChanged());
            inventoryRoot = GameObject.Find("InventoryRoot")?.transform;

            cardList.SetValueWithoutEvent(new List<ViewCard>());

            // 测试用代码
            SOItemBase IntermediateSapphirePotion = Resources.Load<SOItemBase>("ScriptableObjects/Items/Intermediate Sapphire Potion");
            SOItemBase MinorSapphirePotion = Resources.Load<SOItemBase>("ScriptableObjects/Items/Minor Sapphire Potion");
            SOItemBase IntermediateEmeraldPotion = Resources.Load<SOItemBase>("ScriptableObjects/Items/Intermediate Emerald Potion");
            SOItemBase Type_DMinorEnhancementPotion = Resources.Load<SOItemBase>("ScriptableObjects/Items/Type-D Minor Enhancement Potion");
            AddItem(new Item { amount = 1, data = IntermediateSapphirePotion });
            AddItem(new Item { amount = 2, data = MinorSapphirePotion });
            AddItem(new Item { amount = 1, data = MinorSapphirePotion });
            AddItem(new Item { amount = 3, data = IntermediateEmeraldPotion });
            AddItem(new Item { amount = 1, data = Type_DMinorEnhancementPotion });




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

        public void SpawnCardInBag(int cardId)
        {
            GameObject cardItem;
            ISpawnSystem spawnSystem = this.GetSystem<ISpawnSystem>();
            spawnSystem.SpawnCard(cardId);
            cardItem = spawnSystem.GetLastSpawnedCard();
            var cardBase = cardItem.GetComponent<ViewCard>();
            cardList.Value.Add(cardBase);
        }

        public List<ViewCard> GetCardList()
        {
            return cardList;
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
        }
    }
}

