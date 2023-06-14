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

            // 回复量强化检查
            if(viewCard.card.deathEnhancement.healthIncrease != 0)
            {
                addHP += viewCard.card.deathEnhancement.healthIncrease;
            }
            addHP = viewpiece.hp + addHP > viewpiece.maxHp ? viewpiece.maxHp - viewpiece.hp : addHP;
            viewpiece.hp.Value += addHP;

            // 地图上随机3个怪物造成等于回复量的伤害
            DamageRandMonster(addHP, 3);

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
                        monster.takeDamage(damage);
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
                        monster.takeDamage(damage);
                    }
                }
            }
        }
    }
}