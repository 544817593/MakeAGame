using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 魔虫 在任意2*2的格子上分别召唤1只魔虫。
    /// </summary>
    public class Devil_Bug: DeathFuncBase
    {
        public Devil_Bug()
        {
            area.width = 2;
            area.height = 2;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Devil_Bug");
            if (grids.Count == 0)
            {
                return;
            }
            
            // 遍历传入的grid，召唤魔虫
            foreach (BoxGrid grid in grids)
            {
                if (grid.occupation == 0) {
                    Debug.Log("生成魔虫");
                    this.GetSystem<IPieceSystem>().AddPieceFriend(viewCard.card, grids);
                }
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