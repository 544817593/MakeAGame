using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 死面功能基类
    /// </summary>
    public class DeathFuncBase: ICanGetSystem, ICanSendEvent
    {
        public SelectArea area = new SelectArea();
        public ViewCard viewCard;

        public DeathFuncBase()
        {
            area.selectStage = MapSelectStage.IsPutDeathFunc;
        }

        public virtual void OnExecute(List<BoxGrid> grids)
        {
            Debug.Log($"DeathFuncBase execute grids count: {grids.Count}");
            this.SendEvent(new SpecialitiesDeathExecuteEvent() { viewCard = viewCard, grids = grids });
        }

        #region 死面强化
        protected int EnhanceDeathDamage(int damage)
        {
            return damage += viewCard.card.deathEnhancement.damageIncrease;
        }

        protected int EnhanceDeathHeal(int addHP)
        {
            return addHP += viewCard.card.deathEnhancement.healthIncrease;
        }

        protected float EnhanceDeathDuration(float duration)
        {
            return duration += viewCard.card.deathEnhancement.statusTimeIncrease;
        }

        /// <summary>
        /// 死面强化“对随机三个敌人造成恢复量的伤害”的效果，只有可以恢复的死面才有可能获得
        /// </summary>
        /// <param name="heal"></param>
        /// <param name="RandMonsterCount"></param>
        protected void DamageRandMonster(int heal, int RandMonsterCount)
        {
            var pieceSystem = this.GetSystem<IPieceSystem>();
            int damage = heal;
            EnhanceDeathDamage(damage);
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
        #endregion

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}