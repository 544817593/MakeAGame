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
        
        bool AddCard(Card cardData);
        bool SubCard(ViewCard viewCard);
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
        
        public bool AddCard(Card cardData)
        {
            // 判断是否可以加牌
            if (handCardList.Count >= maxCardCount)
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
            handCardList.Add(viewCard);

            // 通知UI变化   // 通过事件注册
            OnAddCardTest.Trigger(handCardList.Count - 1);

            return true;
        }

        public bool SubCard(ViewCard viewCard)
        {
            if (viewCard == null || !handCardList.Contains(viewCard))
            {
                Debug.LogError("try to sub card null or already moved");
                ui.UpdateLayout();
                return false;
            }
            
            int index = handCardList.IndexOf(viewCard);
            handCardList.RemoveAt(index);
            OnSubCardTest.Trigger(index);

            // var cardGO = viewCard.gameObject;
            // cardGO.DestroySelf();
            viewCard.gameObject.SetActive(false);

            return true;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}