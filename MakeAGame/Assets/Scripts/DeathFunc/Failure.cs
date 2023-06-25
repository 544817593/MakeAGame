using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 失败品 为一个友方单位恢复35点生命值。
    /// </summary>
    public class Failure: DeathFuncBase
    {
        private int addHP = 35;

        public Failure()
        {
            area.width = 1;
            area.height = 1;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Failure");
            if (grids.Count == 0)
            {
                return;
            }
            var pieceSystem = this.GetSystem<IPieceSystem>();
            ViewPiece viewpiece = pieceSystem.getViewPieceById(grids[0].occupation);
            if(viewpiece == null)
            {
                return;
            }

            viewpiece.Heal(EnhanceDeathHeal(addHP));

            // 地图上随机3个怪物造成等于回复量的伤害
            DamageRandMonster(addHP, 3);

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