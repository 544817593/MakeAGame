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
    private float lastMoveTime; // 上一次移动的时间

    void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        movementSystem = this.GetSystem<IMovementSystem>();
        
    }

    void Update()
    {
        if (Time.time - lastMoveTime > monster.moveSpeed)
        {
            FindMovementDir();
            lastMoveTime = Time.time;
        }
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
        (int, int) original = monster.leftTopGridPos.Value; // 当前左上坐标
        (int, int) positionAfterMovement = monster.leftTopGridPos.Value; // 怪物移动后的坐标

        // A* 寻路
        List<BoxGrid> aStarPath = PathFinding.FindPath(original.Item1, original.Item2,
            monster.currentTarget.leftTopGridPos.Item1, monster.currentTarget.leftTopGridPos.Item2, monster);

        // 路径存在
        if (aStarPath != null)
        {
            // 场景显示路线
            Color randColor = UnityEngine.Random.ColorHSV();
            for (int i = 0; i < aStarPath.Count - 1; i++)
            {
                Debug.DrawLine(aStarPath[i].transform.position - new Vector3(0, 0, 0.3f), aStarPath[i + 1].transform.position - new Vector3(0, 0, 0.3f), randColor, 3f);
            }

            // 设置移动方向
            monster.currentDir = movementSystem.NeighbourBoxGridsToDir(this.GetSystem<IMapSystem>().Grids()
                [monster.leftTopGridPos.Value.Item1, monster.leftTopGridPos.Value.Item2], aStarPath[1]);
        }
    }

    public void DoMove()
    {

    }
}

