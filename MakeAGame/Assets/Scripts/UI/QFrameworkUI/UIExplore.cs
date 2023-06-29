using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class UIExploreData : UIPanelData
	{
	}
	public partial class UIExplore : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIExploreData ?? new UIExploreData();
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
