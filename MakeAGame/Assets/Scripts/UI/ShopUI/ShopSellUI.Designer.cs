using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopSellUI
{
	// Generate Id:0bc101f2-0209-4ad8-aed9-f16759475d1b
	public partial class ShopSellUI
	{
		public const string Name = "ShopSellUI";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TextGold;
		[SerializeField]
		public UnityEngine.UI.Button Close;
		[SerializeField]
		public UnityEngine.UI.Image ShopGridPanel;
		[SerializeField]
		public UnityEngine.UI.Button Item01;
		[SerializeField]
		public UnityEngine.UI.Button Item02;
		[SerializeField]
		public UnityEngine.UI.Button Item03;
		[SerializeField]
		public UnityEngine.UI.Button Item04;
		[SerializeField]
		public UnityEngine.UI.Button Item05;
		[SerializeField]
		public UnityEngine.UI.Button Item06;
		[SerializeField]
		public UnityEngine.UI.Button Item07;
		[SerializeField]
		public UnityEngine.UI.Button Item08;
		[SerializeField]
		public UnityEngine.UI.Button Item09;
		[SerializeField]
		public UnityEngine.UI.Button Item10;
		[SerializeField]
		public UnityEngine.UI.Button Sell;
		[SerializeField]
		public UnityEngine.UI.Image ImageItemInfo;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextItemInfo;
		[SerializeField]
		public UnityEngine.UI.Button BtnNextPage;
		[SerializeField]
		public UnityEngine.UI.Button BtnPrePage;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextPageNum;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextCount;
		[SerializeField]
		public UnityEngine.UI.Button BtnAdd;
		[SerializeField]
		public UnityEngine.UI.Button BtnSub;
		
		private ShopSellUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TextGold = null;
			Close = null;
			ShopGridPanel = null;
			Item01 = null;
			Item02 = null;
			Item03 = null;
			Item04 = null;
			Item05 = null;
			Item06 = null;
			Item07 = null;
			Item08 = null;
			Item09 = null;
			Item10 = null;
			Sell = null;
			ImageItemInfo = null;
			TextItemInfo = null;
			BtnNextPage = null;
			BtnPrePage = null;
			TextPageNum = null;
			TextCount = null;
			BtnAdd = null;
			BtnSub = null;
			
			mData = null;
		}
		
		public ShopSellUIData Data
		{
			get
			{
				return mData;
			}
		}
		
		ShopSellUIData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ShopSellUIData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
