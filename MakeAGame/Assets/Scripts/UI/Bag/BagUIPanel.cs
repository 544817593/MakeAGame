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
        private List<Transform> mListSlotPosition;
		private List<ViewBagCard> cardList;
		


		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BagUIPanelData ?? new BagUIPanelData();
			// please add init code here
			mCardRoot = SlotPosition.GetComponent<Transform>();
			mListSlotPosition = new List<Transform>();
			cardList = mData.inventorySystem.GetBagCardList();
			

			var rootChilds = mCardRoot.GetComponentInChildren<Transform>(includeInactive: true);
			foreach (Transform cardPos in rootChilds)
			{
				mListSlotPosition.Add(cardPos);
			}
			
			RefreshLayout();
			pageChange();

			Close.onClick.AddListener(() =>
			{
				CheckBag m_bag = GameObject.Find("Bag")?.GetComponent<CheckBag>();
				m_bag?.SetIsOpenFalse();
				Hide();
			});
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

		private void RefreshLayout()
		{
			totalPage = cardList.Count != 0 ? (int)Math.Ceiling((double)cardList.Count / gridNum) : 1;
			// 页数显示
			TextPageNum.text = $" {curPage} / {totalPage}";
			int idx = lowerIndex;
			foreach (Transform m_Transform in mListSlotPosition)
			{
				GameObject curItem = transform.gameObject;
				
				if (idx > upperIndex || idx >= cardList.Count)
				{
					curItem.SetActive(false);
				}
				else
				{
					ViewBagCard card_inList = cardList[idx];
					curItem.SetActive(true);
				}
				idx++;
			}

		}
		public void UpdateLayout()
        {
			int minIndex = Mathf.Min(cardList.Count, mListSlotPosition.Count);
			int index;
			if (cardList.Count <= mListSlotPosition.Count)
			{
				for (index = 0; index < minIndex; index++)
				{
					cardList[index].transform.SetParent(mListSlotPosition[index].transform);
					cardList[index].transform.position = mListSlotPosition[index].transform.position;
					cardList[index].transform.GetComponent<Canvas>().sortingLayerName = "UI";
		
				}

			}
			else
			{
				for (index = 0; index < minIndex; index++)
				{
					cardList[index].transform.SetParent(mListSlotPosition[index].transform);
					cardList[index].transform.position = mListSlotPosition[index].transform.position;
				}
				if (index >=minIndex)
                {
					List<Transform> n_Pos = mListSlotPosition;
					for (int i = 0; i < minIndex; i++)
                    {
						cardList[index+i].transform.SetParent(n_Pos[i].transform);
						cardList[index+i].transform.position = n_Pos[i].transform.position;
					}
                }

			}


			//	for (int i = 0; i< minIndex; i++)
			//{
			//	cardList[i].transform.SetParent(mListSlotPosition[i].transform);
			//	cardList[i].transform.position = mListSlotPosition[i].transform.position;
			//}
            
        }

		public void AddCard(Card cardData)
        {
			mData.inventorySystem.SpawnBagCardInBag(cardData);
			UpdateLayout();
		}

		private void updateIndex()
		{
			int bagCount = mData.inventorySystem.GetBagCardList().Count;
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
