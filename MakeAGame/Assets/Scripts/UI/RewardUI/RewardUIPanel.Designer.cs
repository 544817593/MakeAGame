using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace RewardUI
{
	// Generate Id:750a317d-257a-4bf6-8057-9d72b5bbf62c
	public partial class RewardUIPanel
	{
		public const string Name = "RewardUIPanel";
		
		
		private RewardUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public RewardUIPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		RewardUIPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new RewardUIPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
