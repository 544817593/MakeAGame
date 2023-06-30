using Game;
using QFramework;
using System;
using System.Collections.Generic;
using DG.Tweening;
using PieceInfo;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace Game
{
    public class Monster : ViewPieceBase
    {
        // 怪物初始SO数据
        public SOMonsterBase data;

        #region 怪物数据
        public BindableProperty<(int, int)> leftTopGridPos; // 当前左上角位置
        public BindableProperty<(int, int)> botRightGridPos; // 当前右下角位置
        #endregion

        public ViewPiece currentTarget; // 当前目标
        
        private ItemController itemController = ItemController.Instance;
        public TriggerHelper mouseHelper;
        public GameObject touchArea;
        public BoxCollider2D collider2d;

        void Awake()
        {
            mapSystem = this.GetSystem<IMapSystem>();
        }

        void Start()
        {
            base.Start();
            InitBind();

            // 注意顺序，先放action，再regsiter
            this.RegisterEvent<PieceMoveReadyEvent>(OnPieceMoveReady).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceMoveFinishEvent>(OnPieceMoveFinish).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceAttackStartEvent>(OnPieceAttackStart).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceAttackEndEvent>(OnPieceAttackEnd).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<PieceUnderAttackEvent>(OnPieceUnderAttack).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.SendCommand(new MonsterTargetSelectionCommand(this));
        }

        private void InitBind()
        {            
            mouseHelper = touchArea.AddComponent<TriggerHelper>();
            
            mouseHelper.OnMouseDownEvent = MouseDown;
            mouseHelper.OnMouseUpEvent = MouseUp;
            mouseHelper.OnMouseEnterEvent = MouseEnter;
            mouseHelper.OnMouseExitEvent = MouseExit;
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
                    var newState = new PieceEnemyMovingState(this);
                    ChangeStateTo(newState);
                    break;
            }
        }

        public void Move()
        {
            if (movementSystem == null)
                movementSystem = this.GetSystem<IMovementSystem>();

            // 发送准备移动事件
            this.SendEvent<PieceMoveReadyEvent>(new PieceMoveReadyEvent() { viewPieceBase = this });

            var nextLTCorr = FindMovementDir();

            // 无法移动
            if (nextLTCorr.Item1 == -1 && nextLTCorr.Item2 == -1)
            {
                return;
            }

            this.SendEvent<SpecialitiesMoveCheckEvent>(new SpecialitiesMoveCheckEvent() { piece = this, boxgrid = mapSystem.Grids()[nextLTCorr.Item1, nextLTCorr.Item2] });

            // 更新数据
            int diffR = nextLTCorr.Item1 - leftTopGridPos.Value.Item1;
            int diffC = nextLTCorr.Item2 - leftTopGridPos.Value.Item2;
            List<BoxGrid> nextGrids = new List<BoxGrid>();
            foreach (var crtGrid in pieceGrids)
            {
                nextGrids.Add(mapSystem.Grids()[crtGrid.row + diffR, crtGrid.col + diffC]);
            }

            foreach (var oldGrid in pieceGrids)
            {
                oldGrid.occupation = 0;
                oldGrid.gridStatus.Value = GridStatusEnum.Unoccupied;
                //if (data.monsterId == 9998 && oldGrid.timeMultiplier == TimeMultiplierEnum.Superfast && SceneFlow.combatSceneCount == 2 )
                //{
                //    if(GameObject.Find("CombatSceneController").GetComponent<CombatDialogueControl>().active == true)
                //    {
                //        GameObject.Find("CombatSceneController").GetComponent<CombatDialogueControl>().start_dialogue = true;
                //    }
                    
                //}
            }
            pieceGrids = nextGrids;
            foreach (var newGrid in pieceGrids)
            {
                newGrid.occupation = pieceId;
                newGrid.gridStatus.Value = GridStatusEnum.MonsterPiece;
               
            }

            leftTopGridPos.Value = nextLTCorr;

            DoMove();
        }

        /// <summary>
        /// 根据目标，找到怪物的下一个移动方向
        /// 返回移动后的左上角格子坐标，若无法移动，则返回(-1, -1)
        /// </summary>
        private (int, int) FindMovementDir()
        {
            (int, int) original = leftTopGridPos.Value; // 当前左上坐标
            (int, int) positionAfterMovement = leftTopGridPos.Value; // 怪物移动后的坐标

            // 若目标不存在或就是当前位置
            if (currentTarget == null)
                return (-1, -1);

            // 如果自己处于混乱状态
            if (listBuffs != null && listBuffs.Contains(BuffType.Confusion)) 
            {
                List<DirEnum> dirList = new List<DirEnum>();
                foreach (DirEnum dir in dirs.Value)
                {
                    if (CheckIfMovable(dir, original.Item1, original.Item2))
                    {
                        dirList.Add(dir);
                    }
                }
                int rand = UnityEngine.Random.Range(0, dirList.Count);
                if (dirList.Count != 0)
                {
                    return this.GetSystem<IMovementSystem>().CalculateNextPosition(original, dirList[rand]);
                }
                else
                {
                    return (-1, -1);
                }               
            }


            // A* 寻路
            List<BoxGrid> aStarPath = PathFinding.FindPath(original.Item1, original.Item2,
                currentTarget.pieceGrids[0].row, currentTarget.pieceGrids[0].col, this);

            DirEnum newDirection = DirEnum.None;

            // 路径存在
            if (aStarPath != null && aStarPath.Count != 0)
            {
                // 场景显示路线
                Color randColor = UnityEngine.Random.ColorHSV();
                for (int i = 0; i < aStarPath.Count - 1; i++)
                {
                    Debug.DrawLine(aStarPath[i].transform.position - new Vector3(0, 0, 0.3f), aStarPath[i + 1].transform.position - new Vector3(0, 0, 0.3f), randColor, 3f);
                }

                // 设置移动方向
                newDirection = movementSystem.NeighbourBoxGridsToDir(this.GetSystem<IMapSystem>().Grids()
                    [leftTopGridPos.Value.Item1, leftTopGridPos.Value.Item2], aStarPath[1]);
                
            }
            // 路径不存在
            else
            {
                BoxGrid boxGrid = PathFinding.FindGridClosestToTarget(original.Item1, original.Item2, 
                    currentTarget.pieceGrids[0].row, currentTarget.pieceGrids[0].col, this);

                // 场景显示路线
                Color randColor = UnityEngine.Random.ColorHSV();
                Debug.DrawLine(transform.position - new Vector3(0, 0, 0.3f), boxGrid.transform.position - new Vector3(0, 0, 0.3f), randColor, 3f);

                newDirection = movementSystem.NeighbourBoxGridsToDir(this.GetSystem<IMapSystem>().Grids()
                    [leftTopGridPos.Value.Item1, leftTopGridPos.Value.Item2], boxGrid);
            }

            if (newDirection == DirEnum.None)
            {
                Debug.Log("寻路算法觉得怪物" + pieceId + "原地不动最合理");
                return (-1, -1);
            }

            PieceFlip(newDirection);
            direction = newDirection;

            // 更新想要去的格子
            positionAfterMovement = this.GetSystem<IMovementSystem>().CalculateNextPosition(original, direction);
            return positionAfterMovement;
        }

        /// <summary>
        /// 根据行走方向和下一位置的情况，检查是否可以移动
        /// </summary>
        /// <param name="curMoveDir"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <returns></returns>
        public bool CheckIfMovable(DirEnum curMoveDir, int currentX, int currentY, bool ignoreUnits = false)
        {
            if (isAttacking)
            {
                return false;
            }

            // 获取下一步坐标
            (int, int) intendPos = movementSystem.CalculateNextPosition((currentX, currentY), curMoveDir);
            // 对下一步坐标做基础检查
            if (!movementSystem.MovementBaseCheck(intendPos)) return false;
            // 对下一步的格子做检查
            BoxGrid grid = mapSystem.Grids()[intendPos.Item1, intendPos.Item2];
            if (!CheckIfOneGridCanMove(grid, ignoreUnits)) return false;

            return true;
        }

        /// <summary>
        /// 怪物执行移动
        /// </summary>
        public void DoMove()
        {
            // 更新画面
            // var grid2DList = this.GetSystem<IMapSystem>().Grids();
            // var newGridTransPos = grid2DList[nextIntendPos.Item1, nextIntendPos.Item2].transform.position;
            // this.gameObject.transform.position = newGridTransPos;
            // monster.leftTopGridPos.Value = nextIntendPos;

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
                movementCoroutine = StartCoroutine(MoveToTarget(nextPos, data.moveAudioType));
            }
            else
            {
                transform.DOMove(nextPos, 0.3f).OnComplete(OnMoveFinish);
            }

            // 棋子在移动前可能因为回头攻击被转向
            PieceFlip(direction);


        }



        public override void Attack()
        {
            Debug.Log($"monster {this.pieceId.ToString()} is about to attack");
            // this.SendEvent<PieceAttackReadyEvent>();
            if (pieceAnimator != null)
            {
                foreach (AnimatorControllerParameter parameter in pieceAnimator.parameters)
                {
                    if (parameter.name == "isAttack")
                    {
                        StartCoroutine(PlayAttackAnimByAction(this));
                        break;
                    }
                }
            }
            this.SendCommand<PieceAttackCommand>(new PieceAttackCommand(this));
        }

        public override bool Hit(int damage, ViewPieceBase attacker)
        {
            this.SendEvent<PieceHitReadyEvent>(new PieceHitReadyEvent { piece = this });

            hp.Value -= damage;
            Debug.Log($"Monster Hit, damage: {damage} hp: {hp.Value}");
            MonsterDamageNumber.Spawn(this.Position(), damage);
            if(data.moveAudioType != AudioTypeEnum.Human)
            {
                GameManager.Instance.soundMan.Play_rand_hit_sound();
            }
            else
            {
                GameManager.Instance.soundMan.Play_rand_humanHit_sound();
            }
            // 播放受击动画
            GameObject anim = IdToSO.FindCardSOByID(attacker.generalId)?.GetAttackAnim();
            if (anim != null)
            {               
                StartCoroutine(PlayAttackAnimByMarking(GameObject.Instantiate(anim, this.transform)));
            }       
            
            
            this.SendEvent<PieceHitFinishEvent>(new PieceHitFinishEvent { piece = this });

            return hp.Value <= 0;
        }

        protected override void OnMoveReadyEvent(PieceMoveReadyEvent e)
        {
            base.OnMoveReadyEvent(e);
            Debug.Log("Monster recv OnMoveReadyEvent");
        }

        protected override void OnMoveFinishEvent(PieceMoveFinishEvent e)
        {
            base.OnMoveFinishEvent(e);
            Debug.Log("Monster recv OnMoveFinishEvent");
        }

        protected override void OnAttackStartEvent(PieceAttackStartEvent e)
        {
            // 若不是给自己的通知，不作相应
            if (e.viewPieceBase != this) return;
            ChangeStateTo(new PieceEnemyAttackingState(this));
        }

        protected override void OnAttackEndEvent(PieceAttackEndEvent e)
        {
            if (e.vpb != this) return;
            ChangeStateTo(new PieceEnemyMovingState(this));
        }

        protected override void OnUnderAttackEvent(PieceUnderAttackEvent e)
        {
            base.OnUnderAttackEvent(e);
        }

        private void MouseDown()
        {
            Debug.Log("mouse down monster");
        }

        private void MouseUp()
        {
            Debug.Log("mouse up monster");
            if (itemController.isMarking && itemController.markingType == typeof(Monster))
            {
                ItemController.Instance.markerFunction(this);
                itemController.CancelMarking();
                itemController.AfterUseCombatItem(itemController.markerItem);
            }
            else if(itemController.isMarking && itemController.markingType != typeof(Monster))
            {
                GameManager.Instance.soundMan.Play_cursor_click_invalid_sound();
            }
        }
        private void MouseEnter()
        {
            Debug.Log("mouse enter piece");
            UIKit.OpenPanel<PieceInfoPanel>();
            UIKit.GetPanel<PieceInfoPanel>().LoadPieceData(this);
        }

        private void MouseExit()
        {
            Debug.Log("mouse exit piece");
            UIKit.ClosePanel<PieceInfoPanel>();
        }
        public void SetColliderEnable(bool isEnable)
        {
            if(collider2d != null)  // 可能刚放下的情况
                collider2d.enabled = isEnable;
        }
    }
}



