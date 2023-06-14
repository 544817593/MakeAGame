using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using ShootingEditor2D;
using SnakeGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public partial class ViewCard: MonoBehaviour, IController
    {
        public Card card;   // 卡牌数据

        private Action<SOFeature> OnShowTooltip;

        private void Start()
        {
            gameObject.AddComponent<GraphicRaycaster>();    // 这个必须放在canvas绑定前面，因为它会连带创建canvas
            
            InitBind(); // 绑定各组件
            
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            
            canvas.overrideSorting = true;
            canvas.sortingOrder = UIHandCard.normalSortingLayer;
            transform.localScale = new Vector3(UIHandCard.normalScale, UIHandCard.normalScale, 1f);

            var uiHandCard = UIKit.GetPanel<UIHandCard>();
            OnShowTooltip = uiHandCard.UpdateTooltip;

            uiHelper.OnUIPointEnter += OnFocus;
            uiHelper.OnUIPointEnter += () => uiHandCard.OnFocusCard(this);
            uiHelper.OnUIPointExit += OnUnfocus;
            uiHelper.OnUIPointExit += uiHandCard.OnUnfocusCard;
            uiHelper.OnUIBeginDrag += OnDragStart;
            uiHelper.OnUIBeginDrag += () => uiHandCard.OnDragCardStart(this);
            uiHelper.OnUIEndDrag += uiHandCard.OnDragCardEnd;
            uiHelper.OnUIEndDrag += OnDragEnd;
            uiHelper.OnUIPointClick += OnRightClick;

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
                featureTouchArea[i].GetComponent<Image>().sprite = IdToSO.FindFeatureSOByEnum(card.features[i]).icon;
            }
            for (int i = card.features.Count; i < 3; i++)
            {
                featureTouchArea[i].gameObject.SetActive(false);
            }
        }

        private Action OnUpdate = null;
        // private List<Action> OnUpdate = new List<Action>();
        private void Update()
        {
            // foreach (var act in OnUpdate)
            // {
            //     act.Invoke();
            // }
            OnUpdate?.Invoke();
        }

        #region 查看

        private bool isFocused;
        void OnFocus()
        {
            if (!isInDeathFunc)
            {
                OnUpdate = ShowTooltip;   
            }
        }

        void OnUnfocus()
        {
            if (!isInDeathFunc)
            {
                OnUpdate = null;   
            }
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
                if (featureTouchArea.Contains(ret.gameObject))
                {
                    if (!tooltipTrans.gameObject.activeSelf)
                    {
                        int index = featureTouchArea.IndexOf(ret.gameObject);
                        OnShowTooltip.Invoke(IdToSO.FindFeatureSOByEnum(card.features[index]));
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

        #endregion

        #region 左键拖拽 生面

        private bool isDraging;
        void OnDragStart()
        {
            if (Input.GetMouseButton(0))
            {
                isDraging = true;
                
                canvasGroup.alpha = 0.5f;

                SelectMapStartCommand comm = new SelectMapStartCommand();
                comm.area = new SelectArea() {width = card.width, height = card.height, selectStage = MapSelectStage.IsPutPiece};
                this.SendCommand<SelectMapStartCommand>(comm);   
            }
        }

        void OnDragEnd()
        {
            if (isDraging)
            {
                canvasGroup.alpha = 1f;
                this.SendCommand<SelectMapEndCommand>(new SelectMapEndCommand(this, false));

                isDraging = false;
            }
        }

        #endregion

        #region 右键点击 死面

        private bool isInDeathFunc;
        public DeathFuncBase deathFunc;
        void OnRightClick()
        {
            if (!isInDeathFunc && Input.GetMouseButtonUp(1))  // 右键
            {
                // if (isInDeathFunc)  // 死面状态下再次右键点击卡牌，取消死面功能施放
                // {
                //     Debug.Log("cancel death func");
                //     isInDeathFunc = false;
                //     this.SendCommand<SelectMapEndCommand>(new SelectMapEndCommand(this, true));
                //     return;
                // }
                
                // 这里要得到死面的范围信息
                string deathFuncName = Extensions.GetDeathFuncTypeName(card.charaID);
                if (deathFuncName.IsNullOrEmpty())
                {
                    Debug.LogError("failed to load death func");
                    return;
                }
                
                Debug.Log("start death func");
                isInDeathFunc = true;
                OnUpdate = CheckDeathFuncMouse;

                Type methodType = Type.GetType("Game." + deathFuncName);
                var obj = methodType.Assembly.CreateInstance("Game." + deathFuncName);
                deathFunc = obj as DeathFuncBase;
                deathFunc.viewCard = this;

                SelectMapStartCommand comm = new SelectMapStartCommand();
                // comm.area = new SelectArea() {width = card.width, height = card.height, selectStage = MapSelectStage.IsPutDeathFunc};
                comm.area = deathFunc.area;
                this.SendCommand<SelectMapStartCommand>(comm);
            }
        }

        void CheckDeathFuncMouse()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (isInDeathFunc)  // 死面状态下再次右键点击卡牌，取消死面功能施放
                {
                    Debug.Log("cancel death func");
                    isInDeathFunc = false;
                    OnUpdate = null;
                    this.SendCommand<SelectMapEndCommand>(new SelectMapEndCommand(this, true));
                    deathFunc = null;
                    return;
                }
            }
            else if (Input.GetMouseButtonDown(0))   // 左键施放死面功能
            {
                if (isInDeathFunc)
                {
                    Debug.Log("execute death func");
                    isInDeathFunc = false;
                    OnUpdate = null;
                    this.SendCommand<SelectMapEndCommand>(new SelectMapEndCommand(this, false));
                    deathFunc = null;
                    return;
                }
            }
        }
        
        #endregion
        
        
        void OnUseAsLifeCard(PutPieceByHandCardEvent e)
        {
            // 检测通知的是不是自己
            if (e.viewCard != this) return;
            if (this.GetSystem<IPieceSystem>().GetLastSpawnedFriend(true) != null &&
                e.viewCard.card.charaID == this.GetSystem<IPieceSystem>().GetLastSpawnedFriend(true).card.charaID &&
                e.viewCard.card.HasFeature(FeatureEnum.Insomnia))
            {
                Debug.Log("失眠症，无法连续放置");
                return;
            }

            this.SendCommand(new PutPieceCommand(this, e.pieceGrids));
            if (e.viewCard.card.charaName == "弗朗西斯·维兰德·瑟斯顿")
            {
                Dialogue dialogue = GameObject.Find("Dialogue")?.GetComponent<Dialogue>();
                if (dialogue != null) dialogue.getControl = true;

            }
            // todo 手牌使用后的后续处理（此时已经移出手牌系统并隐藏），如返回背包、销毁...
            Debug.Log("after card use as life card");
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(e.viewCard.card);
            



        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}