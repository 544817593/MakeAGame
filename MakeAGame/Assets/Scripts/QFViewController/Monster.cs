using Game;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;



public class Monster : MonoBehaviour, IController
{
    // 怪物初始SO数据
    public SOMonsterBase data;

    #region 怪物数据
    public Sprite monsterSprite; // 立绘
    public int rarity; // 稀有度 0 白 -- 4 橙
    public int monsterId; // 怪物的ID，辨认品种
    public int pieceId; // 棋子ID，每个棋子独一份

    public BindableProperty<float> moveSpeed; // 移动速度
    public BindableProperty<float> hp; // 当前生命值
    public BindableProperty<float> maxHp; // 最大生命值
    public BindableProperty<float> atkSpeed; // 攻速
    public BindableProperty<float> atkDmg; // 攻击力
    public BindableProperty<float> defense; // 防御力
    public BindableProperty<float> accuracy; // 命中率
    public BindableProperty<int> atkRange; // 射程
    public BindableProperty<List<Property>> properties; // 特性
    public BindableProperty<bool> inCombat; // 是否在战斗中
    public BindableProperty<bool> isAttacking; // 是否在发起攻击
    public BindableProperty<bool> isDying; // 是否正在死亡中                         
    public BindableProperty<(int, int)> leftTopGridPos; // 怪物当前左上角位置
    #endregion

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

    }

    void Start()
    {
        leftTopGridPos.Register(newPosition => OnMonsterPositionChanged());
    }

    private void OnMonsterPositionChanged()
    {

    }

}

/// <summary>
/// 怪物类自定义Inspector，显示ScriptableObject内的信息
/// </summary>
[CustomEditor(typeof(Monster))]
public class MonsterEditor : Editor
{
    private SerializedProperty _data;
    public List<Property> _properties;
    private (int,int) _leftTopGridPos;

    private void OnEnable()
    {
        // 获取 data 字段的 SerializedProperty
        _data = serializedObject.FindProperty("data");
        // 获取特殊类型的 BindableProperty
        _properties = ((Monster)target).properties.Value;
        _leftTopGridPos = ((Monster)target).leftTopGridPos.Value;

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
            _properties[i] = (Property)EditorGUILayout.EnumPopup("Property " + i, _properties[i]);
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
        }

        // 应用 SerializedObject 的更改
        serializedObject.ApplyModifiedProperties();
    }

}
