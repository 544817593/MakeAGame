using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 减少手牌
    /// </summary>
    public class SubHandCardCommand: AbstractCommand
    {
        private ViewCard viewCard;

        public SubHandCardCommand(ViewCard _viewCard)
        {
            viewCard = _viewCard;
        }
        
        protected override void OnExecute()
        {
            Debug.Log("send SubHandCard command");
            var handCardSystem = this.GetSystem<IHandCardSystem>();
            handCardSystem.SubCard(viewCard);
        }
    }
}