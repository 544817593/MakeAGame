using System;
using System.Collections.Generic;
using System.Threading;
using DamageNumbersPro;
using DG.Tweening;
using QFramework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;
using Random = System.Random;

namespace Game
{
    public partial class ViewPiece: ViewPieceBase
    {
        public Card card { get; private set; }

        public TriggerHelper mouseHelper;

        public BindableProperty<float> currLife; // 寿命
        public BindableProperty<float> maxLife; // 最大寿命
        public bool lockLife;

        private ItemController itemController = ItemController.Instance;

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
            lockLife = false;
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
            // 替换当前方向的资源图片
            gameObject.transform.Find("CurMoveDirection").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(ViewDirectionWheel.CurDirectionDict[direction]);
        }

        private new void Update()
        {
            base.Update();
            if(!lockLife && currLife.Value > 0.001f)
            {
                float timeMultiplier = 0;
                foreach(BoxGrid grid in pieceGrids)
                {
                    timeMultiplier += Extensions.ToTimeMultiplierFloat(grid.timeMultiplier.Value);
                }
                timeMultiplier /= pieceGrids.Count;
                currLife.Value -= Time.deltaTime * timeMultiplier;
            }
        }

        private void OnCurrLifeChanged(float e)
        {
            lifeBar.SetBarFillAmount((float)e/maxLife);
            if (currLife.Value <= 0 && !lockLife)
            {
                this.GetSystem<IPieceBattleSystem>().EndBattle(this); // 不确定是不是需要
                // 再从棋子系统中注销
                this.GetSystem<IPieceSystem>().RemovePiece(this);
                // 最后处理自身的死亡
                Die();
            }
        }

