using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using ShootingEditor2D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Card: MonoBehaviour, IController
    {
        public SOCharacterInfo characterInfo;
        
        public Transform nodeFeature;   // feature根结点
        public List<GameObject> featureTouchArea = new List<GameObject>();   // feature UI响应区域
        public GameObject touchArea;    // UI响应区域
        public Canvas canvas;   // 用于调整层级
        public CanvasGroup canvasGroup; // 用于调整透明度
        public Transform tooltipTrans;
        
        public int handCardIndexTest;   // 在手牌中的顺序

        private Action<int> OnShowTooltip;

        private void Start()
        {
            touchArea = transform.Find("Root/UIEventArea").gameObject;
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            gameObject.AddComponent<GraphicRaycaster>();
            canvas = gameObject.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
            canvasGroup = gameObject.GetComponent<CanvasGroup>();

            var uiHandCard = UIKit.GetPanel<UIHandCard>();

            // todo 二级浮窗好像并不好写，暂时搁置
            #region feature二级浮窗

            nodeFeature = transform.Find("Root/NodeFeature");
            for (int i = 0; i < 3; i++)
            {
                var tmpTrans = nodeFeature.GetChild(i).gameObject;
                featureTouchArea.Add(tmpTrans);
            }

            tooltipTrans = transform.Find("Root/TooltipPos");
            OnShowTooltip = uiHandCard.UpdateTooltip;

            #endregion
            
            uiHelper.OnUIPointEnter += OnFocus;
            uiHelper.OnUIPointEnter += () => uiHandCard.OnFocusCard(this);
            uiHelper.OnUIPointExit += OnUnfocus;
            uiHelper.OnUIPointExit += uiHandCard.OnUnfocusCard;
            uiHelper.OnUIBeginDrag += OnDragStart;
            uiHelper.OnUIBeginDrag += uiHandCard.OnDragCardStart;
            uiHelper.OnUIDrag = OnDrag;
            uiHelper.OnUIEndDrag += uiHandCard.OnDragCardEnd;
            uiHelper.OnUIEndDrag += OnDragEnd;
            // uiHelper.OnUIDrag += uiHandCard.OnDragCard;  // todo 看拖拽手牌时手牌ui是否需要响应
            transform.localScale = new Vector3(UIHandCard.normalScale, UIHandCard.normalScale, 1f);
            
            // todo test
            characterInfo = Resources.Load<SOCharacterInfo>("ScriptableObjects/Characters/SOCharacterInfo_1");
        }

        private List<Action> OnUpdate = new List<Action>();
        private void Update()
        {
            foreach (var act in OnUpdate)
            {
                act.Invoke();
            }
        }

        private bool isFocused;
        void OnFocus()
        {
            OnUpdate.Add(ShowTooltip);
        }

        void OnUnfocus()
        {
            OnUpdate.Remove(ShowTooltip);
        }

        void OnDragStart()
        {
            Debug.Log("ViewCard: OnDragStart");
            canvasGroup.alpha = 0.5f;

            SelectMapStartCommand comm = new SelectMapStartCommand();
            comm.area = new SelectArea() {width = 1, height = 1};
            this.SendCommand<SelectMapStartCommand>(comm);
        }

        void OnDrag()
        {
            // Debug.Log("is dragging");
        }

        void OnDragEnd()
        {
            Debug.Log("ViewCard: OnDragEnd");
            canvasGroup.alpha = 1f;
            this.SendCommand<SelectMapEndCommand>();
        }

        void ShowTooltip()
        {
            // 这样是能检测到image的，但如果后面还有其他东西，估计也会检测到
            var e = new PointerEventData(EventSystem.current);
            e.position = Input.mousePosition;
            List<RaycastResult> rayRet = new List<RaycastResult>();
            EventSystem.current.RaycastAll(e, rayRet);
            
            foreach (var ret in rayRet)
            {
                // Debug.Log(ret.ToString());
                if (featureTouchArea.Contains(ret.gameObject))
                {
                    // Debug.Log($"mouse in feature with tooltipTrans active: {tooltipTrans.gameObject.activeSelf}");
                    if (!tooltipTrans.gameObject.activeSelf)
                    {
                        OnShowTooltip.Invoke(featureTouchArea.IndexOf(ret.gameObject));
                        tooltipTrans.gameObject.SetActive(true);
                    }
                    return;
                }
            }
            if(tooltipTrans.gameObject.activeSelf)
                tooltipTrans.gameObject.SetActive(false);
                
            // 这个啥也检测不到，我猜是另外使用了ui摄像机的问题
            // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit hit;
            // if (Physics.Raycast(ray, out hit, 500))
            // {
            //     //打印碰撞信息 
            //     Debug.Log(hit.collider.name);
            // }
            // else
            // {
            //     Debug.Log("ray hit nothing");
            // }
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}