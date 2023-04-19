using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:d1b38981-1662-4764-9a61-eaa6b8fc9f90
	public partial class UIConsolePanel
	{
		public const string Name = "UIConsolePanel";
		
		/// <summary>
		/// 全屏模式显示
		/// </summary>
		[SerializeField]
		public RectTransform FullScreenNode;
		/// <summary>
		/// 控制台输入框
		/// </summary>
		[SerializeField]
		public TMPro.TMP_InputField InputField;
		/// <summary>
		/// 控制台输出文本
		/// </summary>
		[SerializeField]
		public TMPro.TextMeshProUGUI TextOutput;
		/// <summary>
		/// 缩略模式显示
		/// </summary>
		[SerializeField]
		public RectTransform WindowNode;
		/// <summary>
		/// 全屏显示控制台的按钮
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Button BtnConsole;
		
		private UIConsolePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FullScreenNode = null;
			InputField = null;
			TextOutput = null;
			WindowNode = null;
			BtnConsole = null;
			
			mData = null;
		}
		
		public UIConsolePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIConsolePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIConsolePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
