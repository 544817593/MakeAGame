using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace DialogueUI
{
	public class DialoguePanelData : UIPanelData
	{
	}
	public partial class DialoguePanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as DialoguePanelData ?? new DialoguePanelData();
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
