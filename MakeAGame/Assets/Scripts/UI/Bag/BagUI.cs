using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace BagUI
{
	public class BagUIData : UIPanelData
	{
	}
	public partial class BagUI : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as BagUIData ?? new BagUIData();
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
