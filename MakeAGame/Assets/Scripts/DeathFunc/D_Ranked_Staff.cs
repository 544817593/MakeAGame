using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// D级员工（D-ranked staff）对3*3的格子造成15点伤害。
    /// 传入选择的格子，造成伤害，摧毁卡牌。
    /// </summary>
    public class D_Ranked_Staff: DeathFuncBase
    {
        private int damage = 15;
        
        public D_Ranked_Staff()
        {
            area.width = 3;
            area.height = 3;
            deathEnhanceTypeList.Add(DeathEnhanceTypeEnum.Damage);
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("D_Ranked_Staff");
            if (grids.Count == 0)
            {
                return;
            }
            
            var pieceSystem = this.GetSystem<IPieceSystem>();
            // 遍历传入的grid，对所有monster造成伤害
            foreach (BoxGrid grid in grids)
            {
                if (pieceSystem.IsPieceMonster(grid.occupation))
                {
                    Monster monster = pieceSystem.getMonsterById(grid.occupation);
                    monster.TakeDamage(EnhanceDeathDamage(damage));
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