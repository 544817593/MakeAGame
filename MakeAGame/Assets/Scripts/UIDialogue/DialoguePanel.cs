using Game;
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
			foreach(Button b_t in choice.GetComponentsInChildren<Button>())
            {
				b_t.Hide();
            }
		
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			// GameEntry.Interface.SendCommand<SelectMapEndCommand>(new SelectMapEndCommand(null, true));
		}
		
		protected override void OnShow()
		{
			// GameEntry.Interface.SendCommand<SelectMapEndCommand>(new SelectMapEndCommand(null, true));
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
