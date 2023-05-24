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
            // 1.是否格子已被占用
            if (!grid.IsEmpty())
                return false;

            // 某些判断...
            
            return true;
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

    public struct PieceAttackStartEvent
    {
        public ViewPieceBase viewPieceBase;
    }

    public struct PieceUnderAttackEvent
    {
        public ViewPieceBase viewPieceBase;
    }

    public class SpecialitiesAttackCheckEvent
    {
        public ViewPieceBase attacker; // 攻击方
        public ViewPieceBase target; // 防守方
        public bool isTargetMonster; // 防守方是否为怪物
        public int damage; // 伤害
        public bool hit; // 是否命中
    }
}