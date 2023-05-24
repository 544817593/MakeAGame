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
    public partial class ViewCard: MonoBehaviour, IController
    {
        // 卡牌数据
        public Card card;

        private Action<SOFeature> OnShowTooltip;

        private void Start()
        {
            gameObject.AddComponent<GraphicRaycaster>();    // 这个必须放在canvas绑定前面，因为它会连带创建canvas
            
            InitBind(); // 绑定各组件
            
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
            transform.localScale = new Vector3(UIHandCard.normalScale, UIHandCard.normalScale, 1f);

            var uiHandCard = UIKit.GetPanel<UIHandCard>();
            OnShowTooltip = uiHandCard.UpdateTooltip;

            uiHelper.OnUIPointEnter += OnFocus;
            uiHelper.OnUIPointEnter += () => uiHandCard.OnFocusCard(this);
            uiHelper.OnUIPointExit += OnUnfocus;
            uiHelper.OnUIPointExit += uiHandCard.OnUnfocusCard;
            uiHelper.OnUIBeginDrag += OnDragStart;
            uiHelper.OnUIBeginDrag += () => uiHandCard.OnDragCardStart(this);
            uiHelper.OnUIDrag = OnDrag;
            uiHelper.OnUIEndDrag += uiHandCard.OnDragCardEnd;
            uiHelper.OnUIEndDrag += OnDragEnd;
            // uiHelper.OnUIDrag += uiHandCard.OnDragCard;  // todo 看拖拽手牌时手牌ui是否需要响应

            this.RegisterEvent<PutPieceByHandCardEvent>(OnUseAsLifeCard).UnRegisterWhenGameObjectDestroyed(this);

            InitView();
        }

        public void InitView()
        {
            imgRarity.sprite = Extensions.GetRaritySprite(card.rarity);
            imgCharacter.sprite = card.cardSprite;
            tmpSanCost.text = card.sanCost.ToString();
            tmpHP.text = card.hp.ToString();
            tmpMoveSpd.text = card.moveSpd.ToString();
            tmpDamage.text = card.damage.ToString();
            tmpDefend.text = card.defend.ToString();
            tmpName.text = card.charaName;
            tmpDesc.text = card.deathFuncDesc;
            for(int i = 0; i < card.features.Count; i++)
            {
                featureTouchArea[i].GetComponent<Image>().sprite = card.features[i].icon;
            }
            for (int i = card.features.Count; i < 3; i++)
            {
                featureTouchArea[i].gameObject.SetActive(false);
            }
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
            // Debug.Log("ViewCard: OnDragStart");
            canvasGroup.alpha = 0.5f;

            SelectMapStartCommand comm = new SelectMapStartCommand();
            comm.area = new SelectArea() {width = card.width, height = card.height};
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
            this.SendCommand<SelectMapEndCommand>(new SelectMapEndCommand(this));
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
                        int index = featureTouchArea.IndexOf(ret.gameObject);
                        OnShowTooltip.Invoke(card.features[index]);
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

        void OnUseAsLifeCard(PutPieceByHandCardEvent e)
        {
            // 检测通知的是不是自己
            if (e.viewCard != this) return;

            this.SendCommand(new PutPieceCommand(this, e.pieceGrids));
            
            // todo 手牌使用后的后续处理（此时已经移出手牌系统并隐藏），如返回背包、销毁...
            Debug.Log("[TODO] after card use as life card");
            
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}