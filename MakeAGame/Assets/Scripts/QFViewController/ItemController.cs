using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

namespace Game
{
    public class ItemController : MonoBehaviour, IController, ICanSendEvent
    {
        private PlayerManager playerManager;
        private IInventorySystem inventorySystem;

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
        }

        /// <summary>
        /// 收到使用物品事件后处理
        /// </summary>
        /// <param name="e">使用物品事件所需的Struct参数</param>
        private void OnUseItemEvent(UseItemEvent e)
        {
            if (e.item.data.itemType == ItemTypeEnum.Potion)
            {
                OnUsePotion(e);
            }
            else if (e.item.data.itemType == ItemTypeEnum.Misc)
            {
                OnUseMisc(e);
            }
            else if (e.item.data.itemType == ItemTypeEnum.Enhancement)
            {
                OnUseEnhancement(e.item, e.viewCard);
            }
        }

        /// <summary>
        /// 使用强化物品
        /// </summary>
        /// <param name="item">被使用的物品</param>
        /// <param name="viewCard">卡牌</param>
        private void OnUseEnhancement(Item item, ViewCard viewCard)
        {
            int rand; // 随机数，用来计算强化成功与否
            switch (item.data.itemName)
            {
                case "A-型初级强化药剂":
                    viewCard.card.AddDamage(2);
                    break;
                case "B-型初级强化药剂":
                    viewCard.card.AddDefense(1);
                    break;
                case "C-型初级强化药剂":
                    viewCard.card.AddMoveSpeed(0.5f);
                    break;
                case "D-型初级强化药剂":
                    viewCard.card.AddLife(5f);
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
                        viewCard.card.AddDamage(2);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
                    return;
                case "B-型中级强化药剂":
                    rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 0)
                    {
                        viewCard.card.AddDefense(1);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
                    return;
                case "C-型中级强化药剂":
                    rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 0)
                    {
                        viewCard.card.AddMoveSpeed(0.5f);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
                    return;
                case "D-型中级强化药剂":
                    rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 0)
                    {
                        viewCard.card.AddLife(5f);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
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
                        viewCard.card.AddDamage(4);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
                    return;
                case "B-型高级强化药剂":
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand > 14)
                    {
                        viewCard.card.AddDefense(2);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
                    return;
                case "C-型高级强化药剂":
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand > 14)
                    {
                        viewCard.card.AddMoveSpeed(1f);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
                    return;
                case "D-型高级强化药剂":
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand > 14)
                    {
                        viewCard.card.AddLife(8f);
                        break;
                    }
                    AfterUseEnhancement(item, viewCard, false);
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
            }
            AfterUseEnhancement(item, viewCard);
        }

        /// <summary>
        /// 使用强化物品后处理
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="viewCard">卡牌</param>
        /// <param name="successful">强化成功与否</param>
        private void AfterUseEnhancement(Item item, ViewCard viewCard, bool successful = true)
        {
            if (successful)
            {
                viewCard.card.SetNameAfterEnhancement(item.data.enhanceLevel);
                viewCard.card.SetEnhancement(item.data.enhanceLevel + 1);
                viewCard.InitView(); // 刷新卡牌样式
            }
        }

        /// <summary>
        /// 使用其它特效物品
        /// </summary>
        private void OnUseMisc(UseItemEvent e)
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
                    inventorySystem.SpawnCardInBag(viewCard.card.charaID);
                    break;
                case "巫术法杖":
                    List<int> canSpawnCard = new List<int>(); // 可以被生成的卡牌列表
                    // 寻找所有的调查员（橙色）牌
                    foreach (SOCharacterInfo so in IdToSO.soCharacterList)
                    {
                        if (so.rarity == 4) canSpawnCard.Add(so.characterID);
                    }
                    // 随机生成一张
                    if (canSpawnCard.Count > 0)
                    {
                        int rand = UnityEngine.Random.Range(0, canSpawnCard.Count);
                        inventorySystem.SpawnCardInBag(rand);
                    }
                    break;
                case "木制起钉器":
                    if (GameManager.Instance.gameSceneMan.GetCurrentSceneName() != "Merchant") return;
                    viewCard.card.RemoveFeature(e.soFeature);
                    viewCard.InitView();
                    break;
                case "铁质起钉器":
                    if (GameManager.Instance.gameSceneMan.GetCurrentSceneName() != "Merchant") return;
                    viewCard.card.RemoveAllFeatures();
                    viewCard.card.SetEnhancement(0);
                    string initialCardName = IdToSO.FindCardSOByID(viewCard.card.charaID).characterName;
                    viewCard.card.SetName(initialCardName);
                    viewCard.InitView();
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
                    float removeLife = e.viewPiece.card.maxLife / 2;
                    e.viewPiece.card.AddLife(-removeLife);
                    GameManager.Instance.playerMan.player.AddGold((int) removeLife / 2);
                    break;
            }
            AfterUseMisc(item);
        }

        /// <summary>
        /// 使用其它特效物品后处理
        /// </summary>
        /// <param name="item">使用过的物品</param>
        private void AfterUseMisc(Item item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 使用药剂类物品
        /// </summary>
        private void OnUsePotion(UseItemEvent e)
        {      
            if (GameManager.Instance.gameSceneMan.GetCurrentSceneName() != "Combat") return;
            Item item = e.item;
            Card card = e.viewPiece?.card;
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
                case "浅红色药水":
                case "中红色药水":
                case "深红色药水":
                    if (item.data.volume == ItemVolumeEnum.Small) card.AddLife(5f);
                    else if (item.data.volume == ItemVolumeEnum.Medium) card.AddLife(10f);
                    else card.AddLife(20f);
                    break;
                case "猩红色药水":              
                    card.AddCurrLife(card.maxLife - card.currLife);
                    break;
            }
            AfterUsePotion(item);
        }

        /// <summary>
        /// 使用药剂类物品后处理
        /// </summary>
        /// <param name="item">使用过的物品</param>
        private void AfterUsePotion(Item item)
        {
            item.amount -= 1;
        }

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