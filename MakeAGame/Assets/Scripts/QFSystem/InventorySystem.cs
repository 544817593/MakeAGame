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
        /// 返回背包里的物品列表
        /// </summary>
        List<Item> GetItemList();

        /// <summary>
        /// 创建一个卡牌实体Prefab，并放入背包内
        /// </summary>
        /// <param name="cardId">卡牌id</param>
        public void SpawnCardInBag(int cardId);
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

            SOItemBase testItem = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31");

            AddItem(new Item { amount = 1, data = testItem });
            AddItem(new Item { amount = 1, data = testItem });
            AddItem(new Item { amount = 1, data = testItem });


        }

        private void OnItemListChanged()
        {
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
    }
}

