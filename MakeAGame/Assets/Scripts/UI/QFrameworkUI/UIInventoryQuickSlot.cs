using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using BagUI;
using ItemUI;
using System;
using TMPro;
using ItemInfo;
using static UnityEditor.Progress;
using Unity.VisualScripting;

namespace InventoryQuickslotUI
{
	public class UIInventoryQuickSlotData : UIPanelData
	{
		public IInventorySystem inventory = GameEntry.Interface.GetSystem<IInventorySystem>();


	}
	public partial class UIInventoryQuickSlot : UIPanel
	{
        public Transform itemSlotContainer;
        public Transform itemSlotTemplate;
		private int activeQuickSlotCount;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIInventoryQuickSlotData ?? new UIInventoryQuickSlotData();
			CombatSceneButton.onClick.AddListener(() => 
			{
				if (UIKit.GetPanel<UIHandCard>().m_close == false)
				{
					UIKit.GetPanel<UIHandCard>().CloseHandCard();
				}
				
				UIKit.ShowPanel<BagUIPanel>();
				UIKit.GetPanel<BagUIPanel>().RefreshLayout();
				



			});

        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
			RefreshInventoryItems();
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

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1) && activeQuickSlotCount >= 1)
			{
				GameEntry.Interface.GetSystem<IInventorySystem>().UseItem(mData.inventory.GetItemList()[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && activeQuickSlotCount >= 2)
            {
                GameEntry.Interface.GetSystem<IInventorySystem>().UseItem(mData.inventory.GetItemList()[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && activeQuickSlotCount >= 3)
            {
                GameEntry.Interface.GetSystem<IInventorySystem>().UseItem(mData.inventory.GetItemList()[2]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && activeQuickSlotCount >= 4)
            {
                GameEntry.Interface.GetSystem<IInventorySystem>().UseItem(mData.inventory.GetItemList()[3]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && activeQuickSlotCount >= 5)
            {
                GameEntry.Interface.GetSystem<IInventorySystem>().UseItem(mData.inventory.GetItemList()[4]);
            }
			if(UIKit.GetPanel<UIHandCard>().m_close == true)
            {
				GameManager.Instance.PauseGame();

			}
        }

		private void RefreshInventoryItems()
		{
			foreach (Transform child in itemSlotContainer)
			{
				if (child == itemSlotTemplate) continue;
				Destroy(child.gameObject);
			}

			int x = 0;
			int y = 0;
			float itemSlotCellSize = -120f;		
			foreach (Item item in mData.inventory.GetItemList())
			{
				// 没有快捷栏需要显示的东西，直接跳出
				if (item.data.itemUseTime != ItemUseTimeEnum.Combat &&
					item.data.itemUseTime != ItemUseTimeEnum.AnyTime) break;

				int currentIndex = y;
				RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer)
					.GetComponent<RectTransform>();
				itemSlotRectTransform.gameObject.SetActive(true);
				itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize - 25);
				Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
				image.sprite = item.data.sprite;
                UIEventHelper mouseHelper = itemSlotRectTransform.AddComponent<UIEventHelper>();
                mouseHelper.OnUIPointEnter = () => MouseEnter(item);
                mouseHelper.OnUIPointExit = () => MouseExit(item);
                itemSlotRectTransform.Find("ItemAmount").GetComponent<TextMeshProUGUI>().text = "x" + item.amount.ToString();
				itemSlotRectTransform.Find("ShortcutKey").GetComponent<TextMeshProUGUI>().text = (y + 1).ToString();
				itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(() => 
				{
                    GameEntry.Interface.GetSystem<IInventorySystem>().UseItem(mData.inventory.GetItemList()[currentIndex]);
                });
                y++;
				// 快捷栏只显示五个物品，物品每次改动都会重新Sort一下所以前五个一定是优先显示的物品
				if (y > 4) break;
			}
			activeQuickSlotCount = y;

        }

		private void MouseEnter(Item item)
		{
			Debug.Log($" {item.data.itemName} mouseHelper MouseEnter");
			UIKit.OpenPanel<ItemInfoPanel>();
			UIKit.GetPanel<ItemInfoPanel>().LoadItemData(item);
		}
        private void MouseExit(Item item)
        {
            Debug.Log($" {item.data.itemName} mouseHelper MouseExit");
            UIKit.ClosePanel<ItemInfoPanel>();
		}
	}
}
