using Game;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IController
{
    // 怪物基础数据
    public BindableProperty<ScriptableObject> data = new BindableProperty<ScriptableObject>();
    // 怪物当前左上角位置
    public BindableProperty<(int, int)> leftTopGridPos;

    /// <summary>
    /// 获取Architecture 每个IController都要写
    /// </summary>
    /// <returns></returns>
    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }

    // Start is called before the first frame update
    void Start()
    {
        leftTopGridPos.RegisterWithInitValue(position => OnMonsterPositionChanged());
    }

    private void OnMonsterPositionChanged()
    {

    }
}
