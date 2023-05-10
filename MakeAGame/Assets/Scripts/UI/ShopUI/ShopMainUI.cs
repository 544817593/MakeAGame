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
            // please add init code here
			Buy.onClick.AddListener(() =>
            {
				Debug.Log($"点击了Buy按钮");
				this.CloseSelf();
				UIKit.OpenPanel<ShopBuyUI.ShopBuyUI>();
            });
			
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
