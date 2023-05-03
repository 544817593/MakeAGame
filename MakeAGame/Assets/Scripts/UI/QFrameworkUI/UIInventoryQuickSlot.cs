using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;

namespace InventoryQuickslotUI
{
	public class UIInventoryQuickSlotData : UIPanelData
	{
		public InventorySystem inventory = GameEntry.Interface.GetSystem<InventorySystem>();


	}
	public partial class UIInventoryQuickSlot : UIPanel
	{
        public Transform itemSlotContainer;
        public Transform itemSlotTemplate;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIInventoryQuickSlotData ?? new UIInventoryQuickSlotData();
			// please add init code here
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

		private void RefreshInventoryItems()
		{
			int x = 0;
			int y = 0;
			float itemSlotCellSize = 30f;
			Debug.LogWarning(mData.inventory);
			foreach (Item item in mData.inventory.GetItemList())
			{
				RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer)
					.GetComponent<RectTransform>();
				itemSlotRectTransform.gameObject.SetActive(true);
				itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
				y++;
			}
		}
	}
}
