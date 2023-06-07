using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPieceBattleSystem: ISystem
    {
        // List<PieceBattlePair> battlePairs { get; }
        Dictionary<ViewPieceBase, List<ViewPieceBase>> dictBattle { get; }
        
        // 战场状态变化后，所有棋子检测一遍攻击范围
        void CheckAllPieceAtkRange();
        
        // 开启对战
        void StartBattle(ViewPieceBase attacker, List<ViewPieceBase> defender);
        // 某个棋子死亡，结束相关对战
        void EndBattle(ViewPieceBase viewPieceBase);
    }

    public class PieceBattleSystem : AbstractSystem, IPieceBattleSystem
    {
        // public List<PieceBattlePair> battlePairs { get; } = new List<PieceBattlePair>();
        public Dictionary<ViewPieceBase, List<ViewPieceBase>> dictBattle { get; private set; } =
            new Dictionary<ViewPieceBase, List<ViewPieceBase>>();
        
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
            bool pieceHasBattle = dictBattle.ContainsKey(viewPiece);
            List<ViewPieceBase> opponents = pieceHasBattle ? dictBattle[viewPiece] : null;
            
            int atkDist = viewPiece.atkRange;
            List<ViewPieceBase> toAttackPieces = new List<ViewPieceBase>();
            foreach (var monster in pieceSystem.pieceEnemyList)
            {
                // 已经在被这个棋子打了
                if(pieceHasBattle && opponents.Contains(monster))
                    continue;
                
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
            bool pieceHasBattle = dictBattle.ContainsKey(monster);
            List<ViewPieceBase> opponents = pieceHasBattle ? dictBattle[monster] : null;
            
            int atkDist = monster.data.atkRange;
            List<ViewPieceBase> toAttackPieces = new List<ViewPieceBase>();
            foreach (var viewPiece in pieceSystem.pieceFriendList)
            {
                // 已经在被这个棋子打了
                if(pieceHasBattle && opponents.Contains(viewPiece))
                    continue;
                
                if (pieceSystem.GetPieceDist(monster, viewPiece) <= atkDist)
                {
                    toAttackPieces.Add(viewPiece);
                }
            }

            return toAttackPieces;
        }

        public void StartBattle(ViewPieceBase attacker, List<ViewPieceBase> defenders)
        {
            // PieceBattlePair battleInfo = new PieceBattlePair(attacker, defender);
            // battlePairs.Add(battleInfo);
            
            // ViewPieceBase加了个inCombat的bool变量，进入战斗(挨打或者攻击)都变为True，
            // 退出战斗变为False，别忘记在StartBattle和ExitBattle对应的位置处理下，
            // 处理完删掉这条注释就好。

            foreach (var defender in defenders)
            {
                AddDictBattle(attacker, defender);
            }
            
            this.SendEvent<PieceAttackStartEvent>(new PieceAttackStartEvent() {viewPieceBase = attacker});
            foreach (var defender in defenders)
            {
                Debug.DrawLine(attacker.transform.position, defender.transform.position, Color.red, 10f);
            }

            // TODO 计算伤害和命中等基础属性

            // 原始伤害和命中计算完后，先触发攻击方特性，再触发防御方

            // 特性测试用代码 **************
            //Monster m = new Monster();
            //List<PropertyEnum> lst = new List<PropertyEnum>();
            //lst.Add(PropertyEnum.Dominant);
            //m.features = new BindableProperty<List<PropertyEnum>>();
            //m.features.SetValueWithoutEvent(lst);
            //ViewPiece p = new ViewPiece();

            //SpecialitiesAttackCheckEvent e = new SpecialitiesAttackCheckEvent
            //{
            //    attacker = m,
            //    target = p,
            //    isTargetMonster = false,
            //    damage = 20,
            //    hit = true
            //};

            //this.SendEvent<SpecialitiesAttackCheckEvent>(e);
            //Debug.LogWarning(e.damage);
            // ****************************
        }

        public void EndBattle(ViewPieceBase vpb)
        {
            RemoveDictBattleAsAttacker(vpb);
            RemoveDictBattleAsDefender(vpb);
        }

        void AddDictBattle(ViewPieceBase attacker, ViewPieceBase defender)
        {
            if (!dictBattle.ContainsKey(attacker))
            {
                dictBattle[attacker] = new List<ViewPieceBase>();
            }
            dictBattle[attacker].Add(defender);
        }

        void RemoveDictBattle(ViewPieceBase attacker, ViewPieceBase defender)
        {
            if (!dictBattle.ContainsKey(attacker) || !dictBattle[attacker].Contains(defender))
            {
                Debug.LogError("try remove dictBattle unexisted");
                return;
            }

            dictBattle[attacker].Remove(defender);
            if (dictBattle[attacker].Count == 0)
            {
                dictBattle.Remove(attacker);
            }
        }

        void RemoveDictBattleAsAttacker(ViewPieceBase attacker)
        {
            if (dictBattle.ContainsKey(attacker))
            {
                dictBattle[attacker].Clear();
                dictBattle.Remove(attacker);   
            }
        }

        void RemoveDictBattleAsDefender(ViewPieceBase defender)
        {
            List<ViewPieceBase> toRemoveKeys = new List<ViewPieceBase>();
            foreach (var kvp in dictBattle)
            {
                if (kvp.Value.Contains(defender))
                {
                    kvp.Value.Remove(defender);
                    if (kvp.Value.Count == 0)
                    {
                        toRemoveKeys.Add(kvp.Key);
                    }
                }
            }

            foreach (var key in toRemoveKeys)
            {
                this.SendEvent<PieceAttackEndEvent>(new PieceAttackEndEvent() {vpb = key});
                dictBattle.Remove(key);
            }
        }
        
        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
    
    
}