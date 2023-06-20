using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopMainUI
{
	// Generate Id:19511dfd-5dba-4553-9a87-f3cd9928600b
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
		public UnityEngine.UI.Button Enhance;
		[SerializeField]
		public UnityEngine.UI.Button Close;
		
		private ShopMainUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ShopNPC = null;
			Buy = null;
			Sell = null;
			Enhance = null;
			Close = null;
			
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
