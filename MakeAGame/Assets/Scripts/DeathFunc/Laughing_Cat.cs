using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 爱笑猫（Laughing Cat）使3*3格子内的敌人混乱15秒，混乱中的敌人将随机移动。
    /// 传入选择的格子，寻找格子内敌人，施加混乱词条，开始计时，计时完毕删除词条，摧毁卡牌。
    /// </summary>
    public class Laughing_Cat: DeathFuncBase
    {
        float duration = 15f;
        public Laughing_Cat()
        {
            area.width = 3;
            area.height = 3;
        }

        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Laughing_Cat");
            if (grids.Count == 0)
            {
                return;
            }

            var pieceSystem = this.GetSystem<IPieceSystem>();
            foreach(BoxGrid grid in grids)
            {
                Monster monster = pieceSystem.getMonsterById(grid.occupation);
                if (monster != null)
                {
                    duration = EnhanceDeathDuration(duration);
                    GameManager.Instance.buffMan.AddBuff(new BuffConfusion(monster, duration));
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