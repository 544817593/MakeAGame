using UnityEngine;

namespace Game
{
    public class RelicRecycleExample: RelicRecycle
    {
        public RelicRecycleExample(SORelic so): base(so)
        {
        }
        
        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<PieceMoveReadyEvent>(this, TakeEffect);
            sys.RegisterRelicEvent<PieceMoveFinishEvent>(this, WithdrawEffect);
        }

        protected override void TakeEffect(object obj)
        {
            Debug.Log("RelicRecycleExample: TakeEffect");
        }

        protected override void WithdrawEffect(object obj)
        {
            Debug.Log("RelicRecycleExample: WithdrawEffect");
        }
    }
}