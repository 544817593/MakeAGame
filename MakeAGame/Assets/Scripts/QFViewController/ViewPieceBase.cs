using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 棋子基类，理想情况下，敌人和友方都将继承此类
    /// </summary>
    public partial class ViewPieceBase: MonoBehaviour, IController
    {
        protected SOCharacterInfo so;

        public List<BoxGrid> pieceGrids { get; private set; } = new List<BoxGrid>();

        public virtual void SetGrids(List<BoxGrid> grids)
        {
            foreach (var grid in grids)
            {
                pieceGrids.Add(grid);
            }
            transform.position = GetGridsCenterPos();
        }

        protected Vector3 GetGridsCenterPos()
        {
            if (pieceGrids.Count == 0) return transform.position;
            Vector3 centerPos = Vector3.zero;
            foreach (var grid in pieceGrids)
            {
                centerPos += grid.transform.position;
            }
            centerPos /= pieceGrids.Count;
            return centerPos;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}