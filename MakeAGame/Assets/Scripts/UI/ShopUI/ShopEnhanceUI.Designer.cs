using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopEnhanceUI
{
	// Generate Id:0062a9fb-bf05-4d14-8f9d-02a8c7a0be53
	public partial class ShopEnhanceUI
	{
		public const string Name = "ShopEnhanceUI";
		
		[SerializeField]
		public UnityEngine.UI.Image Background;
		[SerializeField]
		public UnityEngine.UI.Image BagPanel;
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
		public UnityEngine.UI.Button Close;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextAfterEnhance;
		[SerializeField]
		public UnityEngine.UI.Button ButtonEnhance;
		[SerializeField]
		public UnityEngine.UI.Button BtnNextPage;
		[SerializeField]
		public UnityEngine.UI.Button BtnPrePage;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextPageNum;
		
		private ShopEnhanceUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Background = null;
			BagPanel = null;
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
			Close = null;
			TextAfterEnhance = null;
			ButtonEnhance = null;
			BtnNextPage = null;
			BtnPrePage = null;
			TextPageNum = null;
			
			mData = null;
		}
		
		public ShopEnhanceUIData Data
		{
			get
			{
				return mData;
			}
		}
		
		ShopEnhanceUIData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ShopEnhanceUIData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
