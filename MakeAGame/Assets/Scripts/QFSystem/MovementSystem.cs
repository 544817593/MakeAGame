using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
 
namespace Game
{
    /// <summary>
    /// 目前只有敌人在用
    /// </summary>
    public interface IMovementSystem : ISystem
    {
        /// <summary>
        /// 提供当前位置和当前移动方向，返回下一个格子的坐标
        /// </summary>
        /// <param name="currPos">当前位置</param>
        /// <param name="dir">移动方向</param>
        /// <returns></returns>
        (int, int) CalculateNextPosition((int, int) currPos, DirEnum dir);

        /// <summary>
        /// 任何单位尝试移动前进行的最基础的检查
        /// </summary>
        /// <param name="intendPos">想移动到的地方</param>
        /// <returns></returns>
        bool MovementBaseCheck((int, int) intendPos);

        /// <summary>
        /// 输入两个相邻或斜角的格子A和格子B，返回A到B所需要的方向
        /// </summary>
        /// <param name="start">格子A</param>
        /// <param name="target">格子B</param>
        /// <returns></returns>
        DirEnum NeighbourBoxGridsToDir(BoxGrid start, BoxGrid target);
    }

    public class MovementSystem : AbstractSystem, IMovementSystem
    {
        protected override void OnInit()
        {

        }

        public (int, int) CalculateNextPosition((int, int) currPos, DirEnum dir)
        {
            (int, int) nextPos = currPos;
            switch (dir)
            {
                case DirEnum.Top:
                    nextPos.Item1 -= 1;
                    break;
                case DirEnum.Right:
                    nextPos.Item2 += 1;
                    break;
                case DirEnum.Down:
                    nextPos.Item1 += 1;
                    break;
                case DirEnum.Left:
                    nextPos.Item2 -= 1;
                    break;
                case DirEnum.Topleft:
                    nextPos.Item1 -= 1; nextPos.Item2 -= 1;
                    break;
                case DirEnum.Topright:
                    nextPos.Item1 -= 1; nextPos.Item2 += 1;
                    break;
                case DirEnum.Downright:
                    nextPos.Item1 += 1; nextPos.Item2 += 1;
                    break;
                case DirEnum.Downleft:
                    nextPos.Item1 += 1; nextPos.Item2 -= 1;
                    break;
            }
            return nextPos;
        }

        public bool MovementBaseCheck((int, int) intendPos)
        {
            // 地图越界
            var grid2DList = this.GetSystem<IMapSystem>().Grids();
            if (intendPos.Item1 < 0 || intendPos.Item1 >= grid2DList.GetLength(0) ||
                intendPos.Item2 < 0 || intendPos.Item2 >= grid2DList.GetLength(1))
            {
                return false;
            }

            // 已有棋子或者地形不可通过
            if (!this.GetSystem<IMapSystem>().GridCanMoveTo(grid2DList[intendPos.Item1, intendPos.Item2]))
            {
                return false;
            }

            return true;
        }


        public DirEnum NeighbourBoxGridsToDir(BoxGrid start, BoxGrid target)
        {
            // 目标在右边
            if (target.col > start.col)
            {
                // 右下
                if (target.row > start.row)
                {
                    return DirEnum.Downright;               
                }
                // 右上
                else if (target.row < start.row)
                {
                    return DirEnum.Topright;
                }
            }
            // 目标在左边
            else if (target.col < start.col)
            {
                // 左下
                if (target.row > start.row)
                {
                    return DirEnum.Downleft;
                // 左上
                }
                else if (target.row < start.row)
                {
                    return DirEnum.Topleft;
                }
            }
            // 目标在正下面
            if ((target.col == start.col) && (target.row > start.row))
            {
                return DirEnum.Down;
            }
            // 目标在正上面
            if ((target.col == start.col) && (target.row < start.row))
            {
                return DirEnum.Top;
            }
            // 目标在正左面
            if ((target.row == start.row) && (target.col < start.col))
            {
                return DirEnum.Left;
            }
            // 目标在正右面
            if ((target.row == start.row) && (target.col > start.col))
            {
                return DirEnum.Right;
            }
            Debug.LogError("错误：目标无法直达，返回默认值Top");
            return DirEnum.Top;

        }


    }

}

