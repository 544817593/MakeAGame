using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
using UnityEditor;


public class MonsterMovement : MonoBehaviour, IController
{
    IMovementSystem movementSystem; // 移动系统
    private Monster monster; // 怪物引用

    public float movementCooldown; // 移动时间冷却，冷却完毕即可移动，每次移动后重置为怪物移动速度
    private float lastUpdateTime; // 上一次触发Update函数的时间
    private float timeSinceUpdate; // 上一次触发Update函数后经过了多久
    private BoxGrid[,] grid2dList; // 地图格子列表

    private (int, int) nextIntendPos; // 下一个想要去到的格子

    public Animator animator; // 移动动画组件


    void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        movementCooldown = monster.moveSpeed;
        movementSystem = this.GetSystem<IMovementSystem>();
        grid2dList = this.GetSystem<IMapSystem>().Grids();
        lastUpdateTime = Time.time;
     
    }

    void Update()
    {
        timeSinceUpdate = Time.time - lastUpdateTime; // 计算两次Update的时间差
        // 根据格子时间倍率减少怪物的移动冷却时间
        movementCooldown -= (timeSinceUpdate * grid2dList[monster.leftTopGridPos.Value.Item1, 
            monster.leftTopGridPos.Value.Item2].timeMultiplier.Value.ToTimeMultiplierFloat());

        // 冷却完毕
        if (movementCooldown <= 0)
        {
            FindMovementDir();
            DoMove();
            movementCooldown = monster.moveSpeed; // 移动冷却时间重置为移动速度
        }
        lastUpdateTime = Time.time;
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
        if (aStarPath != null && aStarPath.Count != 0)
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

            // 更新想要去的格子
            positionAfterMovement = this.GetSystem<IMovementSystem>().CalculateNextPosition(original, monster.currentDir);
            nextIntendPos = positionAfterMovement;
        }
    }

    /// <summary>
    /// 怪物执行移动
    /// </summary>
    public void DoMove()
    {
        // 更新画面
        var grid2DList = this.GetSystem<IMapSystem>().Grids();
        var newGridTransPos = grid2DList[nextIntendPos.Item1, nextIntendPos.Item2].transform.position;
        this.gameObject.transform.position = newGridTransPos;
        monster.leftTopGridPos.Value = nextIntendPos;

        animator.SetBool("isMove", true);
        animator.SetBool("isMove", false);

    }

}

