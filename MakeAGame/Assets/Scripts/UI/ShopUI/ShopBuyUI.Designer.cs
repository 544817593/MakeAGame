using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopBuyUI
{
	// Generate Id:13bc342c-98a1-453f-b288-6c2fb9281a16
	public partial class ShopBuyUI
	{
		public const string Name = "ShopBuyUI";
		
		[SerializeField]
		public UnityEngine.UI.Button Close;
		[SerializeField]
		public UnityEngine.UI.Button Buy;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextCoin;
		
		private ShopBuyUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Close = null;
			Buy = null;
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
