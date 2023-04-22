using System;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewCard: MonoBehaviour
    {
        public GameObject touchArea;    // UI响应区域
        public Canvas canvas;
        public float normalScale = 0.15f;   // 普通缩放系数
        public float zoomScale = 0.4f;      // 放大系数
        public int handCardIndexTest;   // 在手牌中的顺序

        private void Start()
        {
            transform.localScale = new Vector3(normalScale, normalScale, 1f);
            
            touchArea = transform.Find("Root/UIEventArea").gameObject;
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            gameObject.AddComponent<GraphicRaycaster>();
            canvas = gameObject.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
            // uiHelper.OnUIPointEnter = OnMouseEnter;
            // uiHelper.OnUIPointExit = OnMouseExit;
            
            var uiHandCard = UIKit.GetPanel<UIHandCard>();
            uiHelper.OnUIPointEnter = () => uiHandCard.OnFocusCard(this);
            uiHelper.OnUIPointExit = uiHandCard.OnUnfocusCard;
        }

        /// <summary>
        /// 鼠标悬浮
        /// </summary>
        private void OnMouseEnter()
        {
            Debug.Log("enter");
            // transform.localScale = Vector3.one * zoomScale;

            // transform.SetAsLastSibling();
        }

        /// <summary>
        /// 鼠标移出
        /// </summary>
        private void OnMouseExit()
        {
            Debug.Log("exit");
            // transform.localScale = Vector3.one * normalScale;
            
        }
    }
}