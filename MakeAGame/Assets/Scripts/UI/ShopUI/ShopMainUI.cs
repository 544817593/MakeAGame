using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopMainUI
{
	public class ShopMainUIData : UIPanelData
	{
	}
	
	public partial class ShopMainUI : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ShopMainUIData ?? new ShopMainUIData();
            
			// 监听按钮点击，跳转panel
            ShopPanelChange.changeShopPanel(this, Buy);
			ShopPanelChange.changeShopPanel(this, Sell);
            ShopPanelChange.changeShopPanel(this, LevelUp);
            ShopPanelChange.changeShopPanel(this, ShopNPC);

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
