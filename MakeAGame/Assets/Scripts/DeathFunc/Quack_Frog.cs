using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 呱呱蛙 使一个战斗中的敌方单位攻速永久-0.3（最低0.1），如果该单位对声音敏感，则在该场战斗中额外-0.3（在此用buff处理）。
    /// 传入战斗中的敌方单位所在的格子，检查是否对声音敏感，添加buff，摧毁卡牌。
    /// </summary>
    public class Quack_Frog: DeathFuncBase
    {
        public Quack_Frog()
        {
            area.width = 1;
            area.height = 1;
        }
        
        public override void OnExecute(List<BoxGrid> grids)
        {
            base.OnExecute(grids);
            
            Debug.Log("Quack_Frog");
            if (grids.Count == 0)
            {
                return;
            }
            var pieceSystem = this.GetSystem<IPieceSystem>();
            var monster = pieceSystem.getMonsterById(grids[0].occupation);
            if (monster == null)
            {
                return;
            }
            
            // 是否对声音敏感, 99999999代表时间无限, 是否在战斗中由BuffQuackFrog检查
            bool soundSensitive = monster.features.Value.Contains(FeatureEnum.SoundSensitive);
            GameManager.Instance.buffMan.AddBuff(new BuffQuackFrog(monster, 99999999, soundSensitive));

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