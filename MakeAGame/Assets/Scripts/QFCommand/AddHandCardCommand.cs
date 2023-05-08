using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 添加手牌
    /// </summary>
    public class AddHandCardCommand: AbstractCommand
    {
        private Card cardData;

        public AddHandCardCommand(Card _cardData)
        {
            cardData = _cardData;
        }
        
        protected override void OnExecute()
        {
            Debug.Log("send AddHandCard command");

            var handCardSystem = this.GetSystem<IHandCardSystem>();
            handCardSystem.AddCard(cardData);
        }
    }
}