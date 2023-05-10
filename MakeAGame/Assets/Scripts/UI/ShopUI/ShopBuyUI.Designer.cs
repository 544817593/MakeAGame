using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopBuyUI
{
	// Generate Id:4d376610-38f2-4b60-9652-a08664c47ae7
	public partial class ShopBuyUI
	{
		public const string Name = "ShopBuyUI";
		
		[SerializeField]
		public UnityEngine.UI.Button Close;
		[SerializeField]
		public UnityEngine.UI.Button Button;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextCoin;
		
		private ShopBuyUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Close = null;
			Button = null;
			TextCoin = null;
			
			mData = null;
		}
		
		public ShopBuyUIData Data
		{
			get
			{
				return mData;
			}
		}
		
		ShopBuyUIData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ShopBuyUIData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
