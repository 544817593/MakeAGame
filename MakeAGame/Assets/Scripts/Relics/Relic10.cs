using UnityEngine;

namespace Game
{
    // 符纸
    public class Relic10: RelicConsumable
    {
        public Relic10(SORelic so) : base(so)
        {
            leftUseTime = (int)so.effectParams[0];
            minCanDefend = (int) so.effectParams[1];
        }

        private int minCanDefend;
        
        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<RelicDefendCheckEvent>(this, TakeEffect);
        }
        
        protected override void TakeEffect(object obj)
        {
            
            if (obj is RelicDefendCheckEvent e && e.target is ViewPiece)
            {
                // Debug.Log($"relic10: {e.damage} <-> {minCanDefend}");
                if (e.damage <= minCanDefend)
                    return;
                
                OnTakeEffect(obj);

                leftUseTime--;
                if (leftUseTime <= 0)
                {
                    isActive = false;
                    var sys = GameEntry.Interface.GetSystem<IRelicSystem>();
                    // todo 置为失效状态
                
                
                }
            }
        }

        protected override void OnTakeEffect(object obj)
        {
            var e = obj as RelicDefendCheckEvent;
            if (e.damage > minCanDefend) e.damage = 0;
        }
    }
}