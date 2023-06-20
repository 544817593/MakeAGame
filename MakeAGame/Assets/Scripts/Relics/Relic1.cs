namespace Game
{
    // 蛊器
    public class Relic1: RelicBase
    {
        public Relic1(SORelic so): base(so)
        {
            sanAmount = (int)so.effectParams[0];
        }

        private int sanAmount;
        
        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<RoomCombatEndEvent>(this, TakeEffect);
        }

        protected override void TakeEffect(object obj)
        {
            PlayerManager.Instance.player.AddMaxSan(sanAmount);
        }
    }
}