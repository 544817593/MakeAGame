using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopBuyUI
{
	public class ShopBuyUIData : UIPanelData
	{
	}
	public partial class ShopBuyUI : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ShopBuyUIData ?? new ShopBuyUIData();
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
