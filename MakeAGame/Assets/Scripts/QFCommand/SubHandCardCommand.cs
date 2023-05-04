using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 减少手牌
    /// </summary>
    public class SubHandCardCommand: AbstractCommand
    {
        private int index;

        public SubHandCardCommand(int _index)
        {
            index = _index;
        }
        
        protected override void OnExecute()
        {
            Debug.Log("send SubHandCard command");
            var handCardSystem = this.GetSystem<IHandCardSystem>();
            handCardSystem.SubCard(index);
        }
    }
}