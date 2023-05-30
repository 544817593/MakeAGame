using System.Collections.Generic;
using QFramework;

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
                bool isDead = defender.Hit();
                if (isDead)
                {
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