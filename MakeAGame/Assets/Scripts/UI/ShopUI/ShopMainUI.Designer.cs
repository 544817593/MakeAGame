using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopMainUI
{
	// Generate Id:745c2075-5452-4925-9fbb-3b705bc6821a
	public partial class ShopMainUI
	{
		public const string Name = "ShopMainUI";
		
		[SerializeField]
		public UnityEngine.UI.Button ShopNPC;
		[SerializeField]
		public UnityEngine.UI.Button Buy;
		[SerializeField]
		public UnityEngine.UI.Button Sell;
		[SerializeField]
		public UnityEngine.UI.Button LevelUp;
		
		private ShopMainUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ShopNPC = null;
			Buy = null;
			Sell = null;
			LevelUp = null;
			
			mData = null;
		}
		
		public ShopMainUIData Data
		{
			get
			{
				return mData;
			}
		}
		
		ShopMainUIData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ShopMainUIData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
