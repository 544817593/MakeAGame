using UnityEngine;
using QFramework;

namespace Game
{
	public class UIConsolePanelData : UIPanelData
	{
	}

	public partial class UIConsolePanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIConsolePanelData ?? new UIConsolePanelData();
			
			// please add init code here
			crtMode = WindowMode.Window;
			SetMode(WindowMode.Window);
			InputField.ActivateInputField();
			BtnConsole.onClick.AddListener(() => SetMode(WindowMode.FullScreen));
		}

		protected override void OnOpen(IUIData uiData = null)
		{
		}

		private void Update()
		{
			// 要是hide了根本不会走update
			// if (State == PanelState.Opening && Input.GetKeyDown(KeyCode.Return))
			if (Input.GetKeyDown(KeyCode.Return))
			{
				TextOutput.text += $">>{Console.Input(InputField.text)}\n";
				InputField.text = "";
				InputField.ActivateInputField();
			}

			if (Input.GetKeyDown(KeyCode.Tab))
			{
				Debug.Log("tab");
				if(crtMode == WindowMode.Window)
					SetMode(WindowMode.FullScreen);
				else
					SetMode(WindowMode.Window);
			}
		}

		private enum WindowMode
		{
			FullScreen,
			Window
		}

		private WindowMode crtMode;
		private void SetMode(WindowMode mode)
		{
			crtMode = mode;
			FullScreenNode.gameObject.SetActive(mode == WindowMode.FullScreen);
			WindowNode.gameObject.SetActive(mode == WindowMode.Window);
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
