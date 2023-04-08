using QFramework;

namespace Game
{
    public interface IGridGeneratorSystem : ISystem
    {
    }
    
    public class GridGeneratorSystem : AbstractSystem, IGridGeneratorSystem
    {
        protected override void OnInit()
        {
            
        }
    }
}