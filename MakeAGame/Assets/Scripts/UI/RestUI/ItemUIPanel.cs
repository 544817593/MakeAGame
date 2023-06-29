using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using System.Collections.Generic;
using System;
using TMPro;
using ItemInfo;
using Unity.VisualScripting;
namespace ItemUI
{
	public class ItemUIPanelData : UIPanelData
	{
		public IInventorySystem inventorySystem = GameEntry.Interface.GetSystem<IInventorySystem>();
	}
	public partial class ItemUIPanel : UIPanel
	{
		private const int gridNum = 10;
		private int curPage = 1;
		private int totalPage = 1;
		private int upperIndex = gridNum - 1;
		private int lowerIndex = 0;
		private Item m_item = null;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ItemUIPanelData ?? new ItemUIPanelData();

            BagUIChange.ChangeBagPanel(this, Card);
            BagUIChange.ChangeBagPanel(this, Item);
            RefreshLayout();
            PageChange();
            // please add init code here		
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
                List<Item> bagItemList = mData.inventorySystem.GetItemList();
                totalPage = bagItemList.Count != 0 ? (int)Math.Ceiling((double)bagItemList.Count / gridNum) : 1;
                // 页数显示
                TextPageNum.text = $" {curPage} / {totalPage}";
                int idx = lowerIndex;
                foreach (Transform transform in SlotPosition.GetComponentInChildren<Transform>(includeInactive: true))
                {
                    GameObject curItem = transform.gameObject;
                 UIEventHelper mouseHelper = curItem.AddComponent<UIEventHelper>();
                if (idx < bagItemList.Count)
                    {
                        Item itemInList = bagItemList[idx];
                        curItem.SetActive(true);
                        curItem.GetComponent<Image>().sprite = itemInList.data.sprite;
                        mouseHelper.OnUIPointEnter = () => MouseEnter(itemInList);
                        mouseHelper.OnUIPointExit = () => MouseExit(itemInList);
                    foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
                        {
                            //Debug.Log(texts.gameObject.name);
                            if (texts.gameObject.name == "Amount")
                            {
                                texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.amount.ToString();
                            }
                       
                    }

                  
                   

                }
                    else
                    {
                        curItem.GetComponent<Image>().sprite = null;
                        foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
                        {
                            //Debug.Log(texts.gameObject.name);
                            if (texts.gameObject.name == "Amount")
                            {
                                texts.gameObject.GetComponent<TextMeshProUGUI>().text = null;
                            }
                        }
                        curItem.SetActive(false);
                    }
                    idx++;
                }
        }
        private void MouseEnter(Item item)
        {
            
        }
        private void MouseExit(Item item)
        {
            
        }
        private void UpdateIndex()
        {
            
            
                int bagItemCount = mData.inventorySystem.GetItemList().Count;
                lowerIndex = (curPage - 1) * gridNum;
                // 索引上限为当前页*格子数量-1，如果超过list大小，则为list的元素数量-1
                upperIndex = curPage * gridNum - 1 >= bagItemCount ? bagItemCount - 1 : curPage * gridNum - 1;
            
        }
        private void PageChange()
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
                    UpdateIndex();
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
                    UpdateIndex();
                    RefreshLayout();
                }
            });
        }
    }
    
}
