using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace CardRewardUI
{
	public class UICardRewardPanelData : UIPanelData
	{
	}
	public partial class UICardRewardPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UICardRewardPanelData ?? new UICardRewardPanelData();
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
