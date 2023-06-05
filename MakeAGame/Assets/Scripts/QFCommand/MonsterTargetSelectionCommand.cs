using Game;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

/// <summary>
/// 怪物选择目标的具体操作
/// </summary>
public class MonsterTargetSelectionCommand : AbstractCommand
{
    private Monster monster; // 需要的怪物信息

    public MonsterTargetSelectionCommand(Monster monster)
    {
        this.monster = monster;
    }

    protected override void OnExecute()
    {
        TempAllyScript targetAlly = null;
        var targetGO = GameObject.Find("TestAlly");
        if (targetGO == null)
        {
            Debug.LogError("TestAlly is null");
        }
        else
        {
            targetAlly = GameObject.Find("TestAlly").GetComponent<TempAllyScript>(); // 怪物的当前目标   
        }

        List<TempAllyScript> allyList = this.GetSystem<ISpawnSystem>().GetAllyList(); // 当前友军列表
        List<TempAllyScript> targetList = new List<TempAllyScript>(); // Ally pieces that direct monster's movement (investigator/undead)
        monster.currentTarget = targetAlly;
    }


}
