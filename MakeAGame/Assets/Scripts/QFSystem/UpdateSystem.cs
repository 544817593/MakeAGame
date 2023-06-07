using QFramework;
using UnityEngine;

namespace Game
{
    public interface IUpdateSystem: ISystem
    {
        UpdateManager updateMan { get; }
        void Reset();
    }

    public class UpdateSystem : AbstractSystem, IUpdateSystem
    {
        public UpdateManager updateMan { get; private set; }

        protected override void OnInit()
        {
            
        }

        public void Reset()
        {
            if (updateMan != null)
            {
                updateMan.Clear();
            }
            else
            {
                GameObject updateGO = GameObject.Find("UpdateManager");
                updateMan = updateGO.GetComponent<UpdateManager>();
            }
        }
    }
}