#if UNITY_EDITOR
/// <summary>
/// 怪物类自定义Inspector，显示部分关键以及ScriptableObject内的信息
/// </summary>
[CustomEditor(typeof(Monster))]
public class MonsterEditor : Editor
{
    private SerializedProperty _data;
    public List<FeatureEnum> _properties;
    public List<DirEnum> _dirs;
    private (int,int) _leftTopGridPos;
    private (int,int) _botRightGridPos;

    private void OnEnable()
    {
        // 获取 data 字段的 SerializedProperty
        _data = serializedObject.FindProperty("data");
        // 获取特殊类型的 BindableProperty
        _properties = ((Monster)target).features?.Value;
        _dirs = ((Monster)target).dirs.Value;
        _leftTopGridPos = ((Monster)target).leftTopGridPos.Value;
        _botRightGridPos = ((Monster)target).botRightGridPos.Value;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUILayout.Height(10));
        EditorGUILayout.LabelField("Bindable Properties:");
        var monster = (Monster)target;
        monster.moveSpeed.Value = EditorGUILayout.FloatField("Move Speed", monster.moveSpeed.Value);
        monster.hp.Value = EditorGUILayout.IntField("HP", monster.hp.Value);
        monster.maxHp.Value = EditorGUILayout.IntField("Max HP", monster.maxHp.Value);
        monster.atkSpeed.Value = EditorGUILayout.FloatField("Attack Speed", monster.atkSpeed.Value);
        monster.atkDmg.Value = EditorGUILayout.FloatField("Attack Damage", monster.atkDmg.Value);
        monster.defense.Value = EditorGUILayout.FloatField("Defense", monster.defense.Value);
        monster.accuracy.Value = EditorGUILayout.FloatField("Accuracy", monster.accuracy.Value);
        monster.atkRange.Value = EditorGUILayout.IntField("Attack Range", monster.atkRange.Value);
        monster.inCombat = EditorGUILayout.Toggle("In Combat", monster.inCombat);
        monster.isAttacking.Value = EditorGUILayout.Toggle("Is Attacking", monster.isAttacking.Value);
        monster.isDying.Value = EditorGUILayout.Toggle("Is Dying", monster.isDying.Value);

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
        int x = _leftTopGridPos.Item1;
        int y = _leftTopGridPos.Item2;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Left Top Grid Pos");
        x = EditorGUILayout.IntField(x);
        y = EditorGUILayout.IntField(y);
        EditorGUILayout.EndHorizontal();
        _leftTopGridPos = (x, y);

