using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IHandCardSystem : ISystem
    {
        int maxCardCount { get; } // 最大手牌数
        List<ViewCard> handCardList { get; }    // 手牌列表
        
        UIHandCard ui { set; }
        
        bool AddCard(int cardID);
        bool SubCard(int index);
    }
    
    public class HandCardSystem: AbstractSystem, IHandCardSystem
    {
        public List<ViewCard> handCardList { get; } = new List<ViewCard>(); // 手牌列表
        public int maxCardCount { get; } = 7;
        
        public UIHandCard ui {private get; set; }   // 对应UI

        private EasyEvent<int> OnAddCardTest = new EasyEvent<int>();   // 逻辑上卡牌已经添加，要求ui进行更新的事件
        private EasyEvent<int> OnSubCardTest = new EasyEvent<int>();

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
        }
        
        public bool AddCard(int cardID)
        {
            // 判断是否可以加牌
            if (handCardList.Count >= maxCardCount)
            {
                Debug.LogError("try add handcard when hand is full!");
                return false;
            }

            Debug.Log($"HandCardSystem: AddCard({cardID})");
            
            // 卡牌实例化，挂载组件，部分初始化
            GameObject cardGO = this.GetSystem<ICardGeneratorSystem>().CreateCard(cardID);
            var viewCard = cardGO.AddComponent<ViewCard>();
            cardGO.transform.SetParent(ui.CardRoot);

            // 数值变化
            handCardList.Add(viewCard);
            viewCard.handCardIndexTest = handCardList.Count - 1;

            // 通知UI变化   // 通过事件注册
            OnAddCardTest.Trigger(handCardList.Count - 1);

            return true;
        }

        public bool SubCard(int index)
        {
            if (index >= handCardList.Count) return false;
            
            Debug.Log($"HandCardSystem: SubCard {index}");

            var cardGO = handCardList[index].gameObject;
            handCardList.RemoveAt(index);
            
            OnSubCardTest.Trigger(index);
            
            cardGO.DestroySelf();

            return true;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}