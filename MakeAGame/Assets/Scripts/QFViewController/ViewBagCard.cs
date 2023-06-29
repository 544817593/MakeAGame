using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using QFramework;
using ShootingEditor2D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public partial class ViewBagCard: MonoBehaviour, IController
    {
        // 卡牌数据
        public Card card;
        public string enhanceDeathDescription;
        public float normalScale = 0.08f;   // 缩放系数
        public float largeScale = 0.16f;
        public Action OnTouchAction;    // 点击时触发的事件
        public Action OnFocusAction;    // 背包选取时触发的事件
        public Action OnUnFocusAction;  // 背包取消选取时触发的事件
        private void Awake()
        {
            // instantiate以后立刻执行，防止同一帧的某些操作需要调用其组件
            gameObject.AddComponent<GraphicRaycaster>();    // 这个必须放在canvas绑定前面，因为它会连带创建canvas
            InitBind(); // 绑定各组件
        }

        private void Start()
        {
            // gameObject.AddComponent<GraphicRaycaster>();    // 这个必须放在canvas绑定前面，因为它会连带创建canvas
            //
            // InitBind(); // 绑定各组件
            
            var uiHelper = touchArea.AddComponent<UIEventHelper>();
            
            canvas.overrideSorting = true;
            transform.localScale = new Vector3(normalScale, normalScale, 1f);

            uiHelper.OnUIPointEnter += OnFocus;
            uiHelper.OnUIPointExit += OnUnfocus;
            uiHelper.OnUIPointDown += OnTouch;

            InitView();
        }

        public void InitView()
        {
            UpdateEnhanceDeathDescription();
            imgRarity.sprite = Extensions.GetRaritySprite(card.rarity);
            imgCharacter.sprite = card.cardSprite;
            tmpSanCost.text = card.sanCost.ToString();
            tmpHP.text = card.hp.ToString();
            tmpMoveSpd.text = card.moveSpd.ToString();
            tmpDamage.text = card.damage.ToString();
            tmpDefend.text = card.defend.ToString();
            tmpName.text = card.charaName;
            tmpDesc.text = card.deathFuncDesc + enhanceDeathDescription;
            
            for(int i = 0; i < card.features.Count; i++)
            {
                featureTouchArea[i].GetComponent<Image>().sprite = IdToSO.FindFeatureSOByEnum(card.features[i]).icon;
            }
            for (int i = card.features.Count; i < 3; i++)
            {
                featureTouchArea[i].gameObject.SetActive(false);
            }
        }

        public void UpdateEnhanceDeathDescription()
        {
            enhanceDeathDescription = card.deathEnhancement.ToString();
        }


        void OnFocus()
        {
            canvas.sortingOrder = 100;
            transform.DOScale(largeScale, 0.5f);
            OnFocusAction?.Invoke();
            
        }

        void OnUnfocus()
        {
            canvas.sortingOrder = 0;
            transform.DOScale(normalScale, 0.5f);
          OnUnFocusAction?.Invoke();
            
        }
        
        void OnTouch()
        {
            Debug.Log("touch");
            OnTouchAction?.Invoke();
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}