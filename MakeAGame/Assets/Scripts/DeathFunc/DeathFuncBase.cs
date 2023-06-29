using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 死面功能基类
    /// </summary>
    public class DeathFuncBase: ICanGetSystem
    {
        public SelectArea area = new SelectArea();
        public ViewCard viewCard;

        public DeathFuncBase()
        {
            area.selectStage = MapSelectStage.IsPutDeathFunc;
        }

        public virtual void OnExecute(List<BoxGrid> grids)
        {
            Debug.Log($"DeathFuncBase execute grids count: {grids.Count}");
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}