using Game;
using QFramework;

namespace Game
{
    public class SelectMapEndCommand: AbstractCommand
    {
        private ViewCard viewCard;
        private bool isCancel;
        
        public SelectMapEndCommand(ViewCard _viewCard, bool _isCancel)
        {
            viewCard = _viewCard;
            isCancel = _isCancel;
        }
        
        protected override void OnExecute()
        {
            var mapSelectSystem = this.GetSystem<IMapSelectSystem>();
            mapSelectSystem.SelectMapEnd(viewCard, isCancel);
        }
        
    }
}