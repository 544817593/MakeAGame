using Game;
using QFramework;
using System;
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
        ViewPiece targetAlly = null;
        List<ViewPiece> allyList = this.GetSystem<IPieceSystem>().pieceFriendList; // 当前友军列表
        List<ViewPiece> targetList = new List<ViewPiece>(); // 会影响怪物移动的目标

        foreach (ViewPiece piece in allyList)
        {
            if (piece.generalId == 0 || piece.rarity == RarityEnum.Orange) targetList.Add(piece);
        }

        int targetDist = int.MaxValue;
        if (targetList.Count > 0)
        {
            (int, int) monsterPosition = (monster.pieceGrids[0].row, monster.pieceGrids[0].col);
            // 在目标列表里寻找最近的
            foreach (ViewPiece targetPiece in targetList)
            {
                (int, int) targetPosition = (targetPiece.pieceGrids[0].row, targetPiece.pieceGrids[0].col);
                int dist = ManhattanDist(monsterPosition, targetPosition);
                if (dist < targetDist) targetAlly = targetPiece;
                targetDist = dist;
            }
        }
        monster.currentTarget = targetAlly;
    }

    public int ManhattanDist((int,int) original, (int, int) target)
    {
        return Mathf.Abs(target.Item1 - original.Item1) + Mathf.Abs(target.Item2 - original.Item2);
    }


}
