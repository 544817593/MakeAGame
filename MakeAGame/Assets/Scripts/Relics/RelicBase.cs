using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{

    /// <summary>
    /// 遗物基类，收到事件立刻生效
    /// </summary>
    public abstract class RelicBase
    {
        public SORelic so { get; private set; }
        public bool isActive = false;   // 当前是否激活（是否在监听）
        public bool isRunOut = false;   // 是否已经用完了，永久不再生效

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
    public abstract class RelicRecycle: RelicBase
    {
        protected RelicRecycle(SORelic so): base(so)
        {
        }
        protected abstract void WithdrawEffect(object obj);  // 撤回效果
    }

    /// <summary>
    /// 增加流程型遗物：会直接插入一个可能本来没有的流程，如织网（每场战斗开始前，可以获得一次重抽手牌的机会）
    /// </summary>
    public abstract class RelicAddProcess : RelicBase
    {
        protected RelicAddProcess(SORelic so): base(so)
        {
        }
        
        // todo 增加流程很复杂 还没想好咋写
        
    }

    /// <summary>
    /// 充能型遗物：达成x次条件后生效，如废品回收机（每使用3张死面牌，随机获得一张普通品质的学者牌）
    /// </summary>
    public abstract class RelicCharge : RelicBase
    {
        protected RelicCharge(SORelic so): base(so)
        {
        }

        protected int totalChargeTime;    // 总共需要充能次数
        protected int leftChargeTime; // 当前需要充能次数
        protected abstract void Charge(object obj); // 充能
    }
    
    /// <summary>
    /// 耗能性遗物：生效x次后失效，如符纸（抵消下5次生面牌受到的超过10点伤害的攻击）
    /// </summary>
    public abstract class RelicConsumable : RelicBase
    {
        protected RelicConsumable(SORelic so): base(so)
        {
        }

        protected int leftUseTime;  // 剩余可生效次数
        protected override void TakeEffect(object obj)
        {
            OnTakeEffect(obj);

            leftUseTime--;
            if (leftUseTime <= 0)
            {
                isActive = false;
                var sys = GameEntry.Interface.GetSystem<IRelicSystem>();
                // todo 置为失效状态
                
                
            }
        }

        protected abstract void OnTakeEffect(object obj);
    }
    
    
    
}