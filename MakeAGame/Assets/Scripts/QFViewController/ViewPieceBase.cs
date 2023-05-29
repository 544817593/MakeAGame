using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 棋子基类，理想情况下，敌人和友方都将继承此类
    /// </summary>
    public partial class ViewPieceBase: MonoBehaviour, IController, ICanSendEvent
    {
        protected IMapSystem mapSystem;
        protected SOCharacterInfo so;
        protected PieceStateEnum stateFlag = PieceStateEnum.Moving;
        protected PieceState state = new PieceStateIdle(null);

        public DirEnum direction = DirEnum.None;
        public bool inCombat; // 是否在战斗中(挨打或者攻击)
        public List<BuffType> listBuffs;    // 目前身上起效的buff

        public Action<PieceMoveReadyEvent> OnPieceMoveReady;
        public Action<PieceMoveFinishEvent> OnPieceMoveFinish;
        public Action<PieceAttackStartEvent> OnPieceAttackStart;
        public Action<PieceAttackEndEvent> OnPieceAttackEnd;
        public Action<PieceUnderAttackEvent> OnPieceUnderAttack;

        public List<BoxGrid> pieceGrids { get; protected set; } = new List<BoxGrid>();
        // 经过所有占地格子计算出来的时间流速
        public float crtTimeMultiplier
        {
            get
            {
                float val = 0f;
                foreach (var grid in pieceGrids)
                {
                    val += Extensions.ToTimeMultiplierFloat(grid.timeMultiplier);
                }
                val /= pieceGrids.Count;
                return val;
            }
        }

        // public List<BoxGrid> attadkRangeGrids = new List<BoxGrid>(); // todo

        protected virtual void Start()
        {
            mapSystem = this.GetSystem<IMapSystem>();
            
            // 棋子自身也会监听棋子（包括自己）
            OnPieceMoveReady += OnMoveReadyEvent;
            OnPieceMoveFinish += OnMoveFinishEvent;
            OnPieceAttackStart += OnAttackStartEvent;
            OnPieceAttackEnd += OnAttackEndEvent;
            OnPieceUnderAttack += OnUnderAttackEvent;
        }

        public virtual void SetGrids(List<BoxGrid> grids)
        {
            foreach (var grid in grids)
            {
                pieceGrids.Add(grid);
            }
            transform.position = GetGridsCenterPos();
        }

        /// <summary>
        /// 棋子生成后，设置初始状态
        /// </summary>
        public virtual void InitState()
        {

        }

        

        private void Update()
        {
            state.Update();
        }

        protected void ChangeStateTo(PieceState newState)
        {
            Debug.Log($"change state from: {state.stateEnum} to: {newState.stateEnum}");
            state.ExitState();
            state = newState;
            state.EnterState();
            stateFlag = state.stateEnum;
        }

        public PieceStateEnum GetPieceState()
        {
            return stateFlag;
        }

        public PieceEnemyMovingState GetEnemyMovingState()
        {
            return state as PieceEnemyMovingState;
        }

        public PieceFriendMovingState GetFriendMovingState()
        {
            return state as PieceFriendMovingState;
        }

        protected Vector3 GetGridsCenterPos()
        {
            if (pieceGrids.Count == 0) return transform.position;
            Vector3 centerPos = Vector3.zero;
            foreach (var grid in pieceGrids)
            {
                centerPos += grid.transform.position;
            }
            centerPos /= pieceGrids.Count;
            return centerPos;
        }
        
        // 这个函数在MovementSystem里有相同功能的
        protected virtual bool CheckIfOneGridCanMove(BoxGrid grid)
        {
            // 通用判断
            // 1.是否格子已被占用，且不是自己当前占用的格子
            if (!pieceGrids.Contains(grid) && !grid.IsEmpty())
                return false;

            // 某些判断...
            
            return true;
        }

        // 发起攻击
        public virtual void Attack()
        {
            
        }

        // 受到攻击，返回是否死亡
        public virtual bool Hit()
        {
            // 收到攻击数据...
            // 进行各种效果计算...
            // 获取实际伤害
            return false;
        }

        public virtual void Die()
        {
            state = new PieceStateIdle(this);
            foreach (var grid in pieceGrids)
            {
                grid.occupation = 0;
            }
            pieceGrids.Clear();
            
            Destroy(gameObject);
        }
        
        protected virtual void OnMoveReadyEvent(PieceMoveReadyEvent e)
        {
            // todo 
            // Debug.Log("ViewPieceBase receive MoveReadyEvent");
        }
        
        protected virtual void OnMoveFinishEvent(PieceMoveFinishEvent e)
        {
            // todo
            // Debug.Log("ViewPieceBase receive MoveFinishEvent");
        }

        protected virtual void OnAttackStartEvent(PieceAttackStartEvent e)
        {

        }

        protected virtual void OnAttackEndEvent(PieceAttackEndEvent e)
        {
            
        }

        protected virtual void OnUnderAttackEvent(PieceUnderAttackEvent e)
        {

        }

        public bool IsAttacking()
        {
            return stateFlag == PieceStateEnum.Attacking;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }

    public struct PieceMoveReadyEvent
    {
        public ViewPieceBase viewPieceBase;
    }
    
    public struct PieceMoveFinishEvent
    {
        public ViewPieceBase viewPieceBase;
    }

    // 开始战斗（而不是攻击）事件
    public struct PieceAttackStartEvent
    {
        public ViewPieceBase viewPieceBase;
    }

    // 结束战斗事件
    public struct PieceAttackEndEvent
    {
        public ViewPieceBase vpb;
    }

    public struct PieceUnderAttackEvent
    {
        public ViewPieceBase viewPieceBase;
    }

    // 即将发起攻击
    public class PieceAttackReadyEvent
    {
        // todo 一些战斗数据...
    }

    // 攻击处理完毕
    public class PieceAttackFinishEvent
    {
        
    }

    // 即将受到攻击
    public class PieceHitReadyEvent
    {
        
    }

    // 受击处理完毕
    public class PieceHitFinishEvent
    {
        
    }

    /// <summary>
    /// 进攻时计算完基础伤害和命中后进行特性检测
    /// </summary>
    public class SpecialitiesAttackCheckEvent
    {
        public ViewPieceBase attacker; // 攻击方
        public ViewPieceBase target; // 防守方
        public bool isTargetMonster; // 防守方是否为怪物
        public int damage; // 伤害
        public bool hit; // 是否命中
    }

    /// <summary>
    /// 受到伤害时计算完伤害和命中后进行特性检测
    /// </summary>
    public class SpecialitiesDefendCheckEvent
    {
        public ViewPieceBase attacker; // 攻击方
        public ViewPieceBase target; // 防守方
        public bool isTargetMonster; // 防守方是否为怪物
        public bool isMagic; // 伤害是否为魔法伤害
        public int damage; // 伤害
        public BoxGrid boxgrid; // 受到攻击的格子(单位可能并非1*1)
    }

    /// <summary>
    /// 移动时进行特性检测
    /// </summary>
    public class SpecialitiesMoveCheckEvent
    {
        public ViewPieceBase piece; // 棋子
        public BoxGrid boxgrid; // 移动到的格子

    }
}