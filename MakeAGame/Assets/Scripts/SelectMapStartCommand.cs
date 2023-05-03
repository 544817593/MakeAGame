using Game;
using QFramework;

namespace Game
{
    public class SelectMapStartCommand: AbstractCommand
    {
        public SelectArea area;
        
        protected override void OnExecute()
        {
            var mapSelectSystem = this.GetSystem<IMapSelectSystem>();
            mapSelectSystem.SelectMapStart(area);
            
            this.SendEvent<SelectMapStartEvent>();
        }
        
    }
}