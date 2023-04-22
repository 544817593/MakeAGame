using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
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
		private Animator anim;

		private List<ViewCard> viewCardsList;	// 手牌列表
		private List<Transform> cardPosList = new List<Transform>();	// 

		private int focusIndex;	// 鼠标选中卡牌在列表中序号

		public float normalScale = 0.15f;   // 普通缩放系数
		public float zoomScale = 0.4f;      // 放大系数
		private float detailOffsetY = 453f;	// 卡牌放大时的位置偏移y

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHandCardData ?? new UIHandCardData();
			// please add init code here

			anim = GetComponent<Animator>();
			foreach (Transform cardPos in CardRoot)
			{
				if(cardPos.name.Contains("CardPos"))
					cardPosList.Add(cardPos);
			}
			Debug.Log($"UIHandCard OnInit: card pos {cardPosList.Count}");
			
			var handCardSystem = GameEntry.Interface.GetSystem<IHandCardSystem>();
			handCardSystem.ui = this;
			viewCardsList = handCardSystem.handCardList;
			focusIndex = -1;
			
			ButtonAddCardTest.onClick.AddListener(() =>
			{
				GameEntry.Interface.SendCommand<AddHandCardCommand>(new AddHandCardCommand(0));
			});
			// todo 动画事件
			ButtonOpenAnim.onClick.AddListener(() =>
			{
				anim.Play("Open");
				
			});
			ButtonCloseAnim.onClick.AddListener(() =>
			{
				anim.Play("Close");
			});
		}


		public void AddCard(int index)
		{
			Debug.Log($"handcard ui add card {index}");

			viewCardsList[index].transform.localScale = new Vector3(normalScale, normalScale, 1f);

			UpdateLayout();
		}

		public void OnFocusCard(ViewCard viewCard)
		{
			focusIndex = viewCardsList.IndexOf(viewCard);
			Debug.Log($"UIHandCard: OnFocusCard {focusIndex}");

			viewCard.transform.localScale = new Vector3(zoomScale, zoomScale, 1f);
			var tmpPos = cardPosList[focusIndex].localPosition;
			tmpPos.y += detailOffsetY;
			viewCard.transform.localPosition = tmpPos;
			viewCard.canvas.sortingOrder = 110;
			
			UpdateLayout();
		}

		public void OnUnfocusCard()
		{
			viewCardsList[focusIndex].canvas.sortingOrder = 100;
			
			Debug.Log($"UIHandCard: OnUnfocusCard {focusIndex}");

			focusIndex = -1;
			
			UpdateLayout();
		}
		
		public void UpdateLayout()
		{
			// Debug.Log("update layout");
			
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
							// Debug.Log($"card {i} offset arg: {i + 1} / {focusIndex} res: {50f * ((float)(i + 1) / focusIndex)}");
							newLocalPos.x -= 50f * ((float)(i + 1) / focusIndex);
						}
						// 选中卡牌右边的牌稍微向右，最右边的不再偏移
						else if (i > focusIndex)
						{
							// Debug.Log($"card {i} offset arg: {viewCardsList.Count - i + 1} / {viewCardsList.Count - focusIndex} res: {(float) (viewCardsList.Count - i + 1) / (viewCardsList.Count - focusIndex)}");
							newLocalPos.x += 50f * ((float) (viewCardsList.Count - i + 1) / (viewCardsList.Count - focusIndex));
						}
					}

					crtCard.transform.DOLocalMove(newLocalPos, 0.5f);
					crtCard.transform.DOScale(newScale, 0.3f);
				}
			}
		}

		private void OnAnimEvent(string eventName)
		{
			Debug.Log($"anim event: {eventName}");
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
