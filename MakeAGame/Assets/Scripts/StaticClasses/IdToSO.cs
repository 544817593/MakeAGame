using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IdToSO
{
    // 角色SO列表
    public static List<ScriptableObject> soCharacterList = Resources.LoadAll<ScriptableObject>("ScriptableObjects/Characters").ToList();

    /// <summary>
    /// 通过卡牌id来返还卡牌的SO
    /// </summary>
    /// <param name="id">卡牌ID</param>
    /// <returns></returns>
    public static SOCharacterInfo FindCardSOByID(int id)
    {
        foreach (ScriptableObject character in soCharacterList)
        {
            SOCharacterInfo soCharacterInfo = (SOCharacterInfo)character;
            if (soCharacterInfo.characterID == id) return soCharacterInfo;
        }
        return null;
    }
}
