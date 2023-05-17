using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
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
                OnUsePotion(e.item);
            }
            else if (e.item.data.itemType == ItemTypeEnum.Misc)
            {
                OnUseMisc(e.item);
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
            string newName = "";
            switch (item.data.itemName)
            {
                case "A-型初级强化药剂":
                    newName = viewCard.card.charaName;
                    newName += " (+1)";
                    viewCard.card.SetName(newName);
                    viewCard.card.SetEnhancement(1);
                    viewCard.card.AddDamage(2);
                    break;
                case "B-型初级强化药剂":
                    newName = viewCard.card.charaName;
                    newName += " (+1)";
                    viewCard.card.SetName(newName);
                    viewCard.card.SetEnhancement(1);
                    viewCard.card.AddDefense(1);
                    break;
            }
            AfterUseEnhancement(item, viewCard);
        }

        private void AfterUseEnhancement(Item item, ViewCard viewCard)
        {
            viewCard.InitView(); // 刷新卡牌样式
        }

        /// <summary>
        /// 使用其它特效物品
        /// </summary>
        /// <param name="item">被使用的物品</param>
        private void OnUseMisc(Item item)
        {
            switch (item.data.itemName)
            {
                // 没写完
                case "破碎的古镜":
                    List<ViewCard> cardList = inventorySystem.GetCardList();
                    List<int> canDuplicateCardId = new List<int>();
                    if (cardList.Count != 0)
                    {
                        foreach (ViewCard viewCard in cardList)
                        {
                            if (viewCard.card.rarity <= item.data.rarity)
                            {
                                canDuplicateCardId.Add(viewCard.card.charaID);
                            }
                        }

                    }
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
        /// <param name="item">被使用的物品</param>
        private void OnUsePotion(Item item)
        {
            if (GameManager.Instance.gameSceneMan.GetCurrentSceneName() != "Combat") return;
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
            }
            AfterUsePotion(item);
        }

        /// <summary>
        /// 使用药剂类物品后处理
        /// </summary>
        /// <param name="item">使用过的物品</param>
        private void AfterUsePotion(Item item)
        {
            if (item.amount >= 1) item.amount -= 1;
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