        int x2 = _botRightGridPos.Item1;
        int y2 = _botRightGridPos.Item2;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Bot right Grid Pos");
        x2 = EditorGUILayout.IntField(x2);
        y2 = EditorGUILayout.IntField(y2);
        EditorGUILayout.EndHorizontal();
        _leftTopGridPos = (x2, y2);

        // 更新 SerializedObject，以便可以显示和编辑脚本中的值
        serializedObject.Update();

        // 获取data中的变量
        var monsterData = _data.objectReferenceValue as SOMonsterBase;

        // 如果monsterData不为null，则显示变量
        if (monsterData != null)
        {
            EditorGUILayout.LabelField("", GUILayout.Height(15));
            EditorGUILayout.LabelField("初始信息【请勿修改】:");
            EditorGUILayout.ObjectField("Sprite: ", monsterData.monsterSprite, typeof(Sprite), false);
            EditorGUILayout.TextField("Movement speed: " + monsterData.moveSpeed);
            EditorGUILayout.TextField("Max HP: " + monsterData.maxHp);
            EditorGUILayout.TextField("Attack speed: " + monsterData.atkSpeed);
            EditorGUILayout.TextField("Attack damage: " + monsterData.atkDmg);
            EditorGUILayout.TextField("Defense: " + monsterData.defense);
            EditorGUILayout.TextField("Accuracy: " + monsterData.accuracy);
            EditorGUILayout.TextField("Rarity: " + monsterData.rarity);
            EditorGUILayout.TextField("Attack range: " + monsterData.atkRange);
            EditorGUILayout.TextField("Monster ID: " + monsterData.monsterId);
            EditorGUILayout.TextField("Properties: " + monsterData.properties);
            EditorGUILayout.TextField("Properties: " + monsterData.dirs);
            // int x3 = monsterData.pieceSize.Item1;
            // int y3 = monsterData.pieceSize.Item2;
            int x3 = monsterData.width;
            int y3 = monsterData.height;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Monster size");
            EditorGUILayout.IntField(x3);
            EditorGUILayout.IntField(y3);
            EditorGUILayout.EndHorizontal();
        }

        // 应用 SerializedObject 的更改
        serializedObject.ApplyModifiedProperties();
    }

}

#endif
