using UnityEngine;

namespace Game
{
    // 水晶球
    public class Relic9: RelicBase
    {
        public Relic9(SORelic so) : base(so)
        {
            checkSeconds = (int) so.effectParams[0];
            less = (int) so.effectParams[1];
            sys = GameEntry.Interface.GetSystem<IRelicSystem>();
        }

        private int checkSeconds;
        private int less;
        private IRelicSystem sys;
        
        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<CostSanEvent>(this, TakeEffect);
        }

        protected override void TakeEffect(object obj)
        {
            if (sys.totalSecs <= 10 && obj is CostSanEvent e)
            {
                var newCost = (int)Mathf.Floor(e.sanCost * (1 - less / 100f));
                Debug.Log($"relic9 take effect, san cost: {e.sanCost} -> {newCost}");
                e.sanCost = newCost;
            }
        }
    }
}