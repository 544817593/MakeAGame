using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class IdToSO
{
    // 角色SO列表
    public static SOCharacterInfo[] soCharacterList = Resources.LoadAll<SOCharacterInfo>("ScriptableObjects/Characters");

    /// <summary>
    /// 通过卡牌id来返还卡牌的SO
    /// </summary>
    /// <param name="id">卡牌ID</param>
    /// <returns></returns>
    public static SOCharacterInfo FindCardSOByID(int id)
    {
        foreach (SOCharacterInfo character in soCharacterList)
        {
            if (character.characterID == id) return character;
        }
        return null;
    }
}

