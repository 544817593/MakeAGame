using QFramework;
using QFramework.PointGame;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public interface IShopSystem : ISystem
    {
        List<Item> getshopItemList();
        List<Item> getBagItemList();
        List<ViewCard> getBagCardList();
        /// <summary>
        /// 给商店物品列表添加商品
        /// </summary>
        /// <param name="item"></param>
        void addShopItem(Item item);

        /// <summary>
        /// 给背包物品列表添加物品
        /// </summary>
        /// <param name="item"></param>
        void addBagItem(Item item);

        /// <summary>
        /// 给背包添加卡牌
        /// </summary>
        /// <param name="card"></param>
        void addBagCard(ViewCard card);
    }
    public class ShopSystem : AbstractSystem, IShopSystem
    {
        public BindableProperty<List<Item>> shopItemList = new BindableProperty<List<Item>>(); // 商品列表
        // TODO: 接入背包后直接使用背包中的list，当前临时使用
        public BindableProperty<List<Item>> bagItemList = new BindableProperty<List<Item>>(); // 背包物品列表
        public BindableProperty<List<ViewCard>> bagCardList = new BindableProperty<List<ViewCard>>(); // 背包卡牌列表
        protected override void OnInit()
        {
            shopItemList.SetValueWithoutEvent(new List<Item>());
            shopItemList.Register((newList) => OnShopItemListChanged());

            bagItemList.SetValueWithoutEvent(new List<Item>());

            bagCardList.SetValueWithoutEvent(new List<ViewCard>());

            addShopItemWithoutCall(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31") });
            addShopItemWithoutCall(new Item { amount = 2, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01") });
            addShopItemWithoutCall(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31") });
            addShopItemWithoutCall(new Item { amount = 4, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01") });


            //for (int i = 1; i < 32; ++i)
            //{
            //    addBagItem(new Item { amount = 1, data = Resources.Load<SOItemBase>($"ScriptableObjects/Items/Item01 {i}") });
            //}

            // 如果打开的是强化页面才执行
            if (GameObject.Find("BagPanel") != null)
            {
                for (int i = 1; i <= 10; ++i)
                {
                    GameObject cardGO = this.GetSystem<ICardGeneratorSystem>().CreateCard();
                    ViewCard viewCard = cardGO.AddComponent<ViewCard>();
                    // 接收数据，初始化牌面显示
                    viewCard.card = new Card(1);
                    viewCard.card.enhanceID = i;
                    bagCardList.Value.Add(viewCard);
                    
                }
            }
        }

        private void OnShopItemListChanged()
        {
            UIKit.GetPanel("ShopBuyUI")?.Invoke("updateAndShowShopItems", 0f); 
        }

        public List<Item> getshopItemList()
        {
            return shopItemList.Value;
        }

        public List<Item> getBagItemList()
        {
            return bagItemList.Value;
        }
        public List<ViewCard> getBagCardList()
        {
            return bagCardList.Value;
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
                if (shopItemList.Value[i].data.itemName == item.data.itemName)
                {
                    // 直接Add，不会触发调用OnShopItemListChanged()
                    shopItemList.Value[i].amount += item.amount;
                    return;
                }
            }
            shopItemList.Value.Add(item);
        }

        
        public void addBagItem(Item item)
        {
            for (int i = 0; i < bagItemList.Value.Count; i++)
            {
                // 使用unity中物体的name，仅方便用于测试，之后改成itemName
                if (bagItemList.Value[i].data.name == item.data.name)
                {
                    bagItemList.Value[i].amount += item.amount;
                    return;
                }
            }
            bagItemList.Value.Add(item);
        }

        public void addBagCard(ViewCard card)
        {
            bagCardList.Value.Add(card);
        }
    }
}

