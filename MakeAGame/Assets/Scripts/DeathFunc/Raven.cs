using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 渡鸦（Raven）将4*4格子的时间流逝速率修正为正常，持续40秒。
    /// 传入选择的格子，检查强化持续时间，更改时间速率，开始计时，计时完毕回到原始值，摧毁卡牌
    /// </summary>
    public class Raven: DeathFuncBase
    {
        public Raven()
        {
            area.width = 4;
            area.height = 4;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);

            Debug.Log("Raven");
            if (grids.Count == 0)
            {
                return;
            }

            float duration = 40f;
            if (viewCard.card.deathEnhancement.statusTimeIncrease != 0)
            {
                duration += viewCard.card.deathEnhancement.statusTimeIncrease;
            }

            foreach (BoxGrid grid in grids)
            {
                GameManager.Instance.buffMan.AddBuff(new BuffChangeGridSpeed(grid, duration,
                    TimeMultiplierEnum.Normal));
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