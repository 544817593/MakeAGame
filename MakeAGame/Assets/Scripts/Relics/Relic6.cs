using System;
using Random = UnityEngine.Random;

namespace Game
{
    // 亡灵的帽子
    public class Relic6: RelicRecycle
    {
        public Relic6(SORelic so): base(so)
        {
        }

        private PlayerStatsEnum stats = PlayerStatsEnum.None;

        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<RoomCombatStartEvent>(this, TakeEffect);
            sys.RegisterRelicEvent<CombatVictoryEvent>(this, WithdrawEffect);
            sys.RegisterRelicEvent<CombatDefeatEvent>(this, WithdrawEffect);
        }

        protected override void TakeEffect(object obj)
        {
            stats = (PlayerStatsEnum) Random.Range(1, 6);
            PlayerManager.Instance.player.ModifyStats(stats, 1);
        }

        protected override void WithdrawEffect(object obj)
        {
            PlayerManager.Instance.player.ModifyStats(stats, -1);
        }
    }
}