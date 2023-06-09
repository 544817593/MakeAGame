using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Assertions;
using PackOpen;

namespace ShopBuyUI
{
	public class ShopBuyUIData : UIPanelData
	{
        public IShopSystem shopSystem = GameEntry.Interface.GetSystem<IShopSystem>();
        public IInventorySystem inventorySystem = GameEntry.Interface.GetSystem<IInventorySystem>();
    }
	public partial class ShopBuyUI : UIPanel
	{
        
        private int playerGold = 0;
        private int buyCount = 1; // 物品购买数量
        private int gridNum = 10; // 购买栏位上限
		private Dictionary<Button, Item> activeButtons = new Dictionary<Button, Item>();
		private Item selectedItem = null;
		private Button selectedButton = null;
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ShopBuyUIData ?? new ShopBuyUIData();

            // 监听按钮点击，跳转panel
            ShopPanelChange.ChangeShopPanel(this, Close);
            // TODO: 读取玩家当前金币数量，暂时使用hardcode
            playerGold = GameManager.Instance.playerMan.player.GetGold();

            UpdateAndShowShopItems();
			ShowItemInfo();
			CounterLogic();
			buyItem();
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
		/// <summary>
		/// 购买页面的初始化
		/// </summary>
        private void UpdateAndShowShopItems()
        {
			List<Item> shopItemList = mData.shopSystem.GetShopItemList();
			if (shopItemList.Count > ShopGridPanel.GetComponentInChildren<Transform>(includeInactive: true).childCount) 
			{
				Debug.LogError($"商店购买栏位上限为{gridNum}，超过上限，只能显示数组中倒数{gridNum}种物品");
			}
			int idx = 0;
            
			// 玩家金币读取
            TextGold.text = $": {playerGold}";
			// 物品购买数量初始化
			TextCount.text = $"{buyCount}";
			// 初始化每个Item图标，数量，花费
            foreach (Transform transform in ShopGridPanel.GetComponentInChildren<Transform>(includeInactive : true))
			{
                if (idx >= shopItemList.Count) // shopItemList已遍历完就结束
                {
                    return;
                }
				Debug.Log($"idx : {idx}, count : {shopItemList.Count}");
                GameObject curItem = transform.gameObject;
				Item itemInList = shopItemList[idx];
				curItem.SetActive(true); 
				curItem.GetComponent<Image>().sprite = itemInList.data.sprite;
				foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
				{
					Debug.Log(texts.gameObject.name);
					if (texts.gameObject.name == "ItemNum")
					{
						texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.amount.ToString();
					}
					else if (texts.gameObject.name == "ItemCost")
					{
						texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.data.buyCost.ToString();
					}
				}
				// 加入activeButtons dictionary
				activeButtons.Add(curItem.GetComponent<Button>(), itemInList);
                Debug.Log(transform.gameObject.name);
				idx++;
            }
			
        }

		/// <summary>
		/// 监听每个active的物品，点击后显示对应描述，更新selectedItem
		/// </summary>
        private void ShowItemInfo()
		{
			foreach(Button btn in activeButtons.Keys)
			{
				btn.onClick.AddListener(() =>
				{
					TextItemInfo.text = $"{activeButtons[btn].data.itemName}: {activeButtons[btn].data.description}";
					selectedItem = activeButtons[btn];
					selectedButton = btn;
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
				if (selectedItem != null) {
					if(buyCount >= selectedItem.amount)
					{
						Debug.Log("无法增加购买数量");
					}
                    else if ((buyCount + 1) * selectedItem.data.buyCost > playerGold)
                    {
                        Debug.Log("当前金币数量不足，无法增加购买数量");
                    }
                    else
                    {
                        buyCount++;
                        TextCount.text = $"{buyCount}";
                    }
				}
				else
				{
					Debug.Log("请先选择物品");
				}
				
            });
            BtnSub.onClick.AddListener(() =>
            {
				if(buyCount > 1) 
				{
                    buyCount--;
                    TextCount.text = $"{buyCount}";
                }
                else
                {
					Debug.Log("无法减少购买数量");
                }
            });
        }
		private void buyItem()
		{
			Buy.onClick.AddListener(() =>
			{
				if (selectedItem == null)
				{
					Debug.Log("未选择物品");
				}
				else if (selectedItem.amount == 0)
				{
					Debug.Log("物品已售空");
				}
				else if (playerGold < buyCount * selectedItem.data.buyCost)
				{
					Debug.Log("金币不足");
				}
				else
				{
					// 物品入包，更新所有相关内容
					if(selectedItem.data.itemName != "卡包")
					{
                        addItemToAllList(selectedItem.data, buyCount);
					}
					else
					{
						UIKit.OpenPanel<UIOpenPackPanel>();
						UIKit.GetPanel<UIOpenPackPanel>().transform.SetAsLastSibling();
						
                    }
					
					UpdateViewAfterBuy();
				}
			});
		}

        
        /// <summary>
        /// Item存放的地方有，activeButtons，shopItemList，selectedItem，其中一个的数据改变会连带剩余位置一起改变
        /// 更新 1.物品剩余数量 2. 玩家剩余金币 
		///	重置	 1. counter重置  2. selectedItem selectedButton重置为null 3. 文本描述重置为空
        /// </summary>
        private void UpdateViewAfterBuy()
		{
			// 更新
			Assert.IsNotNull(selectedItem, "UpdateViewAfterBuy()中selectedItem不能为null");
            selectedItem.amount -= buyCount;
            selectedButton.transform.Find("ItemNum").GetComponent<TextMeshProUGUI>().text = selectedItem.amount.ToString();
			playerGold -= buyCount * selectedItem.data.buyCost;
            TextGold.text = $": {playerGold}";

			// 重置
            buyCount = 1;
            TextCount.text = $"{buyCount}";
			//可能需要amount == 0后移除shopItemList，activeButtons中对应的元素？

			//Debug.Log($"{mData.shopSystem.GetshopItemList()[0].amount}");
			
		}
		/// <summary>
		/// 购买物品同步到所有list中
		/// </summary>
		/// <param name="itemData"></param>
		/// <param name="amount"></param>
		private void addItemToAllList(SOItemBase itemData, int amount)
		{
			// 检查真实背包中是否包含这个物品，包含的话叠加数量
            foreach(var item in mData.inventorySystem.GetItemList())
			{
				if(item.data.itemName == itemData.itemName)
				{
					item.amount += amount;
                    return;
                }
			}
			// 因为真实背包中没有包含这个物品，就新创建物品
            mData.inventorySystem.AddItem(new Item { amount = amount, data = itemData });
			mData.shopSystem.GetAllBagItemList().Add(new Item { amount = amount, data = itemData });
			if (itemData.itemUseTime == ItemUseTimeEnum.Merchant)
			{
                mData.shopSystem.GetEnhanceBagItemList().Add(new Item { amount = amount, data = itemData });
            }
        }
    }
}
