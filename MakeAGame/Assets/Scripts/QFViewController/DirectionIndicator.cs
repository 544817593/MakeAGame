using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 挂在方向按钮上，指示这个按钮代表哪个方向
    /// </summary>
    public class DirectionIndicator: MonoBehaviour
    {
        public DirEnum direction;
        public TriggerHelper mouseHelper;

        public void InitUIHelper(Action act)
        {
            var uiHelper = this.AddComponent<UIEventHelper>();
            uiHelper.OnUIPointEnter = act;
        }
    }
}