using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOMonsterBase : ScriptableObject
{
    // pieceSize
    // moveDir
    // leftTopGridPos
    // grids
    public SpriteRenderer monsterSprite; // 立绘
    public float moveSpeed; // 移动速度
    public float hp; // 当前生命值
    public float maxHp; // 最大生命值
    public float atkSpeed; // 攻速
    public float atkDmg; // 攻击力
    public float defense; // 防御力
    public float accuracy; // 命中率
    public int rarity; // 稀有度 0 白 -- 4 橙
    public int atkRange; // 射程
    public int monsterId; // 怪物的ID，辨认品种
    public int pieceId; // 棋子ID，每个棋子独一份
    public List<Property> properties; // 特性
    public bool inCombat; // 是否在战斗中
    public bool isAttacking; // 是否在发起攻击
    public bool isDying; // 是否正在死亡中

    
}
