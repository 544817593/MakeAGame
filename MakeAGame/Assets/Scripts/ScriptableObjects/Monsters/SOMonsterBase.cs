using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOMonsterBase : ScriptableObject
{
    // pieceSize
    public string monsterName;
    public Sprite monsterSprite; // 立绘
    public float moveSpeed; // 移动速度
    public int maxHp; // 最大生命值
    public float atkSpeed; // 攻速
    public float atkDmg; // 攻击力
    public float defense; // 防御力
    public float accuracy; // 命中率
    public RarityEnum rarity; // 稀有度 0 白 -- 4 橙
    public int atkRange; // 射程
    public int monsterId; // 怪物的ID，辨认品种
    public int width;   // 怪物的宽
    public int height;  // 怪物的长
    // public (int,int) pieceSize; // 怪物的尺寸
    public AudioTypeEnum moveAudioType;
    public List<FeatureEnum> properties; // 特性
    public List<DirEnum> dirs; // 可移动方向
    public List<FeatureEnum> specialFeatures; // 额外属性
    public GameObject anim; // 角色动画
    public GameObject attackAnim; // 攻击动画

   public GameObject GetAnim()
    {
        return anim;
    }

    public GameObject GetAttackAnim()
    {
        if (attackAnim != null)
        {
            return attackAnim;
        }
        else
        {
            return null;
        }
    }
}
