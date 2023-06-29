using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 藤曼（Vine）使1*6格子内的敌人移动速度减半，持续3秒，随后这些格子时间流逝速率永久降低一个级别.
    /// 检查是否强化持续时间，每个怪物移速减半，持续时间后，格子流速降低
    /// </summary>
    public class Vine: DeathFuncBase
    {
        public Vine()
        {
            area.width = 1;
            area.height = 6;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Vine");
            if (grids.Count == 0)
            {
                return;
            }
            
            float duration = 3f;
            if (viewCard.card.deathEnhancement.statusTimeIncrease != 0)
            {
                duration += viewCard.card.deathEnhancement.statusTimeIncrease;
            }

            var pieceSystem = this.GetSystem<IPieceSystem>();
            foreach (BoxGrid grid in grids)
            {
                Monster monster = pieceSystem.getMonsterById(grid.occupation);
                if (monster != null)
                {
                    GameManager.Instance.buffMan.AddBuff(new BuffVine(monster, duration)); // 移速减半
                }
                GameManager.Instance.buffMan.AddBuff(new BuffVine2(grid, duration)); // 等待duration秒后格子降速
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