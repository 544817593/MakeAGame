using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using Unity.VisualScripting;

namespace Game
{
	public class UIHandCardData : UIPanelData
	{
	}
	/// <summary>
	/// 手牌区域
	/// </summary>
	public partial class UIHandCard : UIPanel
	{
		private Transform nodeTooltip;
		private TextMeshProUGUI tmpTooltip;
		public ViewDirectionWheel viewDirectionWheel;
		
		private Animator anim;
		public bool m_close;
		private List<ViewCard> viewCardsList;	// 手牌列表
		private List<Transform> cardPosList = new List<Transform>();	// 手牌默认位置列表

		private int focusIndex;	// 鼠标选中卡牌在列表中序号
		private bool isDragging;	// 是否正在拖拽，此时屏蔽其他操作

		public const float normalScale = 0.15f;   // 普通缩放系数
		private const float zoomScale = 0.4f;      // 放大系数
		private const float detailOffsetY = 453f;	// 卡牌放大时的位置偏移y
		public const int normalSortingLayer = 101;

		[SerializeField] private TextMeshProUGUI sanValue; // 混沌值显示

		private SOPlayer player;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHandCardData ?? new UIHandCardData();
			// please add init code here

			nodeTooltip = transform.Find("Root/Tooltip");
			nodeTooltip.gameObject.SetActive(false);
			tmpTooltip = nodeTooltip.GetChild(0).GetComponent<TextMeshProUGUI>();

			viewDirectionWheel = new ViewDirectionWheel(transform.Find("DirectionWheel").gameObject);
			viewDirectionWheel.gameObject.SetActive(false);
			m_close = false;
			PieceIcon.gameObject.SetActive(false);
			
			anim = GetComponent<Animator>();
			foreach (Transform cardPos in CardRoot)
			{
				if(cardPos.name.Contains("CardPos"))
					cardPosList.Add(cardPos);
			}

			var handCardSystem = GameEntry.Interface.GetSystem<IHandCardSystem>();
			handCardSystem.ui = this;
			viewCardsList = handCardSystem.handCardList;
			focusIndex = -1;
			ImgMP.fillAmount = 1f;

			player = PlayerManager.Instance.player;			
			SetCrtSanityToMaxSan();

			#region 测试按钮

			ButtonAddCardTest.onClick.AddListener(() =>
			{
				var randomInfo = IdToSO.GetRandomCardSO();
				GameEntry.Interface.SendCommand<AddHandCardCommand>(new AddHandCardCommand(new Card(randomInfo.characterID)));
			});
			ButtonSubCardTest.onClick.AddListener(() =>
			{
				if (handCardSystem.handCardList.Value.Count > 0)
				{
					GameEntry.Interface.SendCommand<SubHandCardCommand>(new SubHandCardCommand(handCardSystem.handCardList.Value[0]));	
				}
			});

			ButtonOpenAnim.onClick.AddListener(() =>
			{
				anim.Play("Open");
				
			});
			ButtonCloseAnim.onClick.AddListener(() =>
			{
				anim.Play("Close");
			});

			#endregion

			// 计时
			GameEntry.Interface.RegisterEvent<CountTimeEvent>(OnSecond);
		}

		public void UpdateMaxSanDisplay()
		{
            sanValue.text = player.GetSan().ToString() + "/" + player.GetMaxSan().ToString();
        }

		public void SetCrtSanityToMaxSan()
		{
            player.SetSan(player.GetMaxSan());
            sanValue.text = player.GetSan().ToString() + "/" + player.GetMaxSan().ToString();
        }

		void OnSecond(CountTimeEvent e)
		{
			OnSanChange((int)GameManager.Instance.playerMan.player.GetSanRegenSpeed());
		}

		public void OnSanChange(int amount)
		{
			player.SetSan(Mathf.Clamp(player.GetSan() + amount, 0, player.GetMaxSan()));
			ImgMP.fillAmount = (float)player.GetSan() / player.GetMaxSan();
            sanValue.text = player.GetSan().ToString() + "/" + player.GetMaxSan().ToString();
        }


		public void AddCard(int index)
		{
			viewCardsList[index].transform.localScale = new Vector3(normalScale, normalScale, 1f);

			UpdateLayout();
		}

		public void SubCard(int index)
		{
			UpdateLayout();
		}

		public void OnFocusCard(ViewCard viewCard)
		{
			if (isDragging) return;
			
			focusIndex = viewCardsList.IndexOf(viewCard);

			viewCard.transform.localScale = new Vector3(zoomScale, zoomScale, 1f);
			var tmpPos = cardPosList[focusIndex].localPosition;
			tmpPos.y += detailOffsetY;
			viewCard.transform.localPosition = tmpPos;
			viewCard.canvas.sortingOrder = 110;
			
			nodeTooltip.SetParent(viewCard.tooltipTrans);
			nodeTooltip.localPosition = Vector3.zero;
			nodeTooltip.gameObject.SetActive(true);

			UpdateLayout();
		}

