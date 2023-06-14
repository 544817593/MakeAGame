using UnityEngine;

namespace Game
{
    public class RelicConsumableExample: RelicConsumable
    {
        public RelicConsumableExample(SORelic so) : base(so)
        {
            leftUseTime = 3;
        }

        public override void Activate(IRelicSystem sys)
        {
            // 注意注册的时候还是TakeEffect
            sys.RegisterRelicEvent<PieceMoveFinishEvent>(this, TakeEffect);
        }

        protected override void OnTakeEffect(object obj)
        {
            Debug.Log($"RelicConsumableExample: OnTakeEffect, leftUseTime before: {leftUseTime}");
        }
    }
}