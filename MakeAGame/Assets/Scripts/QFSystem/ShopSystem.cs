using QFramework;
using QFramework.PointGame;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public interface IShopSystem : ISystem
    {
        List<Item> GetShopItemList();

        List<Item> GetEnhanceBagItemList();

        List<Item> GetAllBagItemList();

        List<ViewBagCard> GetBagCardList();

        /// <summary>
        /// 第二次及之后进入商店要重新初始化商店
        /// </summary>
        public void resetSystem();

        /// <summary>
        /// 给商店物品列表添加商品
        /// </summary>
        /// <param name="item"></param>
        void AddShopItemWithoutCall(Item item);

        /// <summary>
        /// 给背包物品列表添加物品
        /// </summary>
        /// <param name="item"></param>
        void AddEnhanceBagItem(Item item);

        /// <summary>
        /// 给背包添加卡牌
        /// </summary>
        /// <param name="card"></param>
        void AddBagCard(ViewBagCard card);

        /// <summary>
        /// 每次商店出售物品，强化卡牌消耗物品后，如果物品数量为0就把所有list中对应物品remove
        /// 包括真实背包中的list，enhanceBagItemList，allBagItemList
        /// </summary>
        /// <param name="item"></param>
        void RemoveItemInAllList(Item item);

    }
    public class ShopSystem : AbstractSystem, IShopSystem
    {
        public BindableProperty<List<Item>> shopItemList = new BindableProperty<List<Item>>(); // 可购买的商品列表
        public BindableProperty<List<Item>> enhanceBagItemList = new BindableProperty<List<Item>>(); // 可用在强化页面的物品列表
        public BindableProperty<List<Item>> allBagItemList = new BindableProperty<List<Item>>(); // 所有背包中的物品列表
        public BindableProperty<List<ViewBagCard>> bagCardList = new BindableProperty<List<ViewBagCard>>(); // 背包卡牌列表
        public List<SOItemBase> selectedSOItemList = new List<SOItemBase>();
        public static int enterRoomTime = 0;
        protected override void OnInit()
        {
            shopItemList.SetValueWithoutEvent(new List<Item>());
            enhanceBagItemList.SetValueWithoutEvent(new List<Item>());
            allBagItemList.SetValueWithoutEvent(new List<Item>());
            bagCardList.SetValueWithoutEvent(new List<ViewBagCard>());
            
            foreach (SOItemBase item in IdToSO.soItemList)
            {
                if (item.rarity == 0 || item.rarity == 1)
                {
                    selectedSOItemList.Add(item);
                }
            }

            // 陈列出售列表固定加入卡包和纯白气息
            AddShopItemWithoutCall(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/CardPack") });
            AddShopItemWithoutCall(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Pure White") });
            // 洗牌算法，从所有白色和绿色物品中选择wantedItemsNum个陈列出售
            int wantedItemsNum = 10; // 一共想要陈列的物品数量
            List<int> numbers = Enumerable.Range(0, selectedSOItemList.Count - 1).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                int temp = numbers[i];
                int randomIndex = UnityEngine.Random.Range(i, numbers.Count);
                numbers[i] = numbers[randomIndex];
                numbers[randomIndex] = temp;
            }
            for (int i = 0; i < wantedItemsNum; i++)
            {
                AddShopItemWithoutCall(new Item { amount = 1, data = selectedSOItemList[i]});
            }
            // 能在商店使用的物品加到list中
            foreach (var item in this.GetSystem<IInventorySystem>().GetItemList())
            {
                if (item.data.itemUseTime == ItemUseTimeEnum.Merchant)
                {
                    AddEnhanceBagItem(item);
                }
            }
            bagCardList.Value = this.GetSystem<IInventorySystem>().GetBagCardList();
            allBagItemList.Value = this.GetSystem<IInventorySystem>().GetItemList();
            // 以下都是测试使用的初始化数据
            //AddShopItemWithoutCall(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31") });
            //AddShopItemWithoutCall(new Item { amount = 2, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01") });
            //AddShopItemWithoutCall(new Item { amount = 1, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item31") });
            //AddShopItemWithoutCall(new Item { amount = 4, data = Resources.Load<SOItemBase>("ScriptableObjects/Items/Item01") });

            //AddBagItem(new Item { amount = 2, data = Resources.Load<SOItemBase>($"ScriptableObjects/Items/Item31") });
            //for (int i = 1; i <= 32; ++i)
            //{
            //    AddBagItem(new Item { amount = 1, data = Resources.Load<SOItemBase>($"ScriptableObjects/Items/Item01") });
            //}
            // bagCardList在ShopEnhanceUI.cs中初始化，因为ShopSystem在点购买和出售的时候也会执行，不符合需要
        }
        public void resetSystem()
        {
            shopItemList.Value.Clear();
            // 这里不能clearbagCard，会把真实背包中的牌也清空
            enhanceBagItemList.Value.Clear();
            selectedSOItemList.Clear();

            foreach (SOItemBase item in IdToSO.soItemList)
            {
                if (item.rarity == 0 || item.rarity == 1)
                {
                    selectedSOItemList.Add(item);
                }
            }
            // 洗牌算法，打乱列表后选择RandMonsterCount个数作为monster列表中的index
            // 创建一个包含0, 1, 2,..., pieceSystem.pieceEnemyList.Count - 1的list
            // 从头开始，每轮循环从[当前index, 总长度)的范围内随机选1个index, 将随机选择的index位置的值和当前位置的值互换，进行下一轮循环
            int wantedItemsNum = 10; // 一共想要陈列的物品数量
            List<int> numbers = Enumerable.Range(0, selectedSOItemList.Count - 1).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                int temp = numbers[i];
                int randomIndex = UnityEngine.Random.Range(i, numbers.Count);
                numbers[i] = numbers[randomIndex];
                numbers[randomIndex] = temp;
            }
            for (int i = 0; i < wantedItemsNum; i++)
            {
                AddShopItemWithoutCall(new Item { amount = 1, data = selectedSOItemList[i] });
            }
            // 能在商店使用的物品加到list中
            foreach (var item in this.GetSystem<IInventorySystem>().GetItemList())
            {
                if (item.data.itemUseTime == ItemUseTimeEnum.Merchant)
                {
                    AddEnhanceBagItem(item);
                }
            }
            bagCardList.Value = this.GetSystem<IInventorySystem>().GetBagCardList();
            allBagItemList.Value = this.GetSystem<IInventorySystem>().GetItemList();
        }
        public List<Item> GetShopItemList()
        {
            return shopItemList.Value;
        }

        public List<Item> GetEnhanceBagItemList()
        {
            return enhanceBagItemList.Value;
        }
        public List<ViewBagCard> GetBagCardList()
        {
            return bagCardList.Value;
        }
        public List<Item> GetAllBagItemList()
        {
            return allBagItemList.Value;
        }

        public void AddShopItemWithoutCall(Item item)
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

        
        public void AddEnhanceBagItem(Item item)
        {
            for (int i = 0; i < enhanceBagItemList.Value.Count; i++)
            {
                // 使用unity中物体的name，仅方便用于测试，之后改成itemName
                if (enhanceBagItemList.Value[i].data.itemName == item.data.itemName)
                {
                    enhanceBagItemList.Value[i].amount += item.amount;
                    return;
                }
            }
            enhanceBagItemList.Value.Add(item);
        }

        public void AddBagCard(ViewBagCard card)
        {
            bagCardList.Value.Add(card);
        }

        public void RemoveItemInAllList(Item item)
        {
            enhanceBagItemList.Value.Remove(item);
            allBagItemList.Value.Remove(item);
            this.GetSystem<IInventorySystem>().GetItemList().Remove(item);
        }
        

    }
}

