using System;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IUpdateSystem: ISystem
    {
        void Reset();
        // 开启一个定时计划 详见UpdateManager内
        void ScheduleExecute(Action act, bool isFrame, float count, int triggerTime = -1);
        // 延时执行一次
        void DelayExecute(Action act, bool isFrame, float count);
    }

    public class UpdateSystem : AbstractSystem, IUpdateSystem
    {
        private UpdateManager updateMan;

        protected override void OnInit()
        {
        }

        public void Reset()
        {
            if (updateMan != null)
            {
                updateMan.Clear();
            }
            else
            {
                GameObject updateGO = GameObject.Find("UpdateManager");
                updateMan = updateGO.GetComponent<UpdateManager>();
            }
        }

        public void ScheduleExecute(Action act, bool isFrame, float count, int triggerTime = -1)
        {
            updateMan.ScheduleExecute(act, isFrame, count, triggerTime);
        }

        public void DelayExecute(Action act, bool isFrame, float count)
        {
            updateMan.DelayExecute(act, isFrame, count);
        }
    }
}