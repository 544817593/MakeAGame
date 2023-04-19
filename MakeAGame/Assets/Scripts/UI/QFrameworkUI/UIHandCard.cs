using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	public class UIHandCardData : UIPanelData
	{
	}
	/// <summary>
	/// 手牌区域
	/// </summary>
	public partial class UIHandCard : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIHandCardData ?? new UIHandCardData();
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
