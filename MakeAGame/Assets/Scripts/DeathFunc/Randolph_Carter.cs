using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 伦道夫·卡特 当前位置的时间流逝速率永久降为最慢，同时获得一张伦道夫。
    /// </summary>
    public class Randolph_Carter: DeathFuncBase
    {
        public Randolph_Carter()
        {
            area.width = 1;
            area.height = 1;
        }

        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Randolph_Carter");

            if (grids.Count != 1)
            {
                return;
            }

            foreach (var grid in grids)
            {
                grid.timeMultiplier.Value = TimeMultiplierEnum.Superslow; // 最慢流速
            }
            
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(viewCard.card); // 加入背包
            this.GetSystem<IHandCardSystem>().SubCard(viewCard); // 摧毁卡牌
        }
    }
}