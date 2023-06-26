using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 阿比盖尔  对当前位置以及其上下左右范围为1的格子造成10点伤害，获得一张阿比盖尔。
    /// 传入选择的格子(传入最多5个格子，要在传入前选择好，如果上下左右包含了战斗区域外格子就不要传这些格子)，造成伤害，摧毁卡牌，背包加入卡牌。
    /// </summary>
    public class Abigal_Miller: DeathFuncBase
    {
        private int damage = 10;
        
        public Abigal_Miller()
        {
            area.width = 3;
            area.height = 3;
            area.pattern = new List<int[]>()
                {new int[2] {-1, 0}, new int[2] {0, -1}, new int[2] {0, 0}, new int[2] {0, 1}, new int[2] {1, 0}};
            deathEnhanceTypeList.Add(DeathEnhanceTypeEnum.Damage);
        }

        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Abigal_Miller");
            if (grids.Count == 0)
            {
                return;
            }
            
            // 如果经过强化，则伤害加上强化的值
            if (viewCard.card.deathEnhancement.damageIncrease > 0)
            {
                damage += viewCard.card.deathEnhancement.damageIncrease;
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

            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(viewCard.card); // 加入背包
            this.GetSystem<IHandCardSystem>().SubCard(viewCard); // 摧毁卡牌



        }
    }
}