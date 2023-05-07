using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace InventoryQuickslotUI
{
	public class UIAbilityPanelData : UIPanelData
	{
	}
	public partial class UIAbilityPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIAbilityPanelData ?? new UIAbilityPanelData();
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
	}
}
