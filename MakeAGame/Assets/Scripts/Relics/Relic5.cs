using UnityEngine;

namespace Game
{
    // 小背包
    public class Relic5: RelicBase
    {
        public Relic5(SORelic so): base(so)
        {
        }

        private float cardNum => crtParams[0];
        
        public override void Activate(IRelicSystem sys)
        {
            // 激活即生效一次，无需注册其他事件
            Debug.Log("relic5 activate and take effect");
            var inventorySys = GameEntry.Interface.GetSystem<IInventorySystem>();
            var spawnSystem = GameEntry.Interface.GetSystem<ISpawnSystem>();
            for (int i = 0; i < cardNum; i++)
            {
                var randomInfo = IdToSO.GetRandomCardSO();
                inventorySys.SpawnBagCardInBag(new Card(randomInfo.characterID));
            }

            IsRunOut = true;
        }

        protected override void TakeEffect(object obj)
        {
        }
    }
}