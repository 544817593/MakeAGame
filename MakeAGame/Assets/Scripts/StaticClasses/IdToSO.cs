using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class IdToSO
{
    // 角色SO列表
    public static SOCharacterInfo[] soCharacterList = Resources.LoadAll<SOCharacterInfo>("ScriptableObjects/LubanCharacter");
    public static SOFeature[] soFeatureList = Resources.LoadAll<SOFeature>("ScriptableObjects/LubanFeature");
    public static SORelic[] soRelicList = Resources.LoadAll<SORelic>("ScriptableObjects/LubanRelic");
    public static SOItemBase[] soItemList = Resources.LoadAll<SOItemBase>("ScriptableObjects/Items");

    /// <summary>
    /// 通过卡牌id来返还卡牌的SO
    /// </summary>
    /// <param name="id">卡牌ID</param>
    /// <returns></returns>
    public static SOCharacterInfo FindCardSOByID(int id, bool canReturnNull = true)
    {
        foreach (SOCharacterInfo character in soCharacterList)
        {
            if (character.characterID == id) return character;
        }
        
        // 若不能返回空值，返回默认id的so
        if (canReturnNull)
            return null;
        else
            return soCharacterList.ToList().Find(so => so.characterID == 1);
    }

    /// <summary>
    /// 通过SOEnum来返还特性的SO
    /// </summary>
    /// <param name="id">卡牌ID</param>
    /// <returns></returns>
    public static SOFeature FindFeatureSOByEnum(FeatureEnum property, bool canReturnNull = true)
    {
        foreach (SOFeature feature in soFeatureList)
        {
            if (feature.typeName == property.ToString()) return feature;
        }

        // 若不能返回空值，返回默认id的so
        if (canReturnNull)
            return null;
        else
            return soFeatureList.ToList().Find(so => so.featureID == 1);
    }

    public static SOCharacterInfo GetRandomCardSO()
    {
        int index = Random.Range(0, soCharacterList.Length);
        return soCharacterList[index];
    }

    public static SORelic FindRelicSOByID(int id)
    {
        var so = soRelicList.ToList().Find(i => i.relicID == id);
        if (so == null)
        {
            Debug.LogError("unvalid relic id");
        }

        return so;
    }
}

