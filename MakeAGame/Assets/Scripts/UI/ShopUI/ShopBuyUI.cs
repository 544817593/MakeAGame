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
            Close.onClick.AddListener(() =>
            {
                Debug.Log($"点击了{Close.name}按钮");
                this.CloseSelf();
                UIKit.OpenPanel<ShopMainUI.ShopMainUI>();
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
