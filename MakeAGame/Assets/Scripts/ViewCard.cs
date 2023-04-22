using System;
using UnityEngine;

namespace Game
{
    public class ViewCard: MonoBehaviour
    {
        public GameObject touchArea;    // UI响应区域
        public float normalScale = 0.15f;   // 普通缩放系数
        public float zoomScale = 0.4f;      // 放大系数

        private void Start()
        {
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            uiHelper.OnUIPointEnter = OnMouseEnter;
            uiHelper.OnUIPointExit = OnMouseExit;
        }

        /// <summary>
        /// 鼠标悬浮
        /// </summary>
        private void OnMouseEnter()
        {
            Debug.Log("enter");
            transform.localScale = Vector3.one * zoomScale;

            transform.SetAsLastSibling();
        }

        /// <summary>
        /// 鼠标移出
        /// </summary>
        private void OnMouseExit()
        {
            Debug.Log("exit");
            transform.localScale = Vector3.one * normalScale;
        }
    }
}