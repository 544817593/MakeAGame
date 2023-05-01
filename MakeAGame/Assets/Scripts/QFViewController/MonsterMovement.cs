using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;


public class MonsterMovement : MonoBehaviour, IController
{
    IMovementSystem movementSystem; // 移动系统
    private Monster monster; // 怪物引用
    private (int, int) nextPos; // 下一次移动即将去到的位置

    void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        movementSystem = this.GetSystem<IMovementSystem>();
        FindMovementDir();
    }

    /// <summary>
    /// 获取Architecture 每个IController都要写
    /// </summary>
    /// <returns></returns>
    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }

    /// <summary>
    /// 根据行走方向和下一位置的情况，检查是否可以移动
    /// </summary>
    /// <param name="curMoveDir"></param>
    /// <param name="currentX"></param>
    /// <param name="currentY"></param>
    /// <returns></returns>
    public bool CheckIfMovable(DirEnum curMoveDir, int currentX, int currentY)
    {
        if (monster.isAttacking)
        {
            return false;
        }

        // 获取下一步坐标
        (int, int) intendPos = movementSystem.CalculateNextPosition((currentX, currentY), curMoveDir);
        // 对下一步坐标做基础检查
        if (!movementSystem.MovementBaseCheck(intendPos)) return false;

        return true;
    }

    /// <summary>
    /// 根据目标，找到怪物的下一个移动方向
    /// </summary>
    private void FindMovementDir()
    {
        (int, int) original = monster.leftTopGridPos; // 当前左上坐标
        (int, int) positionAfterMovement = monster.leftTopGridPos; // 怪物移动后的坐标


        // A star path finding
        List<BoxGrid> aStarPath = PathFinding.FindPath(monster.leftTopGridPos.Value.Item1, monster.leftTopGridPos.Value.Item2,
            monster.currentTarget.leftTopGridPos.Item1, monster.currentTarget.leftTopGridPos.Item2, monster);

        // A path to the target exists
        if (aStarPath != null)
        {
            // Show the movement path in scene window
            Color randColor = UnityEngine.Random.ColorHSV();
            for (int i = 0; i < aStarPath.Count - 1; i++)
            {
                Debug.DrawLine(aStarPath[i].transform.position - new Vector3(0, 0, 0.3f), aStarPath[i + 1].transform.position - new Vector3(0, 0, 0.3f), randColor, 3f);
            }

            // Set movement direction
            monster.currentDir = movementSystem.NeighbourBoxGridsToDir(this.GetSystem<IMapSystem>().Grids()
                [monster.leftTopGridPos.Value.Item1, monster.leftTopGridPos.Value.Item2], aStarPath[1]);
        }
    }
}

