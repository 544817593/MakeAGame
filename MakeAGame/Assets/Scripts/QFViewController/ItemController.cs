using DG.Tweening;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemController : MonoBehaviour, IController
    {
        private PlayerManager playerManager;
        private IInventorySystem inventorySystem;
        private IShopSystem shopSystem;

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }

        void Awake()
        {          
            this.RegisterEvent<UseItemEvent>(e => OnUseItemEvent(e));
        }

        void Start()
        {
            playerManager = GameManager.Instance.playerMan;
            inventorySystem = this.GetSystem<IInventorySystem>();
            shopSystem = this.GetSystem<IShopSystem>();
        }

        /// <summary>
        /// 收到使用物品事件后处理
        /// </summary>
        /// <param name="e">使用物品事件所需的Struct参数</param>
        private void OnUseItemEvent(UseItemEvent e)
        {
            if (e.item.data.itemUseTime == ItemUseTimeEnum.Combat)
            {
                OnUseCombatItem(e);
            }
            else if (e.item.data.itemUseTime == ItemUseTimeEnum.AnyTime)
            {
                OnUseAnyTimeItem(e);
            }
            else if (e.item.data.itemUseTime == ItemUseTimeEnum.Merchant)
            {
                OnUseMerchantItem(e);
            }
        }

        /// <summary>
        /// 使用在商店使用的物品
        /// </summary>
        public void OnUseMerchantItem(UseItemEvent e)
        {
            Item item = e.item;
            ViewBagCard viewBagCard = e.viewBagCard;
            int rand; // 随机数，用来计算强化成功与否
            switch (item.data.itemName)
            {
                case "A-型初级强化药剂":
                    viewBagCard.card.AddDamage(2);
                    break;
                case "B-型初级强化药剂":
                    viewBagCard.card.AddDefense(1);
                    break;
                case "C-型初级强化药剂":
                    viewBagCard.card.AddMoveSpeed(0.5f);
                    break;
                case "D-型初级强化药剂":
                    viewBagCard.card.AddLife(5f);
                    break;
                case "E-型初级强化药剂":
                    break;
                case "F-型初级强化药剂":
                    break;
                case "G-型初级强化药剂":
                    break;
                case "A-型中级强化药剂":
                    rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 0)
                    {
                        viewBagCard.card.AddDamage(2);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "B-型中级强化药剂":
                    rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 0)
                    {
                        viewBagCard.card.AddDefense(1);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "C-型中级强化药剂":
                    rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 0)
                    {
                        viewBagCard.card.AddMoveSpeed(0.5f);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "D-型中级强化药剂":
                    rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 0)
                    {
                        viewBagCard.card.AddLife(5f);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "E-型中级强化药剂":
                    break;
                case "F-型中级强化药剂":
                    break;
                case "G-型中级强化药剂":
                    break;
                case "A-型腐朽的锤子":
                    break;
                case "B-型腐朽的锤子":
                    break;
                case "C-型腐朽的锤子":
                    break;
                case "A-型高级强化药剂":
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand > 14)
                    {
                        viewBagCard.card.AddDamage(4);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "B-型高级强化药剂":
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand > 14)
                    {
                        viewBagCard.card.AddDefense(2);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "C-型高级强化药剂":
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand > 14)
                    {
                        viewBagCard.card.AddMoveSpeed(1f);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "D-型高级强化药剂":
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand > 14)
                    {
                        viewBagCard.card.AddLife(8f);
                        break;
                    }
                    AfterUseMerchantItem(item, viewBagCard, false);
                    return;
                case "E-型高级强化药剂":
                    break;
                case "F-型高级强化药剂":
                    break;
                case "G-型高级强化药剂":
                    break;
                case "A-型破旧的锤子":
                    break;
                case "B-型破旧的锤子":
                    break;
                case "C-型破旧的锤子":
                    break;
                case "D-型破旧的锤子":
                    break;
                case "木制起钉器":
                    if (GameManager.Instance.gameSceneMan.GetCurrentSceneName() != "Merchant") return;
                    viewBagCard.card.RemoveFeature(e.soFeature);
                    viewBagCard.InitView();
                    break;
                case "铁质起钉器":
                    if (GameManager.Instance.gameSceneMan.GetCurrentSceneName() != "Merchant") return;
                    viewBagCard.card.RemoveAllFeatures();
                    viewBagCard.card.SetEnhancement(0);
                    string initialCardName = IdToSO.FindCardSOByID(viewBagCard.card.charaID).characterName;
                    viewBagCard.card.SetName(initialCardName);
                    viewBagCard.InitView();
                    break;
            }
            AfterUseMerchantItem(item, viewBagCard);
        }

        /// <summary>
        /// 使用商店可用的物品后处理
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="viewCard">卡牌</param>
        /// <param name="successful">强化成功与否</param>
        private void AfterUseMerchantItem(Item item, ViewBagCard viewBagCard, bool successful = true)
        {
            if (successful)
            {
                viewBagCard.card.SetNameAfterEnhancement(item.data.enhanceLevel);
                viewBagCard.card.SetEnhancement(item.data.enhanceLevel + 1);
                viewBagCard.InitView(); // 刷新卡牌样式
            }
            // 物品数量的更新逻辑在强化脚本里，ShopEnhanceUI.cs
            //item.amount -= 1;
            //if (item.amount <= 0) inventorySystem.RemoveItem(item);
            //if (item.amount <= 0) shopSystem.RemoveBagItem(item);
        }

        /// <summary>
        /// 使用随时可用的物品
        /// </summary>
        private void OnUseAnyTimeItem(UseItemEvent e)
        {
            Item item = e.item;
            ViewCard viewCard = e.viewCard;
            switch (item.data.itemName)
            {
                case "破碎的古镜":
                case "损坏的古镜":
                case "破旧的古镜":
                case "完整的古镜":
                case "华丽的古镜":
                    inventorySystem.SpawnBagCardInBag(viewCard.card);
                    break;
                case "巫术法杖":
                    List<int> canSpawnCard = new List<int>(); // 可以被生成的卡牌列表
                    // 寻找所有的调查员（橙色）牌
                    foreach (SOCharacterInfo so in IdToSO.soCharacterList)
                    {
                        if (so.rarity == RarityEnum.Orange) canSpawnCard.Add(so.characterID);
                    }
                    // 随机生成一张
                    if (canSpawnCard.Count > 0)
                    {
                        int rand = UnityEngine.Random.Range(0, canSpawnCard.Count);
                        this.GetSystem<ISpawnSystem>().SpawnCard(rand);
                        Card new_Card = this.GetSystem<ISpawnSystem>().GetLastSpawnedCard().GetComponent<ViewBagCard>().card;
                        inventorySystem.SpawnBagCardInBag(new_Card);
                    }
                    break;
            }
            AfterUseAnyTimeItem(item);
        }

        /// <summary>
        /// 使用完随时可用的物品后处理
        /// </summary>
        /// <param name="item">使用过的物品</param>
        private void AfterUseAnyTimeItem(Item item)
        {
            item.amount -= 1;
            if (item.amount <= 0) inventorySystem.RemoveItem(item);
        }

        /// <summary>
        /// 使用战斗中可以使用的物品
        /// </summary>
        private void OnUseCombatItem(UseItemEvent e)
        {      
            if (GameManager.Instance.gameSceneMan.GetCurrentSceneName() != "Combat") return;
            Item item = e.item;
            switch (item.data.itemName)
            {
                case "浅蓝色药水":
                case "中蓝色药水":
                case "深蓝色药水":
                    if (item.data.volume == ItemVolumeEnum.Small) playerManager.player.AddSan(20);
                    else if (item.data.volume == ItemVolumeEnum.Medium) playerManager.player.AddSan(50);
                    else
                    {
                        int maxSan = playerManager.player.GetMaxSan();
                        int currSan = playerManager.player.GetSan();
                        playerManager.player.AddSan(maxSan - currSan);
                    }
                    break;
                case "浅蓝绿色药水":
                case "中蓝绿色药水":
                case "深蓝绿色药水":
                    StartCoroutine(Emerald_Potion(item.data.volume));
                    break;
                case "浅紫色药水":
                case "深紫色药水":
                    break;
                case "橙色药水":
                    break;
                case "浅黄色药水":
                case "深黄色药水":
                    break;
                case "祈福法杖":
                    break;
                case "传送卷轴":
                    break;
                case "高级传送卷轴":
                    break;
                case "深蓝色羽毛笔":
                    break;
                case "浅蓝色羽毛笔":
                    break;
                case "炼金沙":
                    float removeLife = e.viewPiece.maxLife / 2;
                    e.viewPiece.AddCurrLife(-removeLife);
                    GameManager.Instance.playerMan.player.AddGold((int)removeLife / 2);
                    break;
                case "浅红色药水":
                case "中红色药水":
                case "深红色药水":
                    if (item.data.volume == ItemVolumeEnum.Small) e.viewPiece.AddCurrLife(5f);
                    else if (item.data.volume == ItemVolumeEnum.Medium) e.viewPiece.AddCurrLife(10f);
                    else e.viewPiece.AddCurrLife(20f);
                    break;
                case "猩红色药水":
                    e.viewPiece.AddCurrLife(e.viewPiece.maxLife - e.viewPiece.currLife);
                    break;
            }
            AfterUseCombatItem(item);
        }

        /// <summary>
        /// 使用完战斗中可用的物品后处理
        /// </summary>
        /// <param name="item">使用过的物品</param>
        private void AfterUseCombatItem(Item item)
        {
            item.amount -= 1;

            // 如果物品被用完了，那么移除这个物品，自动触发物品系统的OnItemListChanged()
            // 否则只是物品数量-1，itemList里的Item没有变，则需要手动刷新UI
            if (item.amount <= 0) 
            { 
                inventorySystem.RemoveItem(item); 
            }
            else
            {
                UIKit.GetPanel("UIInventoryQuickSlot")?.Invoke("RefreshInventoryItems", 0f);
            }
            
        }

        /// <summary>
        /// 蓝绿色药水效果协程
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        private IEnumerator Emerald_Potion(ItemVolumeEnum volume)
        {
            float currSanRegenSpeed = playerManager.player.GetSanRegenSpeed();
            float sanRegenSpeedAdded = 0;
            switch (volume)
            {
                case ItemVolumeEnum.Small:
                    sanRegenSpeedAdded = currSanRegenSpeed * 0.1f;
                    playerManager.player.AddSanRegenSpeed(sanRegenSpeedAdded);
                    break;
                case ItemVolumeEnum.Medium:
                    sanRegenSpeedAdded = currSanRegenSpeed * 0.25f;
                    playerManager.player.AddSanRegenSpeed(sanRegenSpeedAdded);
                    break;
                case ItemVolumeEnum.Large:
                    sanRegenSpeedAdded = currSanRegenSpeed * 0.5f;
                    playerManager.player.AddSanRegenSpeed(sanRegenSpeedAdded);
                    break;
            }
            yield return new WaitForSeconds(60f);
            playerManager.player.AddSanRegenSpeed(-sanRegenSpeedAdded);
        }


    }
}