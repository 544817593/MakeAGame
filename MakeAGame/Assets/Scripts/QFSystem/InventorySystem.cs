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
        public BindableProperty<List<Item>> itemList; // 物品列表
        public BindableProperty<List<Card>> cardList; // 卡牌列表
        public Transform inventoryRoot; // 生成的物品Prefab悬挂的父物体位置

        protected override void OnInit()
        {
            itemList = new BindableProperty<List<Item>>(new List<Item>());
            itemList.Register(newItemList => OnItemListChanged(newItemList));
            inventoryRoot = GameObject.Find("InventoryRoot").transform;

            SOItemBase testItem = AssetDatabase.LoadAssetAtPath<SOItemBase>("Assets/Resources/ScriptableObjects/Items/ChaosPotion.asset");
            SOItemBase testItem2 = AssetDatabase.LoadAssetAtPath<SOItemBase>("Assets/Resources/ScriptableObjects/Items/ChaosPotion2.asset");

            AddItem(new Item { amount = 1, data = testItem });
            AddItem(new Item { amount = 1, data = testItem2 });

        }

        private void OnItemListChanged(List<Item> newItemList)
        {
            UIKit.GetPanel("UIInventoryQuickSlot").Invoke("RefreshInventoryItems", 0f);
        }

        public void AddItem(Item item)
        {
            itemList.Value.Add(item);
        }

        public List<Item> GetItemList()
        {
            Debug.LogWarning("itemList is null: " + (itemList == null));
            Debug.LogWarning("itemList value is null: " + (itemList.Value == null));

            return itemList;
        }

        public void SpawnCardInBag(int cardId)
        {
            GameObject cardItem;
            ISpawnSystem spawnSystem = this.GetSystem<ISpawnSystem>();
            spawnSystem.SpawnCard(cardId);
            cardItem = spawnSystem.GetLastSpawnedCard();
            var cardBase = cardItem.GetComponent<Card>();
            cardList.Value.Add(cardBase);
        }
    }
}

