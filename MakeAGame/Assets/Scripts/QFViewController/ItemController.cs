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
            switch (item.data.name)
            {
                case "Item1":
                    // 需要改变卡牌的名字、卡牌显示的名字、卡牌的属性（生面包括攻击力，防御力，速度，寿命，特性；死面包括
                    // 伤害效果的伤害量，恢复效果的恢复量，持续效果的持续时间，给卡牌添加额外功能【如让恢复生命的死面同时随机造成伤害】）
                    break;
            }
        }

        /// <summary>
        /// 使用其它特效物品
        /// </summary>
        /// <param name="item">被使用的物品</param>
        private void OnUseMisc(Item item)
        {
            switch (item.data.name)
            {
                // 没写完
                case "Item42":
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
                case "Item47":
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
            switch (item.data.name)
            {
                case "Item31":
                    if (item.data.volume == ItemVolumeEnum.Small) playerManager.player.AddSan(20);
                    else if (item.data.volume == ItemVolumeEnum.Medium) playerManager.player.AddSan(50);
                    else
                    {
                        int maxSan = playerManager.player.GetMaxSan();
                        int currSan = playerManager.player.GetSan();
                        playerManager.player.AddSan(maxSan - currSan);
                    }
                    break;
                case "Item34":
                    StartCoroutine(Item34(item.data.volume));
                    break;
                case "Item42":
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

        private IEnumerator Item34(ItemVolumeEnum volume)
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