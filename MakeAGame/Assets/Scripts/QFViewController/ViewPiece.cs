using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using QFramework;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public partial class ViewPiece: ViewPieceBase
    {
        public Card card { get; private set; }

        public TriggerHelper mouseHelper;

        public BindableProperty<float> currLife; // 寿命
        public BindableProperty<float> maxLife; // 最大寿命

        public void SetDataWithCard(Card _card)
        {
            card = _card;

            Card pieceData = _card.Clone() as Card;
            rarity = pieceData.rarity;
            generalId = pieceData.charaID;
            pieceId = this.GetSystem<ISpawnSystem>().GetPieceIdCounter();
            this.GetSystem<ISpawnSystem>().IncrementPieceIdCounter();
            pieceSize = (pieceData.height, pieceData.width);
            moveSpeed = new BindableProperty<float>(pieceData.moveSpd);
            hp = new BindableProperty<int>(pieceData.maxHp);
            maxHp = new BindableProperty<int>(pieceData.maxHp);
            atkSpeed = new BindableProperty<float>(pieceData.atkSpd);
            atkDmg = new BindableProperty<float>(pieceData.damage);
            defense = new BindableProperty<float>(pieceData.defend);
            accuracy = new BindableProperty<float>(pieceData.accuracy);
            atkRange = new BindableProperty<int>(pieceData.atkRange);
            features = new BindableProperty<List<FeatureEnum>>(pieceData.features);
            dirs = new BindableProperty<List<DirEnum>>(pieceData.moveDirections);
            inCombat = false;
            isAttacking = new BindableProperty<bool>(false);
            isDying = new BindableProperty<bool>(false);
            currLife = new BindableProperty<float>(pieceData.maxLife);
            maxLife = new BindableProperty<float>(pieceData.maxLife);
        }

        private void Start()
        {
            base.Start();
            InitBind();
            InitView();

            // 注意顺序，先放action，再regsiter
            this.RegisterEvent<PieceMoveReadyEvent>(OnPieceMoveReady).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceMoveFinishEvent>(OnPieceMoveFinish).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceAttackStartEvent>(OnPieceAttackStart).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceAttackEndEvent>(OnPieceAttackEnd).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceUnderAttackEvent>(OnPieceUnderAttack).UnRegisterWhenGameObjectDestroyed(gameObject);

            // OnPieceMoveReady += e => { Debug.Log("add action test"); };  // 在这里加是没有用的，不知道为啥
            // OnPieceMoveReady += OnMoveReadyEvent;
            // OnPieceMoveFinish += OnMoveFinishEvent;

            currLife.Register(e => OnCurrLifeChanged(e));
            
            // 从可选方向中随机一个方向
            int dirIndex = UnityEngine.Random.Range(0, dirs.Value.Count);
            direction = dirs.Value[dirIndex];
        }

        private new void Update()
        {
            base.Update();
            currLife.Value -= Time.deltaTime;
        }

        private void OnCurrLifeChanged(float e)
        {
            lifeBar.SetBarFillAmount((float)e/maxLife);
        }

        void InitView()
        {
            spPiece.sprite = card.pieceSprite;
            // 重置collider大小以贴合图片    // todo collider有点太大了
            if(touchArea)
                touchArea.GetComponent<BoxCollider2D>().size = spPiece.sprite.bounds.size;
        }

        
        public override void SetGrids(List<BoxGrid> grids)
        {
            base.SetGrids(grids);
            foreach (var grid in pieceGrids)
            {
                grid.occupation = pieceId;
            }
        }
        
        public override void InitState()
        {
            switch (stateFlag)
            {
                case PieceStateEnum.Moving:
                    var newState = new PieceFriendMovingState(this);
                    ChangeStateTo(newState);
                    break;
            }
        }

        public void Move()
        {
            // 发送准备移动事件
            this.SendEvent<PieceMoveReadyEvent>(new PieceMoveReadyEvent() {viewPieceBase = this});

            var nextGrids = movementSystem.GetNextGrids(direction, pieceGrids);
            bool canMove = CheckIfCanMove(nextGrids);
            if (canMove)
            {
                // 数据变化
                foreach (var oldGrid in pieceGrids) oldGrid.occupation = 0;
                pieceGrids = nextGrids;
                foreach (var newGrid in pieceGrids) newGrid.occupation = pieceId;
                
                // 视觉表现
                DoMove();
            }
            else
            {
                // 暂时跳一下表示没动成
                transform.DOJump(transform.position, 1, 1, 0.3f);
                Debug.Log("move is canceled because check failed");
            }
        }

        /// <summary>
        /// 根据当前方向，获取下次移动后将占据的格子，不做任何筛选
        /// 移动到了MovementSystem下面
        /// </summary>
        /// <returns></returns>
        //private List<BoxGrid> GetNextGrids()
        //{
        //    int rowDiff = direction == DirEnum.Top ? -1 : direction == DirEnum.Down ? 1 : 0;
        //    int colDiff = direction == DirEnum.Left ? -1 : direction == DirEnum.Right ? 1 : 0;

        //    int nextRow;
        //    int nextCol;
        //    List<BoxGrid> nextGrids = new List<BoxGrid>();
        //    var mapGrids = mapSystem.Grids();
        //    foreach (var crtGrid in pieceGrids)
        //    {
        //        nextRow = crtGrid.row + rowDiff;
        //        nextCol = crtGrid.col + colDiff;
        //        // 超出地图边界的情况
        //        if (nextCol < 0 || nextCol >= mapSystem.mapCol || nextRow < 0 || nextRow >= mapSystem.mapRow)
        //            continue;
                
        //        nextGrids.Add(mapGrids[nextRow, nextCol]);
        //    }

        //    return nextGrids;
        //}

        // 每个棋子可能不一样的，检查某个格子是否可以移动上去的方法
        protected override bool CheckIfOneGridCanMove(BoxGrid grid)
        {
            if (!base.CheckIfOneGridCanMove(grid))
                return false;
            
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

        public override void Attack()
        {
            Debug.Log($"piece {this.ToString()} is about to attack");
            // this.SendEvent<PieceAttackReadyEvent>();
            this.SendCommand<PieceAttackCommand>(new PieceAttackCommand(this));
        }
        
        public override bool Hit(int damage)
        {
            this.SendEvent<PieceHitReadyEvent>();

            hp.Value -= damage;
            Debug.Log($"Piece Hit, damage: {damage} hp: {hp}");
        
            this.SendEvent<PieceHitFinishEvent>();

            return hp <= 0;
        }

        protected override void OnMoveReadyEvent(PieceMoveReadyEvent e)
        {
            base.OnMoveReadyEvent(e);
            Debug.Log("ViewPiece receive MoveReadyEvent");
        }

        // private Action testAction;
        protected override void OnMoveFinishEvent(PieceMoveFinishEvent e)
        {
            base.OnMoveFinishEvent(e);
            Debug.Log("ViewPiece receive MoveFinishEvent");
            // testAction += () => Debug.Log("test");   // 但这里是有效的！如果有什么需要叠加的函数，可以加在这里
            // testAction.Invoke();
        }

        protected override void OnAttackStartEvent(PieceAttackStartEvent e)
        {
            // 若不是给自己的通知，不作相应
            if (e.viewPieceBase != this) return;
            ChangeStateTo(new PieceFriendAttackingState(this));
        }
        
        protected override void OnAttackEndEvent(PieceAttackEndEvent e)
        {
            if (e.vpb != this) return;
            ChangeStateTo(new PieceFriendMovingState(this));
        }

        private void MouseDown()
        {
            Debug.Log("mouse down piece");
            this.GetSystem<IPieceSystem>().ShowDirectionWheel(this);
        }

        private void MouseUp()
        {
            Debug.Log("mouse up piece");
            this.SendCommand<ChangePieceDirectionCommand>(new ChangePieceDirectionCommand());
        }

        /// <summary>
        /// 更改当前棋子的寿命
        /// </summary>
        /// <param name="value"></param>
        public void AddCurrLife(float value)
        {
            currLife.Value += value;
        }
    }
}