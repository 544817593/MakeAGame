using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 定时执行某个方法时，需要提供的信息
    /// </summary>
    public class ScheduleData
    {
        public bool isFrameDelay = false;   // 以帧还是秒计算间隔
        public float second;  // 总时间间隔
        public float leftSecond;
        public int frame; // 总帧数间隔
        public int leftFrame;
        public int leftTriggerTime; // 剩余执行次数，-1为不限次数
    }
    
    /// <summary>
    /// 统一的update管理，可以在这里定时执行某个方法
    /// 目前一个方法只接受一个定时计划
    /// </summary>
    public class UpdateManager: MonoBehaviour, IController
    {
        private void Start()
        {
            Debug.Log("UpdateManager Start");
        }

        private Dictionary<Action, ScheduleData> dictSchedules = new Dictionary<Action, ScheduleData>();
        private Dictionary<Action, ScheduleData> dictToAddSchedules = new Dictionary<Action, ScheduleData>();
        private List<Action> toRemoveActions = new List<Action>();
        private List<Action> toExecuteAcions = new List<Action>();
        
        private void Update()
        {
            // 依次计时，记录即将结束的计划
            foreach (var kvp in dictSchedules)
            {
                var sch = kvp.Value;
                if (sch.isFrameDelay)
                {
                    sch.leftFrame--;
                    if (sch.leftFrame <= 0)
                    {
                        toExecuteAcions.Add(kvp.Key);
                        
                        sch.leftFrame = sch.frame;

                        // 限定次数的计划
                        if (sch.leftTriggerTime > 0)
                        {
                            sch.leftTriggerTime--;
                            if (sch.leftTriggerTime <= 0)
                            {
                                toRemoveActions.Add(kvp.Key);
                            }
                        }
                    }
                }
                else
                {
                    sch.leftSecond -= Time.deltaTime;
                    if (sch.leftSecond <= 0)
                    {
                        toExecuteAcions.Add(kvp.Key);

                        sch.leftSecond = sch.second;
                        
                        // 限定次数的计划
                        if (sch.leftTriggerTime > 0)
                        {
                            sch.leftTriggerTime--;
                            if (sch.leftTriggerTime <= 0)
                            {
                                toRemoveActions.Add(kvp.Key);
                            }
                        }
                    }
                }
            }
            
            // 执行计划
            foreach (var act in toExecuteAcions)
            {
                try
                {
                    act?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message + e.StackTrace);
                }
            }
            toExecuteAcions.Clear();

            // 删去结束的计划
            foreach (var act in toRemoveActions)
            {
                dictSchedules.Remove(act);
            }
            toRemoveActions.Clear();

            // 加入新计划
            foreach (var kvp in dictToAddSchedules)
            {
                dictSchedules.TryGetValue(kvp.Key, out var crtSch);
                if (crtSch != null)
                {
                    Debug.LogError($"UpdateManager: try add schedule action already exist, name: {kvp.Key.Method.Name}");
                }
                else
                {
                    dictSchedules.Add(kvp.Key, kvp.Value);
                }
            }
            dictToAddSchedules.Clear();
        }

        /// <summary>
        /// 开启一个定时执行计划
        /// </summary>
        /// <param name="act">要执行的方法，需要无参</param>
        /// <param name="isFrame">是否为帧计时，否则为秒计时</param>
        /// <param name="count">间隔帧数/秒数</param>
        /// <param name="triggerTime">执行次数，默认为-1（无限次）</param>
        public void ScheduleExecute(Action act, bool isFrame, float count, int triggerTime = -1)
        {
            if(isFrame)
                ScheduleFrameExecute(act, (int)count, triggerTime);
            else
                ScheduleSecondExecute(act, count, triggerTime);
        }

        void ScheduleFrameExecute(Action act, int frameCount, int triggerTime)
        {
            ScheduleData sch = new ScheduleData()
                {isFrameDelay = true, frame = frameCount, leftFrame = frameCount, leftTriggerTime = triggerTime};
            dictToAddSchedules.Add(act, sch);
        }

        void ScheduleSecondExecute(Action act, float secondCount, int triggerTime)
        {
            ScheduleData sch = new ScheduleData()
                {isFrameDelay = false, second = secondCount, leftSecond = secondCount, leftTriggerTime = triggerTime};
            dictToAddSchedules.Add(act, sch);
        }

        public void Clear()
        {
            Debug.Log($"UpdateManager: Clear {dictSchedules.Count} schedules");
            dictSchedules.Clear();
        }
        


        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}