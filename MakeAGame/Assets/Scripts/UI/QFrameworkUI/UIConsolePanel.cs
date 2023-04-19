using UnityEngine;
using QFramework;

namespace Game
{
	public class UIConsolePanelData : UIPanelData
	{
	}

	/// <summary>
	/// 内部控制台
	/// </summary>
	public partial class UIConsolePanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIConsolePanelData ?? new UIConsolePanelData();
			
			// please add init code here
			// 初始化为小窗口模式
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
			// 回车完成指令输入
			if (Input.GetKeyDown(KeyCode.Return))
			{
				TextOutput.text += $">>{Console.Input(InputField.text)}\n";
				InputField.text = "";
				InputField.ActivateInputField();
			}

			// tab切换窗口或全屏显示
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if(crtMode == WindowMode.Window)
					SetMode(WindowMode.FullScreen);
				else
					SetMode(WindowMode.Window);
			}
		}

		/// <summary>
		/// 控制台窗口的显示模式
		/// </summary>
		private enum WindowMode
		{
			FullScreen,
			Window
		}

		private WindowMode crtMode;
		/// <summary>
		/// 设置显示模式
		/// </summary>
		/// <param name="mode"></param>
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
