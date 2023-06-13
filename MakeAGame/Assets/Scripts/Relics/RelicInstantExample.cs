using UnityEngine;

namespace Game
{
    public class RelicInstantExample1 : RelicBase
    {
        public RelicInstantExample1(SORelic so): base(so)
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
    
    public class RelicInstantExample2 : RelicBase
    {
        public RelicInstantExample2(SORelic so): base(so)
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