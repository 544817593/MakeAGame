using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// // 对4*4的格子造成40点伤害，15%的概率获得一张B级员工。
    /// </summary>
    public class A_Ranked_Staff: DeathFuncBase
    {
        private int damage = 40;
        
        public A_Ranked_Staff()
        {
            area.width = 4;
            area.height = 4;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("A_Ranked_Staff");
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
                    monster.TakeDamage(damage);
                }
            }

            // 死面保护判定
            if (ItemController.Instance.deathDestroyProtection > 0)
            {
                this.GetSystem<IInventorySystem>().SpawnBagCardInBag(viewCard.card);
                ItemController.Instance.deathDestroyProtection -= 1;
            }

            this.GetSystem<IHandCardSystem>().SubCard(viewCard); // 摧毁卡牌

            if (UnityEngine.Random.Range(1, 101) <= 15)
            {
                this.GetSystem<IInventorySystem>().SpawnBagCardInBag(new Card(7)); // B级员工加入背包
            }
        }
    }
}