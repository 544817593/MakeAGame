using UnityEngine;

namespace Game
{
    // 镇定剂
    public class Relic3: RelicBase
    {
        public Relic3(SORelic so): base(so)
        {
        }

        private float fixedSan => crtParams[0];
        
        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<RoomCombatStartEvent>(this, TakeEffect);
        }

        protected override void TakeEffect(object obj)
        {
            int crtMaxSan = PlayerManager.Instance.player.GetMaxSan();
            Debug.Log($"Relic3 take effect: maxSan {fixedSan}");
            PlayerManager.Instance.player.AddMaxSan((int)fixedSan - crtMaxSan);
        }
    }
}