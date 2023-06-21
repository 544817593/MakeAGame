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
            if(viewCard.card.deathEnhancement.healthIncrease != 0)
            {
                addHP += viewCard.card.deathEnhancement.healthIncrease;
            }

            var pieceSystem = this.GetSystem<IPieceSystem>();
            foreach (BoxGrid grid in grids)
            {
                int tmpAddHP = addHP;
                ViewPiece viewpiece = pieceSystem.getViewPieceById(grid.occupation);
                if(viewpiece == null) continue;
                if(viewpiece.maxHp - viewpiece.hp <= addHP)
                {
                    tmpAddHP = viewpiece.maxHp - viewpiece.hp;
                }
                viewpiece.hp.Value += tmpAddHP;
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
        
                
        // todo 找一个位置集中写DamageRandMonster
        private void DamageRandMonster(int heal, int RandMonsterCount)
        {
            var pieceSystem = this.GetSystem<IPieceSystem>();
            int damage = heal;
            if (viewCard.card.deathEnhancement.damageIncrease > 0) // 检查卡牌的伤害加成强化效果
            {
                damage += viewCard.card.deathEnhancement.damageIncrease;
            }
            // 地图上随机RandMonsterCount个怪物造成等于回复量的伤害
            if (viewCard.card.deathEnhancement.extraDamageEffect)
            {
                // 地图上怪物总数不超过RandMonsterCount个直接扣血，超过RandMonsterCount个随机挑选
                if (pieceSystem.pieceEnemyList.Count <= RandMonsterCount)
                {
                    foreach (Monster monster in pieceSystem.pieceEnemyList)
                    {
                        monster.TakeDamage(damage);
                    }
                }
                else
                {
                    // 洗牌算法，打乱列表后选择RandMonsterCount个数作为monster列表中的index
                    // 创建一个包含0, 1, 2,..., pieceSystem.pieceEnemyList.Count - 1的list
                    // 从头开始，每轮循环从[当前index, 总长度)的范围内随机选1个index, 将随机选择的index位置的值和当前位置的值互换，进行下一轮循环
                    List<int> numbers = Enumerable.Range(0, pieceSystem.pieceEnemyList.Count - 1).ToList();
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        int temp = numbers[i];
                        int randomIndex = UnityEngine.Random.Range(i, numbers.Count);
                        numbers[i] = numbers[randomIndex];
                        numbers[randomIndex] = temp;
                    }
                    for (int i = 0; i < RandMonsterCount; i++)
                    {
                        Monster monster = pieceSystem.pieceEnemyList[numbers[i]];
                        monster.TakeDamage(damage);
                    }
                }
            }
        }
    }
}