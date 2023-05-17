using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace BagUI
{
	public class BagUIPanelData : UIPanelData
	{
	}
	public partial class BagUIPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BagUIPanelData ?? new BagUIPanelData();
			// please add init code here
			Close.onClick.AddListener(() =>
			{
				Hide();
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
