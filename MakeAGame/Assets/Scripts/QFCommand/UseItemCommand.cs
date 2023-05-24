using QFramework;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class UseItemCommand : AbstractCommand
    {
        private UseItemEvent _event;

        public UseItemCommand(UseItemEvent @event)
        {
            _event = @event;
        }

        protected override void OnExecute()
        {
            this.SendEvent(_event);
        }


    }

    public struct UseItemEvent
    {
        public Item item;
        public ViewCard viewCard;
        public ViewBagCard viewBagCard;
        public SOFeature soFeature;
        public Monster monster;
        public ViewPiece viewPiece;
    }

}