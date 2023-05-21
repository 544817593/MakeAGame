using System;
using System.Collections.Generic;
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

        public PieceMoveDirection direction = PieceMoveDirection.None;

        public Action<PieceMoveReadyEvent> OnPieceMoveReady;
        public Action<PieceMoveFinishEvent> OnPieceMoveFinish;

        public List<BoxGrid> pieceGrids { get; protected set; } = new List<BoxGrid>();

        protected virtual void Start()
        {
            mapSystem = this.GetSystem<IMapSystem>();
            
            // 棋子自身也会监听棋子（包括自己）
            OnPieceMoveReady += OnMoveReadyEvent;
            OnPieceMoveFinish += OnMoveFinishEvent;
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
            switch (stateFlag)
            {
                case PieceStateEnum.Moving:
                    var newState = new PieceFriendMovingState(this);
                    ChangeStateTo(newState);
                    break;
            }
        }

        private void Update()
        {
            state.Update();
        }

        private void ChangeStateTo(PieceState newState)
        {
            Debug.Log($"change state from: {state.stateEnum} to: {newState.stateEnum}");
            state.ExitState();
            state = newState;
            state.EnterState();
            stateFlag = state.stateEnum;
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
            Debug.Log("ViewPieceBase receive MoveReadyEvent");
        }
        
        protected virtual void OnMoveFinishEvent(PieceMoveFinishEvent e)
        {
            // todo
            Debug.Log("ViewPieceBase receive MoveFinishEvent");
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }

    public struct PieceMoveReadyEvent
    {
        public ViewPieceBase ViewPieceBase;
    }
    
    public struct PieceMoveFinishEvent
    {
        public ViewPieceBase viewPieceBase;
    }
}