using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopBuyUI
{
	// Generate Id:78f92ef1-c916-474f-a4b4-f5a3ebe92c50
	public partial class ShopBuyUI
	{
		public const string Name = "ShopBuyUI";
		
		[SerializeField]
		public UnityEngine.UI.Button Close;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextGold;
		[SerializeField]
		public UnityEngine.UI.Button Buy;
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
		public UnityEngine.UI.Button Item11;
		[SerializeField]
		public UnityEngine.UI.Button Item12;
		[SerializeField]
		public UnityEngine.UI.Button Item13;
		[SerializeField]
		public UnityEngine.UI.Button Item14;
		[SerializeField]
		public UnityEngine.UI.Button Item15;
		[SerializeField]
		public UnityEngine.UI.Button Item16;
		[SerializeField]
		public UnityEngine.UI.Button Item17;
		[SerializeField]
		public UnityEngine.UI.Button Item18;
		[SerializeField]
		public UnityEngine.UI.Button Item19;
		[SerializeField]
		public UnityEngine.UI.Button Item20;
		[SerializeField]
		public UnityEngine.UI.Image ImageItemInfo;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextItemInfo;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextCount;
		[SerializeField]
		public UnityEngine.UI.Button BtnAdd;
		[SerializeField]
		public UnityEngine.UI.Button BtnSub;
		
		private ShopBuyUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Close = null;
			TextGold = null;
			Buy = null;
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
			Item11 = null;
			Item12 = null;
			Item13 = null;
			Item14 = null;
			Item15 = null;
			Item16 = null;
			Item17 = null;
			Item18 = null;
			Item19 = null;
			Item20 = null;
			ImageItemInfo = null;
			TextItemInfo = null;
			TextCount = null;
			BtnAdd = null;
			BtnSub = null;
			
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
