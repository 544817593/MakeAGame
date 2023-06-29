using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 水精（Water Wizard）为3*3格子内的友方单位回复20点生命值，如果该范围内有水，则回复25点。
    /// 传入选择的格子，寻找友方单位，判断是否有水，检查是否强化回血，回血，检查是否强化额外效果，摧毁卡牌。
    /// </summary>
    public class Water_Wizard: DeathFuncBase
    {
        private int addHP = 20;
        
        public Water_Wizard()
        {
            area.width = 3;
            area.height = 3;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Water_Wizard");
            if (grids.Count == 0)
            {
                return;
            }
            
            foreach (BoxGrid grid in grids)
            {
                if(grid.terrain.Value == (int)TerrainEnum.Water)
                {
                    addHP = 25;
                    break;
                }
            }
            
            addHP = EnhanceDeathHeal(addHP);

            var pieceSystem = this.GetSystem<IPieceSystem>();
            foreach (BoxGrid grid in grids)
            {
                ViewPiece viewpiece = pieceSystem.getViewPieceById(grid.occupation);
                if(viewpiece == null) continue;
                viewpiece.Heal(addHP);
            }
            // 对地图上随机3个怪物造成等于治疗量的伤害
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