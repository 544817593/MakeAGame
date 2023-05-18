using System;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public partial class ViewPiece: ViewPieceBase
    {
        public Card card { get; private set; }

        public void SetDataWithCard(Card _card)
        {
            card = _card;
        }

        private void Start()
        {
            base.Start();
            InitBind();
            InitView();
            
            // 注意顺序，先放action，再regsiter
            this.RegisterEvent<PieceMoveReadyEvent>(OnPieceMoveReady).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceMoveFinishEvent>(OnPieceMoveFinish).UnRegisterWhenGameObjectDestroyed(gameObject);

            // OnPieceMoveReady += e => { Debug.Log("add action test"); };  // 在这里加是没有用的，不知道为啥
            
            // todo 先随机一个方向
            direction = (PieceMoveDirection)UnityEngine.Random.Range(1, 5);
            // OnPieceMoveReady += OnMoveReadyEvent;
            // OnPieceMoveFinish += OnMoveFinishEvent;

            // Invoke(nameof(MoveTest), 3f);
        }

        void InitView()
        {
            spPiece.sprite = card.pieceSprite;
        }

        void MoveTest()
        {
            BoxGrid grid = pieceGrids[0];
            var mapSystem = this.GetSystem<IMapSystem>();
            int mapCol = mapSystem.mapCol;
            BoxGrid nextRightGrid = mapSystem.Grids()[grid.row, Mathf.Clamp(grid.col + 1, 0, mapCol - 1)];

            Vector3 moveVec = nextRightGrid.transform.position - grid.transform.position;
            transform.DOMove(transform.position + moveVec, 0.3f);
            
            Invoke(nameof(MoveTest), 3f);
        }

        public void Move()
        {
            // 发送准备移动事件
            // GetArchitecture().SendEvent<PieceMoveReadyEvent>(new PieceMoveReadyEvent() {ViewPieceBase = this});
            this.SendEvent<PieceMoveReadyEvent>(new PieceMoveReadyEvent() {ViewPieceBase = this});

            var nextGrids = GetNextGrids();
            bool canMove = CheckIfCanMove(nextGrids);
            if (canMove)
            {
                // 数据变化
                foreach (var oldGrid in pieceGrids) oldGrid.occupation = 0;
                pieceGrids = nextGrids;
                foreach (var oldGrid in pieceGrids) oldGrid.occupation = card.charaID;  // todo id是什么id待定
                
                // 视觉表现
                DoMove();
            }
            else
            {
                // todo 暂时跳一下表示没动成
                transform.DOJump(transform.position, 1, 1, 0.3f);
                Debug.Log("move is canceled because check failed");
            }
        }

        /// <summary>
        /// 根据当前方向，获取下次移动后将占据的格子，不做任何筛选
        /// </summary>
        /// <returns></returns>
        private List<BoxGrid> GetNextGrids()
        {
            int rowDiff = direction == PieceMoveDirection.Up ? -1 : direction == PieceMoveDirection.Down ? 1 : 0;
            int colDiff = direction == PieceMoveDirection.Left ? -1 : direction == PieceMoveDirection.Right ? 1 : 0;

            int nextRow;
            int nextCol;
            List<BoxGrid> nextGrids = new List<BoxGrid>();
            var mapGrids = mapSystem.Grids();
            foreach (var crtGrid in pieceGrids)
            {
                nextRow = crtGrid.row + rowDiff;
                nextCol = crtGrid.col + colDiff;
                // 超出地图边界的情况
                if (nextCol < 0 || nextCol >= mapSystem.mapCol || nextRow < 0 || nextRow >= mapSystem.mapRow)
                    continue;
                
                nextGrids.Add(mapGrids[nextRow, nextCol]);
            }

            return nextGrids;
        }

        // todo 每个棋子可能不一样的，检查某个格子是否可以移动上去的方法
        private bool CheckIfOneGridCanMove(BoxGrid grid)
        {
            base.CheckIfOneGridCanMove(grid);
            
            // 某些判断...
            
            return true;
        }

        /// <summary>
        /// 检查是否可以移动，根据棋子当前移动方向
        /// </summary>
        private bool CheckIfCanMove(List<BoxGrid> nextGrids)
        {
            // 格子不足，无法移动
            if (nextGrids.Count < pieceGrids.Count)
                return false;

            // 某个格子不符合条件，无法移动
            foreach (var grid in nextGrids)
            {
                if (!CheckIfOneGridCanMove(grid))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 真正看起来移动的视觉表现地方
        /// </summary>
        private void DoMove()
        {
            var nextPos = GetGridsCenterPos();

            transform.DOMove(nextPos, 0.3f).OnComplete(OnMoveFinish);
        }

        private void OnMoveFinish()
        {
            // 发送结束移动事件
            GetArchitecture().SendEvent<PieceMoveFinishEvent>(new PieceMoveFinishEvent() {viewPieceBase = this});
        }
        
        /// <summary>
        /// 强行移动，参数待定，可能用于击退、传送等
        /// </summary>
        private void ForceMove()
        {
            
        }

        protected override void OnMoveReadyEvent(PieceMoveReadyEvent e)
        {
            // todo
            base.OnMoveReadyEvent(e);
            Debug.Log("ViewPiece receive MoveReadyEvent");
        }

        private Action testAction;
        protected override void OnMoveFinishEvent(PieceMoveFinishEvent e)
        {
            // todo
            base.OnMoveFinishEvent(e);
            Debug.Log("ViewPiece receive MoveFinishEvent");
            // testAction += () => Debug.Log("test");   // 但这里是有效的！如果有什么需要叠加的函数，可以加在这里
            testAction.Invoke();
        }
    }
}