using UnityEngine;

namespace Game
{
    // 鱼鳞
    public class Relic7: RelicBase
    {
        public Relic7(SORelic so): base(so)
        {
            damage = (int)so.effectParams[0];
        }

        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<RelicAttackCheckEvent>(this, TakeEffect);
        }

        private int damage;

        protected override void TakeEffect(object obj)
        {
            if (obj is RelicAttackCheckEvent e)
            {
                // Debug.Log($"relic7: ret1 {e.attacker is ViewPiece} ret2 {e.hit} ret3 {e.damage}");
                if (e.attacker is ViewPiece && e.hit && e.damage < 3) e.damage = 3;
            }
        }
    }
}