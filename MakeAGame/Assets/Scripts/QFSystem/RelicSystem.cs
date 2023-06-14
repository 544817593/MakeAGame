using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRelicSystem : ISystem
    {
        void ActivateRelics();
        
        void AddRelic();

        void RefreshRelics();

        void StartCountTime();

        void EndCountTime();
    }
    
    public class RelicSystem: AbstractSystem, IRelicSystem
    {
        protected override void OnInit()
        {
            Debug.Log("RelicSystem: OnInit");
            
            
            
            Debug.Log("RelicSystem: add test relic");
            relics.Add(new RelicInstantEffectExample());
            
            ActivateRelics();
        }

        private List<RelicBase> relics = new List<RelicBase>(); // 遗物列表
        private int totalSecs;  // 已经计过的时间

        public void ActivateRelics()
        {
            Debug.Log($"RelicSystem: ActivateRelics count {relics.Count}");
            foreach (var relic in relics)
            {
                relic.Activate();
            }
        }

        public void AddRelic()
        {
            throw new System.NotImplementedException();
        }

        public void RefreshRelics()
        {
            throw new System.NotImplementedException();
        }

        private IUnRegister countTimeUnregister;
        public void StartCountTime()
        {
            totalSecs = 0;
            countTimeUnregister = this.RegisterEvent<CountTimeEvent>(CountTime);
        }

        public void EndCountTime()
        {
            countTimeUnregister.UnRegister();
        }

        void CountTime(CountTimeEvent e)
        {
            totalSecs++;
            Debug.Log(totalSecs);
        }
    }
}