		public void OnUnfocusCard()
		{
			// if (isDragging) return;
			
			viewCardsList[focusIndex].canvas.sortingOrder = normalSortingLayer;

			focusIndex = -1;
			
			nodeTooltip.SetParent(transform.Find("Root"));
			nodeTooltip.localScale = Vector3.one;
			nodeTooltip.gameObject.SetActive(false);
			
			UpdateLayout();
		}

		public void OnDragCardStart(ViewCard viewCard)
		{
			if (Input.GetMouseButton(0))
			{
				isDragging = true;
				
				ImgPieceIcon.sprite = viewCard.card.pieceSprite;
				ImgPieceIcon.SetNativeSize();	// 恢复原大小
				PieceIcon.gameObject.SetActive(true);
				anim.Play("Down", -1, 0);
				GameManager.Instance.soundMan.Play_hide_bag_sound();
                GameManager.Instance.soundMan.Play_drag_card_sound();
            }
		}
		public void PopCard(string cardId)
        {
			
			foreach (ViewCard Vcard in viewCardsList)
            {
				if(Vcard.card.charaName==cardId)
                {
					focusIndex = viewCardsList.IndexOf(Vcard);
					Vcard.transform.localScale = new Vector3(1f, 1f, 1f);
					
					var tmpPos = cardPosList[focusIndex].localPosition;
					tmpPos.y += 100;
					Vcard.transform.localPosition = tmpPos;
					nodeTooltip.SetParent(Vcard.tooltipTrans);
					nodeTooltip.localPosition = Vector3.zero;
					nodeTooltip.gameObject.SetActive(true);
					UpdateLayout();
				}
            }
        }

		public void CloseHandCard()
        {
			anim.Play("Close");
			m_close = true;

        }
		public void OpenHandCard()
		{
			anim.Play("Open");
			m_close = false;

		}
		private void Update()
		{
			// todo 优化 不要在update里写if
			if (isDragging)
			{
				PieceIcon.localPosition = Extensions.ScreenToUIPos(Input.mousePosition);
			}
		}
		

		public void OnDragCardEnd()
		{
			if (isDragging)
			{
				isDragging = false;
			
				PieceIcon.gameObject.SetActive(false);
				anim.Play("Up", -1, 0);
				UpdateLayout();	
			}
		}
		
		public void UpdateLayout()
		{
			// 终止所有旧动画
			for (int i = 0; i < viewCardsList.Count; i++)
			{
				DOTween.Kill(viewCardsList[i].transform);
			}
			
			// 移出一张卡牌、立刻进入另一张卡牌时，正在进行恢复动画的牌要继续播放，所以还是刷新所有卡牌动画
			{
				Vector3 newLocalPos = new Vector3();
				Vector3 newScale = new Vector3(normalScale, normalScale, 1f);
				for (int i = 0; i < viewCardsList.Count; i++)
				{
					var crtCard = viewCardsList[i];
					newLocalPos = cardPosList[i].localPosition;

					// 若有选中卡牌，两边卡牌做位置偏移
					if (focusIndex >= 0)
					{
						// 选中卡牌动画已在OnFocusCard处理
						if(focusIndex == i) continue;
						// 选中卡牌左边的牌稍微向左，最左边不再偏移
						if (i < focusIndex)
						{
							newLocalPos.x -= 50f * ((float)(i + 1) / focusIndex);
						}
						// 选中卡牌右边的牌稍微向右，最右边的不再偏移
						else if (i > focusIndex)
						{
							newLocalPos.x += 50f * ((float) (viewCardsList.Count - i + 1) / (viewCardsList.Count - focusIndex));
						}
					}

					crtCard.transform.DOLocalMove(newLocalPos, 0.5f);
					crtCard.transform.DOScale(newScale, 0.3f);
				}
			}
		}

		public void UpdateTooltip(SOFeature so)
		{
			tmpTooltip.text = so.featureName + "\n" + so.featureDesc;
		}

		private void OnAnimEvent(string eventName)
		{
			// Debug.Log($"anim event: {eventName}");
			if (eventName == "SetCardCanvas")
			{
				foreach (var viewCard in viewCardsList)
				{
					viewCard.canvas.overrideSorting = false;
				}
			}
			else if (eventName == "UnsetCardCanvas")
			{
				foreach (var viewCard in viewCardsList)
				{
					viewCard.canvas.overrideSorting = true;
				}
			}
		}

		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
