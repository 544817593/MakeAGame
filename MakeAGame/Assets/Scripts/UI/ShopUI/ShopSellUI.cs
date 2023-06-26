using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.Assertions;

namespace ShopSellUI
{
	public class ShopSellUIData : UIPanelData
	{
        public IShopSystem shopSystem = GameEntry.Interface.GetSystem<IShopSystem>();
    }
	public partial class ShopSellUI : UIPanel
	{
        private int playerGold = 0;
        // 背包每页的格子数量上限
        private const int gridNum = 10;
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
			mData = uiData as ShopSellUIData ?? new ShopSellUIData();
            // please add init code here
            // 监听按钮点击，跳转panel
            ShopPanelChange.ChangeShopPanel(this, Close);
            playerGold = GameManager.Instance.playerMan.player.GetGold();
            RefreshLayout();
            ButtonListen();
            PageChange();
            CounterLogic();
            SellItem();
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
            List<Item> bagItemList = mData.shopSystem.GetBagItemList();
            totalPage = bagItemList.Count != 0 ? (int)Math.Ceiling((double)bagItemList.Count / gridNum) : 1;
            // 玩家金币显示
            TextGold.text = $"金币: {playerGold}";
			// 页数显示
			TextPageNum.text = $" {curPage} / {totalPage}";
            
            int idx = lowerIndex;
            foreach (Transform transform in ShopGridPanel.GetComponentInChildren<Transform>(includeInactive: true))
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
                       else if (texts.gameObject.name == "ItemCost")
                        {
                           texts.gameObject.GetComponent<TextMeshProUGUI>().text = null;
                        }
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
                            texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.data.sellPrice.ToString();
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
        private void UpdateIndex()
        {
            int bagItemCount = mData.shopSystem.GetBagItemList().Count;
            lowerIndex = (curPage - 1) * gridNum;
            // 索引上限为当前页*格子数量-1，如果超过list大小，则为list的元素数量-1
            upperIndex = curPage * gridNum - 1 >= bagItemCount ? bagItemCount - 1 : curPage * gridNum - 1;
        }

        private void PageChange()
        {
            BtnNextPage.onClick.AddListener(() => 
            {
                if(curPage >= totalPage)
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
                if(curPage == 1)
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

        /// <summary>
		/// 监听每个active的物品，点击后显示对应描述，更新selectedItem selectedButton，更新counter数字
		/// </summary>
        private void ButtonListen()
        {
            foreach (Button btn in activeButtons.Keys)
            {
                btn.onClick.AddListener(() =>
                {
                    //Debug.Log($"{btn.gameObject.name}");
                    TextItemInfo.text = activeButtons[btn].data.description;
                    selectedItem = activeButtons[btn];
                    selectedButton = btn;
                    if(selectedItem.amount < sellCount)
                    {
                        sellCount = selectedItem.amount;
                    }
                });

            }

        }

        /// <summary>
		/// 购买物品数量的加减逻辑，
		/// 增加数量后的总花费不能超过持有金币，并且不能超过商店物品数量上限
		/// 减少数量最少为1
		/// </summary>
		private void CounterLogic()
        {
            BtnAdd.onClick.AddListener(() =>
            {
                if (selectedItem != null)
                {
                    if (sellCount >= selectedItem.amount)
                    {
                        Debug.Log("无法增加出售数量");
                    }
                    else
                    {
                        sellCount++;
                        TextCount.text = $"{sellCount}";
                    }
                }
                else
                {
                    Debug.Log("请先选择物品");
                }

            });
            BtnSub.onClick.AddListener(() =>
            {
                if (sellCount > 1)
                {
                    sellCount--;
                    TextCount.text = $"{sellCount}";
                }
                else
                {
                    Debug.Log("无法减少出售数量");
                }
            });
        }

        private void SellItem()
        {
            Sell.onClick.AddListener(() =>
            {
                if (selectedItem == null)
                {
                    Debug.Log("未选择物品");
                }
                else if (selectedItem.amount < sellCount)
                {
                    Debug.Log("可出售数量不足");
                }
                else
                {
                    // 物品入包，更新所有相关内容
                    UpdateAfterSell();
                }
            });
        }

        private void UpdateAfterSell()
        {
            // 更新
            Assert.IsNotNull(selectedItem, "UpdateViewAfterBuy()中selectedItem不能为null");
            Debug.Log($"出售{selectedItem.data.name}");
            selectedItem.amount -= sellCount;
            selectedButton.transform.Find("ItemNum").GetComponent<TextMeshProUGUI>().text = selectedItem.amount.ToString();
            playerGold += sellCount * selectedItem.data.sellPrice;
            TextGold.text = $"金币: {playerGold}";

            // 重置
            sellCount = 1;
            TextCount.text = $"{sellCount}";
            if(selectedItem.amount == 0)
            {
                mData.shopSystem.GetBagItemList().Remove(selectedItem);
                activeButtons.Remove(selectedButton);
                selectedButton.GetComponent<Image>().sprite = null;
                foreach (Transform texts in selectedButton.GetComponentInChildren<Transform>())
                {
                    //Debug.Log(texts.gameObject.name);
                    if (texts.gameObject.name == "ItemNum")
                    {
                        texts.gameObject.GetComponent<TextMeshProUGUI>().text = null;
                    }
                    else if (texts.gameObject.name == "ItemCost")
                    {
                        texts.gameObject.GetComponent<TextMeshProUGUI>().text = null;
                    }
                }
                selectedButton.gameObject.SetActive(false);
                selectedItem = null;
                selectedButton = null;
                TextItemInfo.text = null;
                // 如果卖掉一个物品后最后一页变为空，并且当前页面在最后一页并且总页数不止一页，则自动将当前页-1
                if (mData.shopSystem.GetBagItemList().Count % gridNum == 0 && curPage != 1 && curPage == totalPage)
                {
                    curPage--;
                }
            }
            UpdateIndex();
            RefreshLayout();
        }
    }
}
