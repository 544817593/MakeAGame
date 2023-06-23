using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace CardRewardUI
{
	// Generate Id:8b829aeb-c335-440d-aed6-786bfd079fc1
	public partial class UICardRewardPanel
	{
		public const string Name = "UICardRewardPanel";
		
		
		private UICardRewardPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UICardRewardPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UICardRewardPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UICardRewardPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