        void InitView()
        {
            spPiece.sprite = card.pieceSprite;
            // 重置collider大小以贴合图片    // todo collider有点太大了
            if (touchArea)
                touchArea.GetComponent<BoxCollider2D>().size = spPiece.sprite.bounds.size;

            //动画部分
            GameObject animGO = IdToSO.FindCardSOByID(card.charaID).GetAnim();
            if (animGO != null)
            {                
                GameObject pieceAnim = GameObject.Instantiate(animGO);                
                pieceAnimator = pieceAnim.GetComponent<Animator>();
                pieceAnim.transform.SetParent(gameObject.transform);
                pieceAnim.transform.localScale = animGO.transform.localScale * 0.1f;
                pieceAnim.transform.localPosition = new Vector3(0, 0.25f, -0.25f); // 确保不会被棋盘遮住
                pieceAnim.transform.localRotation = animGO.transform.localRotation;

                Destroy(gameObject.transform.Find("Root/SpritePiece").GetComponent<SpriteRenderer>());
            }

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

            // 如果自己处于混乱状态
            if (listBuffs != null && listBuffs.Contains(BuffType.Confusion))
            {
                List<DirEnum> dirList = new List<DirEnum>();
                foreach(DirEnum dir in dirs.Value)
                {
                    var tempNextGrids = movementSystem.GetNextGrids(direction, pieceGrids);
                    if (CheckIfCanMove(tempNextGrids)) dirList.Add(dir);
                }
                
                if (dirList.Count > 0)
                {
                    int rand = UnityEngine.Random.Range(0, dirList.Count);
                    direction = dirList[rand];
                    // 替换当前方向的资源图片
                    Sprite curDirection = Resources.Load<Sprite>(ViewDirectionWheel.CurDirectionDict[direction]);
                    gameObject.transform.Find("CurMoveDirection").GetComponent<SpriteRenderer>().sprite = curDirection;
                }                         
            }

            var nextGrids = movementSystem.GetNextGrids(direction, pieceGrids);
            bool canMove = CheckIfCanMove(nextGrids);
            if (canMove)
            {
                this.SendEvent<SpecialitiesMoveCheckEvent>(new SpecialitiesMoveCheckEvent() { piece = this, boxgrid = nextGrids[0] });

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

        // 每个棋子可能不一样的，检查某个格子是否可以移动上去的方法
        protected override bool CheckIfOneGridCanMove(BoxGrid grid, bool ignoreUnits = false)
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
            // 如果移动动画协程还在执行，但又触发DoMove了，那么强制停止之前的
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
                movementCoroutine = null;
            }

            // 找到位置
            var nextPos = GetGridsCenterPos();

            // 如果有动画，则播放动画并启动移动协程，否则直接更改怪物位置
            if (pieceAnimator != null)
            {
                pieceAnimator.SetBool("isMove", true);
                movementCoroutine = StartCoroutine(MoveToTarget(nextPos));
            }
            else
            {
                transform.DOMove(nextPos, 0.3f).OnComplete(OnMoveFinish);
            }
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
        
        public override bool Hit(int damage, ViewPieceBase attacker)
        {
            this.SendEvent<PieceHitReadyEvent>(new PieceHitReadyEvent { piece = this });

            hp.Value -= damage;
            Debug.Log($"Piece Hit, damage: {damage} hp: {hp}");
            MonsterDamageNumber.Spawn(this.Position(), damage);
            // 播放受击动画
            bool foundAttackAnim = false;
            GameObject anim = ((Monster)attacker).data.GetAttackAnim();
            if (anim != null)
            {               
                StartCoroutine(PlayAttackAnimByMarking(GameObject.Instantiate(anim, this.transform)));
                foundAttackAnim = true;
            }          
            if (attacker.pieceAnimator != null)
            {
                foreach (AnimatorControllerParameter parameter in attacker.pieceAnimator.parameters)
                {
                    if (parameter.name == "isAttack")
                    {
                        StartCoroutine(PlayAttackAnimByAction(attacker));
                        foundAttackAnim = true;
                        break;
                    }
                }
            }

            if (!foundAttackAnim)
                Debug.LogError("Attack animation for piece " + attacker.generalId + " was not found");
            

            this.SendEvent<PieceHitFinishEvent>(new PieceHitFinishEvent { piece = this });

            return hp <= 0;
        }

        public override void Die()
        {
            // 亡灵死亡，游戏失败，发送事件
            if (generalId == 0)
            {
                this.SendEvent<CombatDefeatEvent>(new CombatDefeatEvent());
            }
            else
            {
                base.Die();
            }
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

            // 亡灵到达房间终点，关卡通过，发送事件
            if (e.viewPieceBase.generalId == 0 &&
                e.viewPieceBase.pieceGrids[0].terrain.Value == (int)TerrainEnum.Door)
            {
                Debug.LogError("获胜");
                this.SendEvent<CombatVictoryEvent>(new CombatVictoryEvent());
            }

            Debug.Log("ViewPiece receive MoveFinishEvent");
            //testAction += () => Debug.Log("test");   // 但这里是有效的！如果有什么需要叠加的函数，可以加在这里
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
            if (!ItemController.Instance.isMarking)
            {
                this.GetSystem<IPieceSystem>().ShowDirectionWheel(this);
            }
        }

        private void MouseUp()
        {
            Debug.Log("mouse up piece");
            if (itemController.isMarking && itemController.markingType == typeof(ViewPiece))
            {
                ItemController.Instance.markerFunction(this);
                itemController.CancelMarking();
                itemController.AfterUseCombatItem(itemController.markerItem);
            }
            else if (itemController.isMarking && itemController.markingType != typeof(ViewPiece))
            {
                GameManager.Instance.soundMan.Play_cursor_click_invalid_sound();
            }
            else
            {
                GameManager.Instance.soundMan.Play_click_sound();
                this.SendCommand<ChangePieceDirectionCommand>(new ChangePieceDirectionCommand());
            }
        }

        /// <summary>
        /// 更改当前棋子的寿命
        /// </summary>
        /// <param name="value"></param>
        public void AddCurrLife(float value)
        {
            currLife.Value += value;
        }

        public void SetColliderEnable(bool isEnable)
        {
            if(collider2d != null)  // 刚放下的情况
                collider2d.enabled = isEnable;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 棋子类自定义Inspector，显示部分关键信息
    /// </summary>
    [CustomEditor(typeof(ViewPiece))]
    public class ViewPieceEditor : Editor
    {
        public List<FeatureEnum> _properties;
        public List<DirEnum> _dirs;
        private (int, int) _position;

        private void OnEnable()
        {
            // 获取特殊类型的 BindableProperty
            _properties = ((ViewPiece)target).features?.Value;
            _dirs = ((ViewPiece)target).dirs.Value;
            _position = (((ViewPiece)target).pieceGrids[0].row, ((ViewPiece)target).pieceGrids[0].col);

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("", GUILayout.Height(10));
            EditorGUILayout.LabelField("Bindable Properties:");
            var viewPiece = (ViewPiece)target;
            viewPiece.moveSpeed.Value = EditorGUILayout.FloatField("Move Speed", viewPiece.moveSpeed.Value);
            viewPiece.hp.Value = EditorGUILayout.IntField("HP", viewPiece.hp.Value);
            viewPiece.maxHp.Value = EditorGUILayout.IntField("Max HP", viewPiece.maxHp.Value);
            viewPiece.atkSpeed.Value = EditorGUILayout.FloatField("Attack Speed", viewPiece.atkSpeed.Value);
            viewPiece.atkDmg.Value = EditorGUILayout.FloatField("Attack Damage", viewPiece.atkDmg.Value);
            viewPiece.defense.Value = EditorGUILayout.FloatField("Defense", viewPiece.defense.Value);
            viewPiece.accuracy.Value = EditorGUILayout.FloatField("Accuracy", viewPiece.accuracy.Value);
            viewPiece.atkRange.Value = EditorGUILayout.IntField("Attack Range", viewPiece.atkRange.Value);
            viewPiece.inCombat = EditorGUILayout.Toggle("In Combat", viewPiece.inCombat);
            viewPiece.isAttacking.Value = EditorGUILayout.Toggle("Is Attacking", viewPiece.isAttacking.Value);
            viewPiece.isDying.Value = EditorGUILayout.Toggle("Is Dying", viewPiece.isDying.Value);

            // 特性
            for (int i = 0; i < _properties.Count; i++)
            {
                _properties[i] = (FeatureEnum)EditorGUILayout.EnumPopup("Property " + i, _properties[i]);
            }

            // 可移动方向
            for (int i = 0; i < _dirs.Count; i++)
            {
                _dirs[i] = (DirEnum)EditorGUILayout.EnumPopup("Direction " + i, _dirs[i]);
            }

            // 怪物坐标
            int x = _position.Item1;
            int y = _position.Item2;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Piece position");
            x = EditorGUILayout.IntField(x);
            y = EditorGUILayout.IntField(y);
            EditorGUILayout.EndHorizontal();
            _position = (x, y);

            // 更新 SerializedObject，以便可以显示和编辑脚本中的值
            serializedObject.Update();
           
            // 应用 SerializedObject 的更改
            serializedObject.ApplyModifiedProperties();
        }

    }

#endif
}