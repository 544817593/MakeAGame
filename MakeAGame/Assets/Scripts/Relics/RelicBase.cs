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
        protected abstract void TakeEffect(object obj);    // 生效
    }

    /// <summary>
    /// 临时生效型遗物：会撤回效果，如亡灵的帽子（每场战斗开始时，亡灵随机获得一点属性，战斗结束后消失）
    /// </summary>
    public interface IRelicRecycle
    {
        protected void WithdrawEffect(object obj);  // 撤回效果
    }
    
    
}