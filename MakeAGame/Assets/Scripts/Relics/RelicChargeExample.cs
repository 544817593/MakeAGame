using UnityEngine;

namespace Game
{
    public class RelicChargeExample: RelicCharge
    {
        public RelicChargeExample(SORelic so) : base(so)
        {
            totalChargeTime = 3;
            leftChargeTime = totalChargeTime;
        }
        
        public override void Activate(IRelicSystem sys)
        {
            // 注意顺序
            sys.RegisterRelicEvent<PieceMoveFinishEvent>(this, Charge);
            sys.RegisterRelicEvent<PieceMoveFinishEvent>(this, TakeEffect);
        }

        protected override void Charge(object obj)
        {
            leftChargeTime--;
            Debug.Log($"RelicChargeExample: Charge, left: {leftChargeTime}");
        }

        protected override void TakeEffect(object obj)
        {
            if (leftChargeTime <= 0)
            {
                Debug.Log("RelicChargeExample: TakeEffect");
                leftChargeTime = totalChargeTime;
            }
        }
    }
}