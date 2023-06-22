using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IRelicSystem : ISystem
    {
        UIRelic ui { set; }
        
        void ActivateAllRelics();

        void AddRelic(SORelic so);

        void RefreshRelics();

        void StartCountTime();

        void EndCountTime();

        void RegisterRelicEvent<T>(RelicBase relic, Action<object> act);

        void UnregisterOneRelic(RelicBase relic);

        List<RelicBase> GetRelics();
    }

    public class RelicEventData
    {
        public int priority;
        public RelicBase relic;
        public Action<object> act;
    }

    public class RelicSystem: AbstractSystem, IRelicSystem  // todo 一些结束时统一inactivate的处理
    {
        public UIRelic ui { get; set; }

        protected override void OnInit()
        {
            Debug.Log("RelicSystem: OnInit");
            
            Debug.Log("RelicSystem: add test relic");
            // var so = IdToSO.FindRelicSOByID(1);
            // this.GetSystem<IRelicSystem>().AddRelic(so);

            // 遗物测试
            var so = IdToSO.FindRelicSOByID(1);
            this.GetSystem<IRelicSystem>().AddRelic(so);
            so = IdToSO.FindRelicSOByID(3);
            this.GetSystem<IRelicSystem>().AddRelic(so);
            // so = IdToSO.FindRelicSOByID(5);
            // this.GetSystem<IRelicSystem>().AddRelic(so);
            
            // ActivateAllRelics();
        }

        private List<RelicBase> relics = new List<RelicBase>(); // 遗物列表
        private int totalSecs;  // 已经计过的时间

        private Dictionary<Type, List<RelicEventData>> dictRelicEvents = new Dictionary<Type, List<RelicEventData>>();
        private List<IUnRegister> unregisters = new List<IUnRegister>();

        public List<RelicBase> GetRelics()
        {
            return relics;
        }

        public void ActivateAllRelics()
        {
            DeactivateAllRelic();
            
            Debug.Log($"RelicSystem: ActivateRelics count {relics.Count}");
            foreach (var relic in relics)
            {
                // 玩家当前的所有遗物中，没有遗物会取消其效果
                var conflictRelics = IsCanceledByPlayerRelics(relic);
                if (conflictRelics.Count == 0)
                {
                    if (!relic.IsRunOut)
                    {
                        relic.Activate(this);
                        relic.isActive = true;   
                    }
                }
                else
                {
                    string s = String.Empty;
                    foreach (var conflict in conflictRelics)
                    {
                        s += conflict.so.name + " ";
                    }
                    Debug.Log($"relic actvate failed, {relic.so.name} is canceled by {s}");
                }
            }
        }

        public void UnregisterOneRelic(RelicBase relic)
        {
            foreach (var kvp in dictRelicEvents)
            {
                var datas = kvp.Value.FindAll(data => data.relic == relic);
                foreach (var d in datas)
                {
                    kvp.Value.Remove(d);
                    Debug.Log($"relic {relic.so.relicName} run out, unregister {d.act.Method.Name}");
                }
            }
        }

        // 遗物是否被已经拥有的遗物取消效果
        List<RelicBase> IsCanceledByPlayerRelics(RelicBase relic)
        {
            List<RelicBase> ret = new List<RelicBase>();
            foreach (var playerRelic in relics)
            {
                if (playerRelic.so.toCancelRelics.Contains(relic.so.relicID))
                    ret.Add(playerRelic);
            }

            return ret;
        }
        
        List<RelicBase> WillCancelPlayerRelics(RelicBase relic)
        {
            List<RelicBase> ret = new List<RelicBase>();
            foreach (var playerRelic in relics)
            {
                if (playerRelic.so.beCanceledRelics.Contains(relic.so.relicID))
                    ret.Add(playerRelic);
            }

            return ret;
        }
        
        public void AddRelic(SORelic so)
        {
            var item = relics.Find(item => item.so.relicID == so.relicID);
            if (item != null) 
            {
                Debug.LogError($"repeat relic {so.relicName}");
                return;   // 不能重复添加
            }

            var relic = Extensions.GetRelicBySO(so);

            relics.Add(relic);
            // ActivateOneRelic(relic);
            ActivateAllRelics();
            ui?.AddRelic(relic);
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

        // 只从遗物内部调用
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
            else // 该事件已经有其他遗物在监听，做判断   // 移到上层处理了
            {
                // // 遗物是否会取消现有遗物效果
                // List<RelicEventData> toRemoveDatas = new List<RelicEventData>();
                // foreach (var data in datas)
                // {
                //     if (data.relic.so.beCanceledRelics.Contains(relic.so.relicID))
                //     {
                //         toRemoveDatas.Add(data);
                //     }
                // }
                // foreach (var data in toRemoveDatas)
                // {
                //     datas.Remove(data);
                // }
                // toRemoveDatas.Clear();
                //
                // // 现有遗物是否会取消该遗物效果
                // bool isCanceled = false;
                // foreach (var data in datas)
                // {
                //     if (data.relic.so.toCancelRelics.Contains(relic.so.relicID))
                //     {
                //         isCanceled = true;
                //         break;
                //     }
                // }
                // if (isCanceled) return;

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

            string s = $"relic {relic.so.name} register event {type.Name}, act {act.Method.Name}\n";
            var listData = dictRelicEvents[type];
            for (int i = 0; i < listData.Count; i++)
            {
                var data = listData[i];
                s += $"{i}-{data.priority}-{data.relic.so.name}-{data.act.Method.Name}\n";
            }
            Debug.Log(s);
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

        void DeactivateAllRelic()
        {
            ClearAllRegister();
            foreach (var relic in relics)
            {
                relic.isActive = false;
            }
        }

        void ClearAllRegister()
        {
            Debug.Log("relic system ClearAllRegister");
            foreach (var unregister in unregisters)
            {
                unregister.UnRegister();
            }
            unregisters.Clear();
        }

        void CountTime(CountTimeEvent e)
        {
            totalSecs++;
            Debug.Log($"RelicSystem: count time {totalSecs}");
        }
    }
}