using Game;
using QFramework;

namespace Game
{
    public class SelectMapEndCommand: AbstractCommand
    {
        private ViewCard viewCard;
        
        public SelectMapEndCommand(ViewCard _viewCard)
        {
            viewCard = _viewCard;
        }
        
        protected override void OnExecute()
        {
            var mapSelectSystem = this.GetSystem<IMapSelectSystem>();
            mapSelectSystem.SelectMapEnd(viewCard);
        }
        
    }
}