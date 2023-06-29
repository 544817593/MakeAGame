using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public class PieceAttackCommand: AbstractCommand
    {
        private ViewPieceBase attacker;
        // 攻击数据...

        public PieceAttackCommand(ViewPieceBase _attacker)
        {
            attacker = _attacker;
        }
        
        protected override void OnExecute()
        {
            var battleSystem = this.GetSystem<IPieceBattleSystem>();
            var defenders = battleSystem.dictBattle[attacker];
            List<ViewPieceBase> toDiePieces = new List<ViewPieceBase>();
            foreach (var defender in defenders)
            {
                // 对每个防御者，通过初始值计算是否命中
                bool isHit = Random.Range(0, 1f) <= attacker.accuracy;

                int damage = (int)attacker.atkDmg;
                
                var atkEvent = new SpecialitiesAttackCheckEvent()
                    {attacker = attacker, damage = (int) damage, hit = isHit, target = defender};
                this.SendEvent<SpecialitiesAttackCheckEvent>(atkEvent);

                var relicAtkEvent = new RelicAttackCheckEvent()
                    {attacker = attacker, damage = atkEvent.damage, hit = atkEvent.hit, target = defender};
                this.SendEvent<RelicAttackCheckEvent>(relicAtkEvent);
                
                var defEvent = new SpecialitiesDefendCheckEvent()
                {
                    attacker = attacker, target = defender, isMagic = false, damage = (int)relicAtkEvent.damage, // todo isMagic咋判断的
                    boxgrids = defender.pieceGrids
                };
                this.SendEvent<SpecialitiesDefendCheckEvent>(defEvent);
                
                var relicDefEvent = new RelicDefendCheckEvent()
                {
                    attacker = attacker, target = defender, isMagic = false, damage = (int)defEvent.damage, // todo isMagic咋判断的
                    boxgrids = defender.pieceGrids
                };
                this.SendEvent<RelicDefendCheckEvent>(relicDefEvent);
                
                // 攻击棋子可能需要转向
                attacker.PieceFlip(defender);
                bool isDead = defender.Hit(damage, attacker);

                if (isDead)
                {
                    this.SendEvent<SpecialitiesPieceDieEvent>(new SpecialitiesPieceDieEvent { viewPiece = defender });
                    toDiePieces.Add(defender);
                }
            }

            foreach (var defender in toDiePieces)
            {
                // 死亡棋子从战斗系统中注销
                battleSystem.EndBattle(defender);
                // 再从棋子系统中注销
                this.GetSystem<IPieceSystem>().RemovePiece(defender);
                // 最后处理自身的死亡
                defender.Die();
            }
        }
    }
}