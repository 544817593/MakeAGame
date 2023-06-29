using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ItemShopInfoPanel
{
	public class ItemShopInfoPanelData : UIPanelData
	{
	}
	public partial class ItemShopInfoPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ItemShopInfoPanelData ?? new ItemShopInfoPanelData();
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
