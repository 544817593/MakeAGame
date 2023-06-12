using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace RewardUI
{
	// Generate Id:454e5d38-ffda-452c-9e5e-33985c40f830
	public partial class RewardUIPanel
	{
		public const string Name = "RewardUIPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button legacy;
		[SerializeField]
		public UnityEngine.UI.Button Coin;
		[SerializeField]
		public TMPro.TextMeshProUGUI CoinAmount;
		[SerializeField]
		public UnityEngine.UI.Button Item;
		[SerializeField]
		public UnityEngine.UI.Button Confirm;
		
		private RewardUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			legacy = null;
			Coin = null;
			CoinAmount = null;
			Item = null;
			Confirm = null;
			
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
