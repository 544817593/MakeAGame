using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// 需要对点击、拖拽等操作作出响应的UI挂载此组件
    /// </summary>
    public class UIEventHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public Action OnUIPointEnter;   // 鼠标进入回调
        public Action OnUIPointExit;    // 鼠标离开回调
        public Action OnUIPointClick;   // 鼠标点击回调
        public Action OnUIPointDown;    // 鼠标按下回调
        public Action OnUIPpintUp;      // 鼠标抬起回调
        public Action OnUIDrag;         // 鼠标拖拽回调

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            OnUIPointEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            OnUIPointExit.Invoke();
        }
        
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            OnUIPointClick?.Invoke();
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            OnUIPointDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData pointerEventData)
        {
            OnUIPpintUp?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnUIDrag?.Invoke();
        }        
    }
}