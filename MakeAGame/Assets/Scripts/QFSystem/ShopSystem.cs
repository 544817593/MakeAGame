using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public interface IShopSystem : ISystem
    {
        /// <summary>
        /// 给商店物品列表增添商品
        /// </summary>
        /// <param name="item"></param>
        void addShopItem(Item item);

        /// <summary>
        /// 获取商店物品列表
        /// </summary>
        /// <returns></returns>
        List<Item> getshopItemList();
    }
    public class ShopSystem : AbstractSystem, IShopSystem
    {
        public BindableProperty<List<Item>> shopItemList = new BindableProperty<List<Item>>(); // 物品列表
        protected override void OnInit()
        {
            shopItemList.SetValueWithoutEvent(new List<Item>());
            shopItemList.Register((newList) => OnShopItemListChanged());

            //shopItemList.Value.Add(new Item { amount = 10, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31")});
            //shopItemList.Value.Add(new Item { amount = 2, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01")});
            //shopItemList.Value.Add(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31")});
            //shopItemList.Value.Add(new Item { amount = 4, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01")});

            addShopItemWithoutCall(new Item { amount = 10, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31") });
            addShopItemWithoutCall(new Item { amount = 2, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01") });
            addShopItemWithoutCall(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31") });
            addShopItemWithoutCall(new Item { amount = 4, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01") });


            //addShopItem(new Item { amount = 10, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31")});
            //addShopItem(new Item { amount = 2, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01")});
            //addShopItem(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31")});
            //addShopItem(new Item { amount = 4, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01")});
        }

        private void OnShopItemListChanged()
        {
            UIKit.GetPanel("ShopBuyUI")?.Invoke("updateAndShowShopItems", 0f); 
        }

        public List<Item> getshopItemList()
        {
            return shopItemList.Value;
        }
        public void addShopItem(Item item)
        {
            for (int i = 0; i < shopItemList.Value.Count; i++)
            {
                // 如果物品已存在shopItemList中，amount叠加
                if (shopItemList.Value[i].data.name == item.data.name)
                {
                    // 因为是双层数据类型，需要重新给shopItemList.Value赋值，不然不会调用updateAndShowShopItems()
                    List<Item> tempList = new List<Item>(shopItemList.Value);
                    tempList[i].amount += item.amount;
                    shopItemList.Value = tempList;
                    return;
                }
            }
            List<Item> newList = new List<Item>(shopItemList.Value);
            newList.Add(item);
            shopItemList.Value = newList;
        }

        public void addShopItemWithoutCall(Item item)
        {
            for (int i = 0; i < shopItemList.Value.Count; i++)
            {
                // 如果物品已存在shopItemList中，amount叠加
                if (shopItemList.Value[i].data.name == item.data.name)
                {
                    // 直接Add，不会触发调用OnShopItemListChanged()
                    shopItemList.Value[i].amount += item.amount;
                    return;
                }
            }
            shopItemList.Value.Add(item);
        }

    }
}

