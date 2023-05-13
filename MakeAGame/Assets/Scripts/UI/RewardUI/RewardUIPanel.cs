using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace RewardUI
{
	public class RewardUIPanelData : UIPanelData
	{
	}
	public partial class RewardUIPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as RewardUIPanelData ?? new RewardUIPanelData();
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
