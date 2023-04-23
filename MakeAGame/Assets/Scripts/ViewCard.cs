using System;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewCard: MonoBehaviour
    {
        public Transform nodeFeature;   // feature根结点
        public GameObject[] featureTouchArea = new GameObject[3];   // feature UI响应区域

        public GameObject touchArea;    // UI响应区域
        public Canvas canvas;   // 用于调整层级
        
        public int handCardIndexTest;   // 在手牌中的顺序

        private void Start()
        {
            touchArea = transform.Find("Root/UIEventArea").gameObject;
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            gameObject.AddComponent<GraphicRaycaster>();
            canvas = gameObject.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;

            // todo 二级浮窗好像并不好写，暂时搁置
            #region feature二级浮窗

            nodeFeature = transform.Find("Root/NodeFeature");
            for (int i = 0; i < 3; i++)
            {
                var tmpTrans = nodeFeature.GetChild(i).gameObject;
                var uiFeatureHelper = tmpTrans.AddComponent<UIEventHelper>();
                // tmpTrans.AddComponent<GraphicRaycaster>();
                // var tmpCanvas = tmpTrans.GetComponent<Canvas>();
                uiFeatureHelper.OnUIPointEnter = () =>
                {
                    Debug.Log("ui feature enter");
                };

                featureTouchArea[i] = tmpTrans;
            }

            #endregion

            var uiHandCard = UIKit.GetPanel<UIHandCard>();
            uiHelper.OnUIPointEnter = () => uiHandCard.OnFocusCard(this);
            uiHelper.OnUIPointExit = uiHandCard.OnUnfocusCard;
            uiHelper.OnUIBeginDrag = uiHandCard.OnDragCardStart;
            uiHelper.OnUIDrag = OnDrag;
            uiHelper.OnUIEndDrag = uiHandCard.OnDragCardEnd;
            // uiHelper.OnUIDrag += uiHandCard.OnDragCard;  // todo 看拖拽手牌时手牌ui是否需要响应
            transform.localScale = new Vector3(UIHandCard.normalScale, UIHandCard.normalScale, 1f);
        }

        void OnDrag()
        {
            Debug.Log("is dragging");
        }
    }
}