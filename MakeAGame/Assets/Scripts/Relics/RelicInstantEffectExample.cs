using UnityEngine;

namespace Game
{
    public class RelicInstantEffectExample1 : RelicBase
    {
        public RelicInstantEffectExample1(SORelic so): base(so)
        {
        }

        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<PieceMoveFinishEvent>(this, TakeEffect);
        }

        protected override void TakeEffect(object obj)
        {
            if (obj is PieceMoveFinishEvent e)
            {
                Debug.Log("elicInstantEffectExample1: take instant effect");   
            }
        }
    }
    
    public class RelicInstantEffectExample2 : RelicBase
    {
        public RelicInstantEffectExample2(SORelic so): base(so)
        {
        }

        public override void Activate(IRelicSystem sys)
        {
            sys.RegisterRelicEvent<PieceMoveReadyEvent>(this, TakeEffect);
        }

        protected override void TakeEffect(object obj)
        {
            if (obj is PieceMoveReadyEvent e)
            {
                Debug.Log("RelicInstantEffectExample2: take instant effect");   
            }
        }
    }
}