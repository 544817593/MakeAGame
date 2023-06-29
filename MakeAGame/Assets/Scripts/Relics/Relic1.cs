using UnityEngine;

namespace Game
{
    // 蛊器
    public class Relic1: RelicBase
    {
        public Relic1(SORelic so): base(so)
        {
        }

        private float sanAmount => crtParams[0];
        
        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<CombatVictoryEvent>(this, TakeEffect);
        }

        protected override void TakeEffect(object obj)
        {
            int crtMaxSan = PlayerManager.Instance.player.GetMaxSan();
            Debug.Log($"Relic1 take effect: maxSan {crtMaxSan} + {sanAmount}");
            PlayerManager.Instance.player.AddMaxSan((int)sanAmount);
        }
    }
}