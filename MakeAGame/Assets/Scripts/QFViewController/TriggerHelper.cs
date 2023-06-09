using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// 挂载有作为trigger的collier组件的，用此脚本管理鼠标等事件
    /// </summary>
    public class TriggerHelper : MonoBehaviour
    {
        public Action OnMouseEnterEvent;
        public Action OnMouseExitEvent;
        public Action OnMouseDownEvent;
        public Action OnMouseUpEvent;
        
        private void OnMouseEnter()
        {
            OnMouseEnterEvent?.Invoke();
        }

        private void OnMouseExit()
        {
            OnMouseExitEvent?.Invoke();
        }

        private void OnMouseDown()
        {
            OnMouseDownEvent?.Invoke();
        }

        private void OnMouseUp()
        {
            OnMouseUpEvent?.Invoke();
        }
    }
}