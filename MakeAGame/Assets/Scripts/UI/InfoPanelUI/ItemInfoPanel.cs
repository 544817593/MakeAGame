using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;

namespace ItemInfo
{
	public class ItemInfoPanelData : UIPanelData
	{
	}
	public partial class ItemInfoPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ItemInfoPanelData ?? new ItemInfoPanelData();
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

		public void LoadItemData(Item item)
		{
			ItemName.text = item.data.itemName;
			ItemDescription.text = item.data.description;
		}
	}
}
