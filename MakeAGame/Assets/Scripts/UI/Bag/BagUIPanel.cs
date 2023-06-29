using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using System;
using System.Collections.Generic;
using TMPro;

namespace BagUI
{
	public class BagUIPanelData : UIPanelData
	{
		public IInventorySystem inventorySystem = GameEntry.Interface.GetSystem<IInventorySystem>();
		

	}
	public partial class BagUIPanel : UIPanel
	{
		// 背包每页的格子数量上限
		private const int gridNum = 10;
		private int curPage = 1;
		private int totalPage = 1;
		// 每页显示的元素索引区间[lowerIndex, upperIndex]
		private int upperIndex = gridNum - 1;
		private int lowerIndex = 0;
        private Transform mCardRoot;
        private List<Transform> mListSlotPosition = new List<Transform>();
		public List<ViewBagCard> cardList = new List<ViewBagCard>();
		


		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BagUIPanelData ?? new BagUIPanelData();
			// please add init code here
			mCardRoot = SlotPosition.GetComponent<Transform>();
			cardList = mData.inventorySystem.GetBagCardList();
			
			var rootChilds = mCardRoot.GetComponentInChildren<Transform>(includeInactive: true);
			foreach (Transform cardPos in rootChilds)
			{
				mListSlotPosition.Add(cardPos);
			}
			BagUIChange.ChangeBagPanel(this, Card);
			BagUIChange.ChangeBagPanel(this, Item);
			RefreshLayout();
			updateIndex();
			//UpdateLayout();

			pageChange();

			Close.onClick.AddListener(() =>
			{
				CheckBag m_bag = GameObject.Find("Bag")?.GetComponent<CheckBag>();
				m_bag?.SetIsOpenFalse();
				GameManager.Instance.ResumeGame();
				Hide();
				if (UIKit.GetPanel<UIHandCard>()?.m_close == true)
				{
					UIKit.GetPanel<UIHandCard>()?.OpenHandCard();
				}
			});
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			RefreshLayout();
			//UpdateLayout();
			updateIndex();
			
		}
		
		protected override void OnShow()
		{
			RefreshLayout();
			//UpdateLayout();
			updateIndex();
			
		}
		
		protected override void OnHide()
		{
			RefreshLayout();
			//UpdateLayout();
			updateIndex();
			
		}
		
		protected override void OnClose()
		{
			
		}
      
        public void RefreshLayout()
		{
			totalPage = cardList.Count != 0 ? (int)Math.Ceiling((double)cardList.Count / gridNum) : 1;
			// 页数显示
			TextPageNum.text = $" {curPage} / {totalPage}";
			int idx = 0;
			foreach (ViewBagCard v_card in cardList)
			{
				v_card.transform.SetParent(SlotPosition.transform);
				v_card.transform.position = mListSlotPosition[(idx%10)].transform.position;
				v_card.transform.GetComponent<Canvas>().sortingLayerName = "UI";
				if (idx >= lowerIndex && idx <= upperIndex)
				{
					
					v_card.gameObject.SetActive(true);
				}
				else
				{
					
					v_card.gameObject.SetActive(false);
				}
				idx++;
			}

		}
		//public void UpdateLayout()
  //      {
			
		//	int minIndex = Mathf.Min(cardList.Count, mListSlotPosition.Count);
		//	int index;
			
		//		for (index = 0; index < cardList.Count+1; index++)
		//		{
		//			cardList[index].transform.SetParent(SlotPosition.transform);
		//			cardList[index].transform.position = mListSlotPosition[index].transform.position;
		//			cardList[index].transform.GetComponent<Canvas>().sortingLayerName = "UI";
				
		//	}
		//		//// 牌被抽走后在背包的显示里清除(此时bagCardList已经更新完毕)
		//		//for (index = minIndex; index < mListSlotPosition.Count; index++)
		//		//{
		//		//	mListSlotPosition[index].transform.DestroyChildren();
  //  //            }
		//	RefreshLayout();

		//	updateIndex();


		//	//	for (int i = 0; i< minIndex; i++)
		//	//{
		//	//	cardList[i].transform.SetParent(mListSlotPosition[i].transform);
		//	//	cardList[i].transform.position = mListSlotPosition[i].transform.position;
		//	//}

		//}

		public void AddCard(Card cardData)
        {
			mData.inventorySystem.SpawnBagCardInBag(cardData);
		}

		private void updateIndex()
		{
			

			int bagCount = cardList.Count;
			lowerIndex = (curPage - 1) * gridNum;
			// 索引上限为当前页*格子数量-1，如果超过list大小，则为list的元素数量-1
			upperIndex = curPage * gridNum - 1 >= bagCount ? bagCount - 1 : curPage * gridNum - 1;
		}

		private void pageChange()
		{
			BtnNextPage.onClick.AddListener(() =>
			{
				if (curPage >= totalPage)
				{
					Debug.Log("无法往后翻页");
				}
				else
				{
					curPage++;
					updateIndex();
					RefreshLayout();
				}

			});
			BtnPrePage.onClick.AddListener(() =>
			{
				if (curPage == 1)
				{
					Debug.Log("无法往前翻页");
				}
				else
				{
					curPage--;
					updateIndex();
					RefreshLayout();
				}
			});
		}

	}
}
