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
            ShopPanelChange.ChangeShopPanel(this, Buy);
			ShopPanelChange.ChangeShopPanel(this, Sell);
            ShopPanelChange.ChangeShopPanel(this, Enhance);
            ShopPanelChange.ChangeShopPanel(this, ShopNPC);
			Close.onClick.AddListener(() =>
			{
				GameObject.Find("GameSceneManager")?.transform.GetComponent<Game.SceneFlow>().LoadRoom();
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
