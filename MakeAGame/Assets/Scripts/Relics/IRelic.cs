using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 遗物接口
    /// </summary>
    public interface IRelic
    {
        void Activate();    // 激活，监听事件
        void Inactivate();  // 失活/失效，自动取消监听
        void RegisterRelicEvent<T>(Action<T> onEvent); // 遗物注册事件，使用该方法注册监听以自动取消
    }
    
    /// <summary>
    /// 遗物基类
    /// </summary>
    public abstract class RelicBase : IRelic
    {
        private readonly HashSet<IUnRegister> unRegisters = new HashSet<IUnRegister>();
        
        public abstract void Activate();

        public void Inactivate()
        {
            foreach (var unRegister in unRegisters)
            {
                unRegister.UnRegister();
            }
        }

        public void RegisterRelicEvent<T>(Action<T> onEvent)
        {
            var unregister = GameEntry.Interface.RegisterEvent<T>(onEvent);
            unRegisters.Add(unregister);
        }
    }

    public class RelicInstantEffectExample : RelicBase
    {
        public override void Activate()
        {
            RegisterRelicEvent<PieceMoveFinishEvent>(InstantEffect);
        }

        void InstantEffect(PieceMoveFinishEvent e)
        {
            Debug.Log("relic: take instant effect");
        }
    }
    
    
    
    
}