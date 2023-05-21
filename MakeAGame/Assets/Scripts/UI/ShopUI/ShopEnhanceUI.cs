using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using Game;
using System;
using TMPro;

namespace ShopEnhanceUI
{
	public class ShopEnhanceUIData : UIPanelData
	{
        public IShopSystem shopSystem = GameEntry.Interface.GetSystem<IShopSystem>();
    }
	public partial class ShopEnhanceUI : UIPanel
	{
        // TODO 读取玩家当前金币数量，暂时使用hardcode
        public int playerGold = 50;
        // 背包每页的格子数量上限
        private const int gridNum = 15;
        private int curPage = 1;
        private int totalPage = 1;
        // 每页显示的元素索引区间[lowerIndex, upperIndex]
        private int upperIndex = gridNum - 1;
        private int lowerIndex = 0;

        private int sellCount = 1;
        private Item selectedItem = null;
        private Button selectedButton = null;
        private Dictionary<Button, Item> activeButtons = new Dictionary<Button, Item>();
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ShopEnhanceUIData ?? new ShopEnhanceUIData();
            // please add init code here
            ShopPanelChange.changeShopPanel(this, Close);
			refreshLayout();
            buttonListen();
            pageChange();
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
        private void refreshLayout()
        {
            List<Item> bagItemList = mData.shopSystem.getBagItemList();
            totalPage = bagItemList.Count != 0 ? (int)Math.Ceiling((double)bagItemList.Count / gridNum) : 1;
            // 页数显示
            TextPageNum.text = $" {curPage} / {totalPage}";
            int idx = lowerIndex;
            foreach (Transform transform in BagPanel.GetComponentInChildren<Transform>(includeInactive: true))
            {
                GameObject curItem = transform.gameObject;
                // bagItemList已遍历完或idx超过upperidx，未填充物品的格子不显示，并且重置text和sprite
                if (idx > upperIndex || idx >= bagItemList.Count)
                {
                    curItem.GetComponent<Image>().sprite = null;
                    foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
                    {
                        //Debug.Log(texts.gameObject.name);
                        if (texts.gameObject.name == "ItemNum")
                        {
                            texts.gameObject.GetComponent<TextMeshProUGUI>().text = null;
                        }
                        //else if (texts.gameObject.name == "ItemCost")
                        //{
                        //    texts.gameObject.GetComponent<TextMeshProUGUI>().text = null;
                        //}
                    }
                    activeButtons.Remove(curItem.GetComponent<Button>());
                    curItem.SetActive(false);
                }
                else
                {
                    Item itemInList = bagItemList[idx];
                    curItem.SetActive(true);
                    curItem.GetComponent<Image>().sprite = itemInList.data.sprite;
                    foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
                    {
                        //Debug.Log(texts.gameObject.name);
                        if (texts.gameObject.name == "ItemNum")
                        {
                            texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.amount.ToString();
                        }
                        else if (texts.gameObject.name == "ItemCost")
                        {
                            texts.gameObject.SetActive(false);
                        }
                    }
                    if (!activeButtons.ContainsKey(curItem.GetComponent<Button>()))
                    {
                        activeButtons.Add(curItem.GetComponent<Button>(), itemInList);
                    }
                    else
                    {
                        activeButtons[curItem.GetComponent<Button>()] = itemInList;
                    }
                }

                idx++;
            }

        }
        private void updateIndex()
        {
            int bagItemCount = mData.shopSystem.getBagItemList().Count;
            lowerIndex = (curPage - 1) * gridNum;
            // 索引上限为当前页*格子数量-1，如果超过list大小，则为list的元素数量-1
            upperIndex = curPage * gridNum - 1 >= bagItemCount ? bagItemCount - 1 : curPage * gridNum - 1;
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
                    refreshLayout();
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
                    refreshLayout();
                }
            });
        }

        /// <summary>
		/// 监听每个active的物品，点击后显示对应描述，更新selectedItem selectedButton，更新counter数字
		/// </summary>
        private void buttonListen()
        {
            foreach (Button btn in activeButtons.Keys)
            {
                btn.onClick.AddListener(() =>
                {
                    //Debug.Log($"{btn.gameObject.name}");
                    //TextItemInfo.text = activeButtons[btn].data.description;
                    selectedItem = activeButtons[btn];
                    selectedButton = btn;
                    if (selectedItem.amount < sellCount)
                    {
                        sellCount = selectedItem.amount;
                    }
                });

            }

        }
    }
}