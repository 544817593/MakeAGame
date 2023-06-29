using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 猫头鹰 选择3*3的格子并将他们的时间流逝速率变为慢，持续30秒。
    /// 传入选择的格子，流逝速率变为慢，开始协程计时，计时完毕回到原始值，摧毁卡牌。
    /// </summary>
    public class Owl: DeathFuncBase
    {
        float duration = 30f;
        public Owl()
        {
            area.width = 3;
            area.height = 3;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Owl");
            if (grids.Count == 0)
            {
                return;
            }
            duration = EnhanceDeathDuration(duration);
            foreach (BoxGrid grid in grids)
            {
                GameManager.Instance.buffMan.AddBuff(new BuffChangeGridSpeed(grid, duration, TimeMultiplierEnum.Slow));
            }

            // 死面保护判定
            if (ItemController.Instance.deathDestroyProtection > 0)
            {
                this.GetSystem<IInventorySystem>().SpawnBagCardInBag(viewCard.card);
                ItemController.Instance.deathDestroyProtection -= 1;
            }

            this.GetSystem<IHandCardSystem>().SubCard(viewCard); // 摧毁卡牌
        }
    }
}