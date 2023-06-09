using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IHandCardSystem : ISystem
    {
        int maxCardCount { get; } // 最大手牌数
        BindableProperty<List<ViewCard>> handCardList { get; }    // 手牌列表
        
        UIHandCard ui { set; }
        
        bool AddCard(Card cardData);
        bool SubCard(ViewCard viewCard);

        /// <summary>
        /// 返还手牌列表
        /// </summary>
        /// <returns></returns>
        List<ViewCard> GetHandCardList();

        /// <summary>
        /// 把手牌返回背包
        /// </summary>
        void ReturnHandCardToBag();

        /// <summary>
        /// 在玩家属性改变后更新卡牌数据
        /// </summary>
        void UpdateCardStats(PlayerStatsEnum stat, int value);
    }
    
    public class HandCardSystem: AbstractSystem, IHandCardSystem, ICanSendCommand
    {
        public BindableProperty<List<ViewCard>> handCardList { get; } = new BindableProperty<List<ViewCard>>(); // 手牌列表

        public int maxCardCount { get; } = 7;
        
        public UIHandCard ui {private get; set; }   // 对应UI

        private EasyEvent<int> OnAddCardTest = new EasyEvent<int>();   // 逻辑上卡牌已经添加，要求ui进行更新的事件
        private EasyEvent<int> OnSubCardTest = new EasyEvent<int>();
        private IInventorySystem inventorySystem;

        protected override void OnInit()
        {
            OnAddCardTest.Register((index) =>
            {
                ui.AddCard(index);
            });
            OnSubCardTest.Register((index) =>
            {
                ui.SubCard(index);
            });

            handCardList.SetValueWithoutEvent(new List<ViewCard>());
            inventorySystem = this.GetSystem<IInventorySystem>();
            this.RegisterEvent<UnloadSceneEvent>(e => 
            { 
                if (e.sceneName == "Combat")
                {
                    ReturnHandCardToBag();
                }
            });

        }

        public void ReturnHandCardToBag()
        {
            for (int i = handCardList.Value.Count - 1; i >= 0; i--)
            {
                ViewCard viewCard = handCardList.Value[i];
               
                UIKit.GetPanel<BagUI.BagUIPanel>().AddCard(viewCard.card);
                this.SendCommand(new SubHandCardCommand(viewCard));
            }
           // UIKit.ClosePanel<BagUI.BagUIPanel>();
        }

        public bool AddCard(Card cardData)
        {
            // 判断是否可以加牌
            if (handCardList.Value.Count >= maxCardCount)
            {
                Debug.LogError("try add handcard when hand is full!");
                return false;
            }

            Debug.Log($"HandCardSystem: AddCard(chara: {cardData.charaID})");
            
            // 卡牌实例化，挂载组件，部分初始化
            GameObject cardGO = this.GetSystem<ICardGeneratorSystem>().CreateCard();
            cardGO.transform.SetParent(ui.CardRoot);
            var viewCard = cardGO.AddComponent<ViewCard>();
            // 接收数据，初始化牌面显示
            viewCard.card = cardData;
            // viewCard.InitView(); // 在这里写会先于start执行，不对    // 转由start触发
            
            
            

            // 数值变化
            handCardList.Value.Add(viewCard);

            // 通知UI变化   // 通过事件注册
            OnAddCardTest.Trigger(handCardList.Value.Count - 1);

            return true;
        }

        public bool SubCard(ViewCard viewCard)
        {
            if (viewCard == null || !handCardList.Value.Contains(viewCard))
            {
                Debug.LogError("try to sub card null or already moved");
                ui.UpdateLayout();
                return false;
            }
            
            int index = handCardList.Value.IndexOf(viewCard);
            handCardList.Value.RemoveAt(index);
            OnSubCardTest.Trigger(index);

            // var cardGO = viewCard.gameObject;
            // cardGO.DestroySelf();
            viewCard.gameObject.SetActive(false);

            return true;
        }

        public List<ViewCard> GetHandCardList()
        {
            return handCardList;
        }

        public void UpdateCardStats(PlayerStatsEnum stat, int value)
        {
            foreach (ViewCard viewCard in handCardList.Value)
            {
                SOCharacterInfo so = IdToSO.FindCardSOByID(viewCard.card.charaID);
                if (stat == so.atkSpdBonus.stat)
                {
                    viewCard.card.atkSpd += value * so.atkSpdBonus.multiple;
                }
                if (stat == so.atkBonus.stat)
                {
                    viewCard.card.damage += value * so.atkBonus.multiple;
                }
                if (stat == so.hpBonus.stat)
                {
                    viewCard.card.hp += (int)(value * so.hpBonus.multiple);
                    viewCard.card.maxHp += (int)(value * so.hpBonus.multiple);
                }
                viewCard.card.sanCost += value * so.sanCostBonus.multiple;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }

    }
}