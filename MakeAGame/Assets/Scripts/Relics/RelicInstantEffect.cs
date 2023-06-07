using UnityEngine;

namespace Game
{
    public class RelicInstantEffect : RelicBase
    {
        public RelicInstantEffect(SORelic so): base(so)
        {
        }

        public override void Activate(IRelicSystem sys)
        {
            // RegisterRelicEvent<PieceMoveFinishEvent>(InstantEffect);
            sys.RegisterRelicEvent<PieceMoveFinishEvent>(this, InstantEffect);
        }

        public override void InstantEffect(object obj)
        {
            if (obj is PieceMoveFinishEvent e)
            {
                Debug.Log("relic: take instant effect");   
            }
        }
    }
    
    public class RelicInstantEffect2 : RelicBase
    {
        public RelicInstantEffect2(SORelic so): base(so)
        {
        }

        public override void Activate(IRelicSystem sys)
        {
            // RegisterRelicEvent<PieceMoveFinishEvent>(InstantEffect);
            sys.RegisterRelicEvent<PieceMoveReadyEvent>(this, InstantEffect);
        }

        public override void InstantEffect(object obj)
        {
            if (obj is PieceMoveReadyEvent e)
            {
                Debug.Log("relic2: take instant effect");   
            }
        }
    }
}