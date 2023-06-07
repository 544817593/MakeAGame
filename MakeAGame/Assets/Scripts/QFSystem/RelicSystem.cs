using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRelicSystem : ISystem
    {
        void ActivateRelics();

        void AddRelic();

        void RefreshRelics();

        void StartCountTime();

        void EndCountTime();

        void RegisterRelicEvent<T>(RelicBase relic, Action<object> act);
    }

    public class RelicEventData
    {
        public int priority;
        public RelicBase relic;
        public Action<object> act;
    }

    public class RelicSystem: AbstractSystem, IRelicSystem
    {
        protected override void OnInit()
        {
            Debug.Log("RelicSystem: OnInit");
            
            
            
            Debug.Log("RelicSystem: add test relic");
            var so = Extensions.GetTestSORelic();
            relics.Add(new RelicInstantEffect(so));
            relics.Add(new RelicInstantEffect2(so));
            
            ActivateRelics();
        }

        private List<RelicBase> relics = new List<RelicBase>(); // 遗物列表
        private int totalSecs;  // 已经计过的时间

        private Dictionary<Type, List<RelicEventData>> dictRelicEvents = new Dictionary<Type, List<RelicEventData>>();
        private List<IUnRegister> unregisters = new List<IUnRegister>();

        public void ActivateRelics()
        {
            Debug.Log($"RelicSystem: ActivateRelics count {relics.Count}");
            foreach (var relic in relics)
            {
                relic.Activate(this);
            }
        }

        public void AddRelic()
        {
            throw new System.NotImplementedException();
        }

        public void RefreshRelics()
        {
            throw new System.NotImplementedException();
        }

        private IUnRegister countTimeUnregister;
        public void StartCountTime()
        {
            totalSecs = 0;
            countTimeUnregister = this.RegisterEvent<CountTimeEvent>(CountTime);
        }

        public void EndCountTime()
        {
            countTimeUnregister.UnRegister();
        }

        public void RegisterRelicEvent<T>(RelicBase relic, Action<object> act)
        {
            var type = typeof(T);
            if (!dictRelicEvents.TryGetValue(type, out var datas)) // 该事件尚未有遗物监听，系统创立监听列表
            {
                dictRelicEvents[type] = new List<RelicEventData>();
                unregisters.Add(this.RegisterEvent<T>(CheckListenerRelics));
                RelicEventData newData = new RelicEventData() {priority = relic.so.effectPriority, relic = relic, act = act};
                dictRelicEvents[type].Add(newData);
            }
            else // 该事件已经有其他遗物在监听，做判断
            {
                // 遗物是否会取消现有遗物效果
                List<RelicEventData> toRemoveDatas = new List<RelicEventData>();
                foreach (var data in datas)
                {
                    if (data.relic.so.beCanceledRelics.Contains(relic.so.relicID))
                    {
                        toRemoveDatas.Add(data);
                    }
                }
                foreach (var data in toRemoveDatas)
                {
                    datas.Remove(data);
                }
                toRemoveDatas.Clear();

                // 现有遗物是否会取消该遗物效果
                bool isCanceled = false;
                foreach (var data in datas)
                {
                    if (data.relic.so.toCancelRelics.Contains(relic.so.relicID))
                    {
                        isCanceled = true;
                        break;
                    }
                }
                if (isCanceled) return;

                // 按优先级插入
                RelicEventData newData = new RelicEventData() {priority = relic.so.effectPriority, relic = relic, act = act};
                // 找到第一个 priority大于新插入值的数据（priority越大，优先级越低）
                int index = datas.FindIndex(data => data.priority > relic.so.effectPriority);
                // 插入到这个数据之前，若为-1，说明没有更低优先级的，插入到最后
                if(index < 0)
                    datas.Add(newData);
                else
                    datas.Insert(index, newData);
            }
        }

        // 实际执行遗物方法
        void CheckListenerRelics<T>(T t)
        {
            var type = typeof(T);
            var datas = dictRelicEvents[type];
            foreach (var data in datas)
            {
                data.act.Invoke(t);
            }
        }

        void CountTime(CountTimeEvent e)
        {
            totalSecs++;
            Debug.Log(totalSecs);
        }
    }
}