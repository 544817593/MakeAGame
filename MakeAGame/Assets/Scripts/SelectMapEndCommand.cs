using Game;
using QFramework;

namespace Game
{
    public class SelectMapEndCommand: AbstractCommand
    {
        protected override void OnExecute()
        {
            var mapSelectSystem = this.GetSystem<IMapSelectSystem>();
            mapSelectSystem.SelectMapEnd();
            
            this.SendEvent<SelectMapEndEvent>();
        }
        
    }
}