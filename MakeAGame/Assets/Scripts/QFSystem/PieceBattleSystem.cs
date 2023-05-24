using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPieceBattleSystem: ISystem
    {
        List<PieceBattlePair> battlePairs { get; }
        
        // 战场状态变化后，所有棋子检测一遍攻击范围
        void CheckAllPieceAtkRange();
        
        // 开启对战
        void StartBattle(ViewPieceBase attacker, List<ViewPieceBase> defender);
        // 结束对战（脱战、死亡等）
        void EndBattle(PieceBattlePair bp);
    }

    public class PieceBattleSystem : AbstractSystem, IPieceBattleSystem
    {
        public List<PieceBattlePair> battlePairs { get; } = new List<PieceBattlePair>();
        private IPieceSystem pieceSystem;
        
        protected override void OnInit()
        {
            pieceSystem = this.GetSystem<IPieceSystem>();
            this.RegisterEvent<PieceMoveFinishEvent>((e) =>
            {
                CheckAllPieceAtkRange();
            });
        }
        
        public void CheckAllPieceAtkRange()
        {
            // 先检查友方的
            foreach (var viewPiece in pieceSystem.pieceFriendList)
            {
                if(viewPiece.IsAttacking()) continue;    // 注意这种方式似乎不能兼容群攻
                var toAttackPieces = CheckFriendPieceAtkRange(viewPiece);
                if (toAttackPieces.Count > 0)
                {
                    StartBattle(viewPiece, toAttackPieces);
                }
            }
            
            // 再检查敌方的
            foreach (var monster in pieceSystem.pieceEnemyList)
            {
                if(monster.IsAttacking()) continue;    // 注意这种方式似乎不能兼容群攻
                var toAttackPieces = CheckEnemyPieceAtkRange(monster);
                if (toAttackPieces.Count > 0)
                {
                    StartBattle(monster, toAttackPieces);
                }
            }
        }

        /// <summary>
        /// 从攻击范围中寻找可以攻击的对象（友方，默认和敌方对战）
        /// </summary>
        /// <param name="viewPB"></param>
        /// <returns></returns>
        List<ViewPieceBase> CheckFriendPieceAtkRange(ViewPiece viewPiece)
        {
            int atkDist = viewPiece.card.atkRange;
            List<ViewPieceBase> toAttackPieces = new List<ViewPieceBase>();
            foreach (var monster in pieceSystem.pieceEnemyList)
            {
                if (pieceSystem.GetPieceDist(viewPiece, monster) <= atkDist)
                {
                    toAttackPieces.Add(monster);
                }
            }

            return toAttackPieces;
        }
        
        /// <summary>
        /// 从攻击范围中寻找可以攻击的对象（敌方，默认和友方对战）
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        List<ViewPieceBase> CheckEnemyPieceAtkRange(Monster monster)
        {
            int atkDist = monster.data.atkRange;
            List<ViewPieceBase> toAttackPieces = new List<ViewPieceBase>();
            foreach (var viewPiece in pieceSystem.pieceFriendList)
            {
                if (pieceSystem.GetPieceDist(monster, viewPiece) <= atkDist)
                {
                    toAttackPieces.Add(monster);
                }
            }

            return toAttackPieces;
        }

        public void StartBattle(ViewPieceBase attacker, List<ViewPieceBase> defender)
        {
            PieceBattlePair battleInfo = new PieceBattlePair(attacker, defender);
            battlePairs.Add(battleInfo);
            
            this.SendEvent<PieceAttackStartEvent>(new PieceAttackStartEvent() {viewPieceBase = attacker});

            // 特性测试用代码 **************
            Monster m = new Monster();
            List<PropertyEnum> lst = new List<PropertyEnum>();
            lst.Add(PropertyEnum.Dominant);
            m.properties = new BindableProperty<List<PropertyEnum>>();
            m.properties.SetValueWithoutEvent(lst);
            ViewPiece p = new ViewPiece();

            SpecialitiesAttackCheckEvent e = new SpecialitiesAttackCheckEvent
            {
                attacker = m,
                target = p,
                isTargetMonster = false,
                damage = 20,
                hit = true
            };

            this.SendEvent<SpecialitiesAttackCheckEvent>(e);
            Debug.LogWarning(e.damage);
            // ****************************
        }

        public void EndBattle(PieceBattlePair bp)
        {
            
        }
        
        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
    
    
}