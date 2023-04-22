using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 添加手牌
    /// </summary>
    public class AddHandCardCommand: AbstractCommand
    {
        private int cardID;

        public AddHandCardCommand(int _cardID)
        {
            cardID = _cardID;
        }
        
        protected override void OnExecute()
        {
            Debug.Log("send AddHandCard command");
            var handCardSystem = this.GetSystem<IHandCardSystem>();
            handCardSystem.AddCard(cardID);
        }
    }
}