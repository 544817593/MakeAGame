using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace CardRewardUI
{
	// Generate Id:01a1dfea-ffd0-4ffa-83ea-de8931b7da3c
	public partial class UICardRewardPanel
	{
		public const string Name = "UICardRewardPanel";
		
		[SerializeField]
		public RectTransform Card;
		[SerializeField]
		public UnityEngine.UI.Button Confirm;
		
		private UICardRewardPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Card = null;
			Confirm = null;
			
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
