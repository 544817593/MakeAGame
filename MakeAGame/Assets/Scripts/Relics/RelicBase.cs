using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{

    /// <summary>
    /// 遗物基类
    /// </summary>
    public abstract class RelicBase
    {
        public SORelic so { get; private set; }

        protected RelicBase(SORelic so)
        {
            this.so = so;
        }

        public abstract void Activate(IRelicSystem sys);    // 注册监听事件
        public abstract void InstantEffect(object obj);

    }
}