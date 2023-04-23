using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
public class SOPlayer : ScriptableObject
{
    [SerializeField] int gold; // 玩家金币
    [SerializeField] float goldAcquisitionMultiplier; // 玩家获取金币倍率

    // 玩家属性，力量，精神，技巧，体力，魅力
    [SerializeField] int strength;
    [SerializeField] int spirit;
    [SerializeField] int skill;
    [SerializeField] int stamina;
    [SerializeField] int chrisma;

    /// <summary>
    /// 获得玩家现有金币数量
    /// </summary>
    /// <returns>玩家金币数量</returns>
    public int GetGold()
    {
        return gold;
    }

    /// <summary>
    /// 获得金币
    /// </summary>
    /// <param name="amount">金币数量</param>
    public void AddGold(int amount)
    {
        gold += amount;
    }

    /// <summary>
    /// 获得玩家五维其中之一
    /// </summary>
    /// <param name="stats">需要返回的属性，力量，精神，技巧，体力，魅力</param>
    /// <returns>五维其中之一，错误时返回-1</returns>
    public int GetStats(PlayerStats stats)
    {
        switch (stats)
        {
            case PlayerStats.Strength: return strength;
            case PlayerStats.Spirit: return spirit;
            case PlayerStats.Skill: return skill;
            case PlayerStats.Stamina: return stamina;
            case PlayerStats.Charisma: return chrisma;
        }
        return -1;
    }

    /// <summary>
    /// 改变玩家五维其中之一
    /// </summary>
    /// <param name="stats">需要改变的属性，力量，精神，技巧，体力，魅力</param>
    /// <param name="amount">要改变的数值</param>
    public void ModifyStats(PlayerStats stats, int amount)
    {
        switch (stats)
        {
            case PlayerStats.Strength: strength += amount; return;
            case PlayerStats.Spirit: spirit += amount; return;
            case PlayerStats.Skill: skill += amount;  return;
            case PlayerStats.Stamina: stamina += amount; return;
            case PlayerStats.Charisma: chrisma += amount; return;
        }
    }
}
