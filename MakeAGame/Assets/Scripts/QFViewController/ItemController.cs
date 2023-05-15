using QFramework;
using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

namespace Game
{
    public class ItemController : MonoBehaviour, IController, ICanSendEvent
    {
        private PlayerManager playerManager;
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
        }

        /// <summary>
        /// 收到使用物品事件后处理
        /// </summary>
        /// <param name="e">使用物品时间所需的Struct参数</param>
        private void OnUseItemEvent(UseItemEvent e)
        {
            if (e.item.data.itemType == ItemTypeEnum.Potion)
            {
                OnUsePotion(e.item);
            }
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


    }
}