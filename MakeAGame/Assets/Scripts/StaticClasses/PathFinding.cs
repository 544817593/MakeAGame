using QFramework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Game
{

    public static class PathFinding
    {
        private static List<BoxGrid> toSearchList; // 即将被检查的格子
        private static List<BoxGrid> searchedList; // 检查过的格子

        /// <summary>
        /// 修改版的A*寻路
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="monster"></param>
        /// <param name="ignoreUnits">寻找路线时是否无视单位挡路</param>
        /// <returns></returns>
        public static List<BoxGrid> FindPath(int startX, int startY, int endX, int endY, Monster monster, bool ignoreUnits = false)
        {
            IMapSystem mapSystem = GameEntry.Interface.GetSystem<IMapSystem>();
            BoxGrid[,] grid2DList = mapSystem.Grids();
            toSearchList = new List<BoxGrid> { grid2DList[startX, startY] }; // 即将被检查的格子列表
            searchedList = new List<BoxGrid>(); // 检查过的格子

            // 初始化，gCost和fCost初始值为正无穷
            for (int x = 0; x < grid2DList.GetLength(0); x++)
            {
                for (int y = 0; y < grid2DList.GetLength(1); y++)
                {
                    grid2DList[x, y].gCost = int.MaxValue;
                    CalculateFCost(grid2DList[x, y]);
                    grid2DList[x, y].cameFrom = null;
                }
            }

            // 为初始格子设定值
            grid2DList[startX, startY].gCost = 0;
            grid2DList[startX, startY].hCost = EstimateDistCost(grid2DList[startX, startY], grid2DList[endX, endY], monster);
            CalculateFCost(grid2DList[startX, startY]);

            // 如果还有可以检查的格子就继续
            while (toSearchList.Count > 0)
            {
                // 寻找消耗最少的格子
                BoxGrid currentBoxGrid = GetLowestFCostBox(toSearchList);
                // 如果这个格子是目标格子，统计路径并返回
                if (currentBoxGrid == grid2DList[endX, endY])
                {
                    return CalculatePath(grid2DList[endX, endY]);
                }

                // 更新已检查和未检查的格子列表
                toSearchList.Remove(currentBoxGrid);
                searchedList.Add(currentBoxGrid);


                // 根据单位的可移动方向寻找所有能到达的格子
                List<BoxGrid> neighbourList = GetNeighbourList(currentBoxGrid, monster, grid2DList, (endX, endY), ignoreUnits);
                foreach (BoxGrid neighbourBoxGrid in neighbourList)
                {
                    if (searchedList.Contains(neighbourBoxGrid)) continue;
                    // 当前禁止到达的格子（比如格子上有单位）排除在外
                    if (!mapSystem.GridCanMoveTo(neighbourBoxGrid, ignoreUnits) && (neighbourBoxGrid.row, neighbourBoxGrid.col) != (endX, endY))
                    {
                        searchedList.Add(neighbourBoxGrid);
                        continue;
                    }


                    // 跳过超出边界的格子
                    if ((neighbourBoxGrid.row, neighbourBoxGrid.col).Item1 > mapSystem.Grids().GetLength(0) 
                        || (neighbourBoxGrid.row, neighbourBoxGrid.col).Item2 > mapSystem.Grids().GetLength(1)) { searchedList.Add(neighbourBoxGrid); continue; }

                    // 检查有没有更好的路径
                    int tempGCost = currentBoxGrid.gCost + EstimateDistCost(currentBoxGrid, neighbourBoxGrid, monster);
                    if (tempGCost < neighbourBoxGrid.gCost)
                    {
                        neighbourBoxGrid.cameFrom = currentBoxGrid;
                        neighbourBoxGrid.gCost = tempGCost;
                        neighbourBoxGrid.hCost = EstimateDistCost(neighbourBoxGrid, grid2DList[endX, endY], monster);
                        CalculateFCost(neighbourBoxGrid);

                        if (!toSearchList.Contains(neighbourBoxGrid))
                        {
                            toSearchList.Add(neighbourBoxGrid);
                        }
                    }

                }
            }

            return null;
        }

        /// <summary>
        /// 怪物没有无遮挡可直达的路线，寻找一个最合适的格子进行移动
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="monster"></param>
        /// <returns></returns>
        public static BoxGrid FindGridClosestToTarget(int startX, int startY, int endX, int endY, Monster monster)
        {
            // 如果有格子能让怪物离目标更近，那么前进
            BoxGrid[,] grid2DList = GameEntry.Interface.GetSystem<IMapSystem>().Grids();
            List<BoxGrid> boxGrids = GetNeighbourList(grid2DList[startX, startY], monster, grid2DList, (endX, endY));
            int distance = EstimateDistCost(grid2DList[endX, endY], grid2DList[startX, startY], monster);
            BoxGrid bestBoxGrid = grid2DList[startX, startY];
            foreach(BoxGrid boxGrid in boxGrids)
            {
                int tempDistance = EstimateDistCost(grid2DList[endX, endY], boxGrid, monster);
                if (tempDistance < distance)
                {
                    bestBoxGrid = boxGrid;
                    distance = tempDistance;
                }
            }

            // 如果没有，那么寻找没有棋子挡路情况下的最佳路线，如果可以，向该路线的下一个格子点前进
            if (bestBoxGrid == grid2DList[startX, startY])
            {
                List<BoxGrid> theoreticalPath = FindPath(startX, startY, endX, endY, monster, true);
                if (GameEntry.Interface.GetSystem<IMapSystem>().GridCanMoveTo(theoreticalPath[1]))
                {
                    bestBoxGrid = theoreticalPath[1];
                }
            }
            return bestBoxGrid;
        }

        // 根据单位的可移动方向寻找所有能到达的格子
        public static List<BoxGrid> GetNeighbourList(BoxGrid currentBoxGrid, Monster monster, BoxGrid[,] grid2DList, (int, int) targetPos, bool ignoreUnits = false)
        {
            List<BoxGrid> neighbourList = new List<BoxGrid>();
            IMovementSystem movementSystem = GameEntry.Interface.GetSystem<IMovementSystem>();

            foreach (DirEnum dir in monster.dirs.Value)
            {
                if (monster.CheckIfMovable(dir, currentBoxGrid.row, currentBoxGrid.col, ignoreUnits))
                {
                    (int, int) newPos = movementSystem.CalculateNextPosition((currentBoxGrid.row, currentBoxGrid.col), dir);
                    neighbourList.Add(grid2DList[newPos.Item1, newPos.Item2]);
                }
                
                // 上面CheckIfMovable会把有棋子的友方格子给否认掉，但格子上的友方棋子可能是调查员（怪物移动的目标），所以我们不想被否掉这个选项
                // TODO：按照现在的规则应该有这段吗？
                else if (movementSystem.CalculateNextPosition((currentBoxGrid.row, currentBoxGrid.col), dir).Item1 == targetPos.Item1 &&
                    movementSystem.CalculateNextPosition((currentBoxGrid.row, currentBoxGrid.col), dir).Item2 == targetPos.Item2)
                {
                    neighbourList.Add(grid2DList[targetPos.Item1, targetPos.Item2]);
                }
            }
            return neighbourList;
        }

        // 计算返回的路径
        private static List<BoxGrid> CalculatePath(BoxGrid target)
        {
            List<BoxGrid> path = new List<BoxGrid>();
            path.Add(target);
            BoxGrid currentBoxGrid = target;
            while (currentBoxGrid.cameFrom != null)
            {
                path.Add(currentBoxGrid.cameFrom);
                currentBoxGrid = currentBoxGrid.cameFrom;
            }
            path.Reverse();
            return path;
        }

        // 无视时间流逝倍率和障碍物，计算从起点到终点所需要的步数
        private static int EstimateDistCost(BoxGrid start, BoxGrid target, Monster monster)
        {
            int xDist = Mathf.Abs(start.row - target.row);
            int yDist = Mathf.Abs(start.col - target.col);
            int remaining = Mathf.Abs(xDist - yDist);

            // 怪物能斜着移动
            if (monster.dirs.Value.Contains(DirEnum.Topleft) ||
                monster.dirs.Value.Contains(DirEnum.Topright) ||
                monster.dirs.Value.Contains(DirEnum.Downleft) ||
                monster.dirs.Value.Contains(DirEnum.Downright)) 
                return Mathf.Min(xDist, yDist) + remaining;

            return xDist + yDist;
        }

        // 在一个格子列表里寻找消耗最低的格子
        private static BoxGrid GetLowestFCostBox(List<BoxGrid> boxGridList)
        {
            BoxGrid lowestFCostBox = boxGridList[0];
            for (int i = 0; i < boxGridList.Count; i++)
            {
                if (boxGridList[i].fCost < lowestFCostBox.fCost)
                {
                    lowestFCostBox = boxGridList[i];
                }
            }
            return lowestFCostBox;
        }

        // 计算fCost
        public static void CalculateFCost(BoxGrid boxGrid)
        {
            boxGrid.fCost = boxGrid.gCost + boxGrid.hCost;
        }

    }
}
