using QFramework;
using UnityEngine;

namespace Game
{
    public interface ICameraSystem : ISystem
    {
        void SetLookAt(Transform target);
    }
    
    public class CameraSystem: AbstractSystem, ICameraSystem
    {
        protected override void OnInit()
        {
            
        }

        public void SetLookAt(Transform target)
        {
            
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}