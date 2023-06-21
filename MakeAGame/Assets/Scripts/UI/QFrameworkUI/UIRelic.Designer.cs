using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:fc48ec36-9b7f-4ef3-9fa2-1be3163e622e
	public partial class UIRelic
	{
		public const string Name = "UIRelic";
		
		[SerializeField]
		public UnityEngine.UI.Image ContentPanel;
		[SerializeField]
		public UnityEngine.UI.Image Tooltip;
		
		private UIRelicData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ContentPanel = null;
			Tooltip = null;
			
			mData = null;
		}
		
		public UIRelicData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIRelicData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIRelicData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
