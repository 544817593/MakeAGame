using Game;
using QFramework;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;



public class Monster : MonoBehaviour, IController
{
    // 怪物初始SO数据
    public SOMonsterBase data;

    #region 怪物数据
    public int rarity; // 稀有度 0 白 -- 4 橙
    public int monsterId; // 怪物的ID，辨认品种
    public int pieceId; // 棋子ID，每个棋子独一份
    public (int, int) pieceSize; // 怪物尺寸

    public BindableProperty<float> moveSpeed; // 移动速度
    public BindableProperty<float> hp; // 当前生命值
    public BindableProperty<float> maxHp; // 最大生命值
    public BindableProperty<float> atkSpeed; // 攻速
    public BindableProperty<float> atkDmg; // 攻击力
    public BindableProperty<float> defense; // 防御力
    public BindableProperty<float> accuracy; // 命中率
    public BindableProperty<int> atkRange; // 射程
    public BindableProperty<List<PropertyEnum>> properties; // 特性
    public BindableProperty<List<DirEnum>> dirs; // 特性
    public BindableProperty<bool> inCombat; // 是否在战斗中
    public BindableProperty<bool> isAttacking; // 是否在发起攻击
    public BindableProperty<bool> isDying; // 是否正在死亡中                         
    public BindableProperty<(int, int)> leftTopGridPos; // 怪物当前左上角位置
    public BindableProperty<(int, int)> botRightGridPos; // 怪物当前右下角位置
    #endregion

    public TempAllyScript currentTarget; // 当前目标
    public DirEnum currentDir = DirEnum.None; // 当前移动方向

    private IMapSystem mapSystem; // 地图系统
    private float oldSpeed; // 记录移动速度被改变前的旧值

    /// <summary>
    /// 获取Architecture 每个IController都要写
    /// </summary>
    /// <returns></returns>
    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }

    void Awake()
    {
        mapSystem = this.GetSystem<IMapSystem>();
    }

    void Start()
    {
        leftTopGridPos.RegisterWithInitValue(newPosition => OnMonsterPositionChanged(newPosition));
        leftTopGridPos.RegisterBeforeValueChanged(oldPosition => OnBeforeMonsterPositionChanged(oldPosition));
        moveSpeed.Register(newMoveSpeed => OnMonsterMoveSpeedChanged(newMoveSpeed));
        moveSpeed.RegisterBeforeValueChanged(oldSpeed => OnBeforeMonsterMoveSpeedChanged(oldSpeed));
        this.SendCommand(new MonsterTargetSelectionCommand(this));
    }

    private void OnBeforeMonsterMoveSpeedChanged(float oldSpeed)
    {
        this.oldSpeed = oldSpeed;
    }

    private void OnMonsterMoveSpeedChanged(float newMoveSpeed)
    {
        // 移动速度改变时同时改变移动冷却计时器
        float differential = newMoveSpeed - oldSpeed;

        gameObject.GetComponent<MonsterMovement>().movementCooldown += differential;
    }

    /// <summary>
    /// 怪物位置改变前一瞬间
    /// </summary>
    /// <param name="oldPosition">原位置</param>
    private void OnBeforeMonsterPositionChanged((int, int) oldPosition)
    {
        // 更新格子上储存的信息
        mapSystem.Grids()[oldPosition.Item1, oldPosition.Item2].occupation = 0;
    }

    /// <summary>
    /// 怪物改变位置后一瞬间
    /// </summary>
    /// <param name="newPosition">新位置</param>
    private void OnMonsterPositionChanged((int,int) newPosition)
    {
        // 更新格子上储存的信息
        mapSystem.Grids()[newPosition.Item1, newPosition.Item2].occupation = pieceId;       
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
    public List<PropertyEnum> _properties;
    public List<DirEnum> _dirs;
    private (int,int) _leftTopGridPos;
    private (int,int) _botRightGridPos;

    private void OnEnable()
    {
        // 获取 data 字段的 SerializedProperty
        _data = serializedObject.FindProperty("data");
        // 获取特殊类型的 BindableProperty
        _properties = ((Monster)target).properties.Value;
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
        monster.hp.Value = EditorGUILayout.FloatField("HP", monster.hp.Value);
        monster.maxHp.Value = EditorGUILayout.FloatField("Max HP", monster.maxHp.Value);
        monster.atkSpeed.Value = EditorGUILayout.FloatField("Attack Speed", monster.atkSpeed.Value);
        monster.atkDmg.Value = EditorGUILayout.FloatField("Attack Damage", monster.atkDmg.Value);
        monster.defense.Value = EditorGUILayout.FloatField("Defense", monster.defense.Value);
        monster.accuracy.Value = EditorGUILayout.FloatField("Accuracy", monster.accuracy.Value);
        monster.atkRange.Value = EditorGUILayout.IntField("Attack Range", monster.atkRange.Value);
        monster.inCombat.Value = EditorGUILayout.Toggle("In Combat", monster.inCombat.Value);
        monster.isAttacking.Value = EditorGUILayout.Toggle("Is Attacking", monster.isAttacking.Value);
        monster.isDying.Value = EditorGUILayout.Toggle("Is Dying", monster.isDying.Value);

        // 特性
        for (int i = 0; i < _properties.Count; i++)
        {
            _properties[i] = (PropertyEnum)EditorGUILayout.EnumPopup("Property " + i, _properties[i]);
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
            int x3 = monsterData.pieceSize.Item1;
            int y3 = monsterData.pieceSize.Item2;
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
