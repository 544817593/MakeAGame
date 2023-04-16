using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance; // 怪物管理器实例

    /// <summary>
    /// 怪物管理器Getter
    /// </summary>
    private static MonsterManager Instance { get { return instance; } }

    public List<GameObject> monsterList; // 当前存在的怪物列表

    private void Awake()
    {
        // 确保单例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
