using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 对3*3的格子造成20点伤害，25%的概率获得一张D级员工。
    /// 传入选择的格子，造成伤害，摧毁卡牌，判定成功后添加一张D级员工到背包。
    /// </summary>
    public class C_Ranked_Staff: DeathFuncBase
    {
        private int damage = 20;
        
        public C_Ranked_Staff()
        {
            area.width = 3;
            area.height = 3;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("C_Ranked_Staff");
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
            if (UnityEngine.Random.Range(1, 101) <= 25)
            {
                this.GetSystem<IInventorySystem>().SpawnBagCardInBag(new Card(5)); // D级员工加入背包，D级员工id为5
            }
        }
    }
}