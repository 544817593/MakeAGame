using UnityEngine;

namespace Game
{
    // 废品回收机
    public class Relic8: RelicCharge
    {
        public Relic8(SORelic so) : base(so)
        {
            totalChargeTime = (int)so.effectParams[0];
            leftChargeTime = totalChargeTime;
        }
        
        public override void Activate(IRelicSystem sys)
        {
            // todo 暂借该事件表示使用了死面
            sys.RegisterRelicEvent<SpecialitiesDeathExecuteEvent>(this, Charge);
            sys.RegisterRelicEvent<SpecialitiesDeathExecuteEvent>(this, TakeEffect);
        }
        
        protected override void Charge(object obj)
        {
            leftChargeTime--;
            Debug.Log($"Relic8: Charge, left: {leftChargeTime}");
        }

        protected override void TakeEffect(object obj)
        {
            if (leftChargeTime <= 0)
            {
                Debug.Log("Relic8: TakeEffect");
                var inventorySys = GameEntry.Interface.GetSystem<IInventorySystem>();

                // todo 目前是随机加牌，如何加普通品质学者牌需要再写
                var randomInfo = IdToSO.GetRandomCardSO();
                inventorySys.SpawnBagCardInBag(new Card(randomInfo.characterID));
                    
                leftChargeTime = totalChargeTime;
            }
        }
    }
}