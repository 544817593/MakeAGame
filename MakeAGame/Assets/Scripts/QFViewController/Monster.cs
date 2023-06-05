using Game;
using QFramework;
using System;
using System.Collections.Generic;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



public class Monster : ViewPieceBase
{
    // 怪物初始SO数据
    public SOMonsterBase data;

    #region 怪物数据
    //public int rarity; // 稀有度 0 白 -- 4 橙
    //public int monsterId; // 怪物的ID，辨认品种
    //public int pieceId; // 棋子ID，每个棋子独一份
    //public (int, int) pieceSize; // 怪物尺寸

    //public BindableProperty<float> moveSpeed; // 移动速度
    //public BindableProperty<int> hp; // 当前生命值
    //public BindableProperty<int> maxHp; // 最大生命值
    //public BindableProperty<float> atkSpeed; // 攻速
    //public BindableProperty<float> atkDmg; // 攻击力
    //public BindableProperty<float> defense; // 防御力
    //public BindableProperty<float> accuracy; // 命中率
    //public BindableProperty<int> atkRange; // 射程
    //public BindableProperty<List<PropertyEnum>> features; // 特性
    //public BindableProperty<List<DirEnum>> dirs; // 可移动方向
    //public BindableProperty<bool> isAttacking; // 是否在发起攻击
    //public BindableProperty<bool> isDying; // 是否正在死亡中                         
    //public BindableProperty<(int, int)> leftTopGridPos; // 怪物当前左上角位置
    //public BindableProperty<(int, int)> botRightGridPos; // 怪物当前右下角位置
    #endregion

    public TempAllyScript currentTarget; // 当前目标
    // public DirEnum currentDir = DirEnum.None; // 当前移动方向

    // private IMapSystem mapSystem; // 地图系统
    // private float oldSpeed; // 记录移动速度被改变前的旧值

    void Awake()
    {
        mapSystem = this.GetSystem<IMapSystem>();
    }
    
    void Start()
    {
        base.Start();
    
        // 注意顺序，先放action，再regsiter
        this.RegisterEvent<PieceMoveReadyEvent>(OnPieceMoveReady).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<PieceMoveFinishEvent>(OnPieceMoveFinish).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<PieceAttackStartEvent>(OnPieceAttackStart).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<PieceAttackEndEvent>(OnPieceAttackEnd).UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<PieceUnderAttackEvent>(OnPieceUnderAttack).UnRegisterWhenGameObjectDestroyed(gameObject);
        
        this.SendCommand(new MonsterTargetSelectionCommand(this));
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

    // 更新速度的过程换到PieceEnemyMovingState中
    // private void OnBeforeMonsterMoveSpeedChanged(float oldSpeed)
    // {
    //     this.oldSpeed = oldSpeed;
    // }

    // private void OnMonsterMoveSpeedChanged(float newMoveSpeed)
    // {
    //     // 移动速度改变时同时改变移动冷却计时器
    //     float differential = newMoveSpeed - oldSpeed;
    //
    //     gameObject.GetComponent<MonsterMovement>().movementCooldown += differential;
    // }

    /// <summary>
    /// 怪物位置改变前一瞬间
    /// </summary>
    /// <param name="oldPosition">原位置</param>
    // private void OnBeforeMonsterPositionChanged((int, int) oldPosition)
    // {
    //     // 更新格子上储存的信息
    //     // mapSystem.Grids()[oldPosition.Item1, oldPosition.Item2].occupation = 0;
    // }

    /// <summary>
    /// 怪物改变位置后一瞬间
    /// </summary>
    /// <param name="newPosition">新位置</param>
    // private void OnMonsterPositionChanged((int,int) newPosition)
    // {
    //     // 更新格子上储存的信息
    //     // mapSystem.Grids()[newPosition.Item1, newPosition.Item2].occupation = pieceId;       
    //     foreach (var oldGrid in pieceGrids) oldGrid.occupation = pieceId;
    // }
    
    public void Move()
    {
        if (movementSystem == null)
            movementSystem = this.GetSystem<IMovementSystem>();
        
        // 发送准备移动事件
        this.SendEvent<PieceMoveReadyEvent>(new PieceMoveReadyEvent() {viewPieceBase = this});
        
        var nextLTCorr = FindMovementDir();
        
        // 无法移动
        if (nextLTCorr.Item1 == -1 && nextLTCorr.Item2 == -1)
        {
            return;
        }
        
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

    private IMovementSystem movementSystem;
    /// <summary>
    /// 根据目标，找到怪物的下一个移动方向
    /// 返回移动后的左下角格子坐标，若无法移动，则返回(-1, -1)
    /// </summary>
    private (int, int) FindMovementDir()
    {
        (int, int) original = leftTopGridPos.Value; // 当前左上坐标
        (int, int) positionAfterMovement = leftTopGridPos.Value; // 怪物移动后的坐标

        // 若目标不存在或就是当前位置
        if (currentTarget == null || currentTarget.leftTopGridPos == original)
            return (-1, -1);
        

        // A* 寻路
        List<BoxGrid> aStarPath = PathFinding.FindPath(original.Item1, original.Item2,
            currentTarget.leftTopGridPos.Item1, currentTarget.leftTopGridPos.Item2, this);

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
            direction = movementSystem.NeighbourBoxGridsToDir(this.GetSystem<IMapSystem>().Grids()
                [leftTopGridPos.Value.Item1, leftTopGridPos.Value.Item2], aStarPath[1]);

            // 更新想要去的格子
            positionAfterMovement = this.GetSystem<IMovementSystem>().CalculateNextPosition(original, direction);
            // nextIntendPos = positionAfterMovement;
            return positionAfterMovement;
        }
        else
        {
            return (-1, -1);
        }
    }
    
    /// <summary>
    /// 根据行走方向和下一位置的情况，检查是否可以移动
    /// </summary>
    /// <param name="curMoveDir"></param>
    /// <param name="currentX"></param>
    /// <param name="currentY"></param>
    /// <returns></returns>
    public bool CheckIfMovable(DirEnum curMoveDir, int currentX, int currentY)
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
        if (!CheckIfOneGridCanMove(grid)) return false;

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
        var nextPos = GetGridsCenterPos();
        transform.DOMove(nextPos, 0.3f).OnComplete(OnMoveFinish);
    }

    void OnMoveFinish()
    {
        // 发送结束移动事件
        GetArchitecture().SendEvent<PieceMoveFinishEvent>(new PieceMoveFinishEvent() {viewPieceBase = this});
    }
    
    public override void Attack()
    {
        Debug.Log($"monster {this.ToString()} is about to attack");
        // todo attack
        this.SendEvent<PieceAttackReadyEvent>();
        this.SendCommand<PieceAttackCommand>(new PieceAttackCommand(this));
    }

    public override bool Hit()
    {
        this.SendEvent<PieceHitReadyEvent>();

        hp.Value -= 1;
        Debug.Log($"Monster Hit, hp: {hp.Value}");
        
        this.SendEvent<PieceHitFinishEvent>();

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
}


#if UNITY_EDITOR
/// <summary>
/// 怪物类自定义Inspector，显示ScriptableObject内的信息
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
