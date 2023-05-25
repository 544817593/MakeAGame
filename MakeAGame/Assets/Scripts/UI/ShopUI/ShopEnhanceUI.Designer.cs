using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopEnhanceUI
{
	// Generate Id:7ccb6276-2b05-42b5-8020-e7037a8eaa03
	public partial class ShopEnhanceUI
	{
		public const string Name = "ShopEnhanceUI";
		
		[SerializeField]
		public UnityEngine.UI.Image Background;
		[SerializeField]
		public UnityEngine.UI.Button Close;
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
		public RectTransform CardBeforeEnhance;
		[SerializeField]
		public UnityEngine.UI.Button EnhanceItem;
		[SerializeField]
		public RectTransform CardAfterEnhance;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextAfterEnhance;
		[SerializeField]
		public UnityEngine.UI.Button ButtonEnhance;
		[SerializeField]
		public UnityEngine.UI.Button ButtonClear;
		[SerializeField]
		public UnityEngine.UI.Button BtnNextPage;
		[SerializeField]
		public UnityEngine.UI.Button BtnPrePage;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextPageNum;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem;
		[SerializeField]
		public UnityEngine.UI.Button BtnCard;
		
		private ShopEnhanceUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Background = null;
			Close = null;
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
			CardBeforeEnhance = null;
			EnhanceItem = null;
			CardAfterEnhance = null;
			TextAfterEnhance = null;
			ButtonEnhance = null;
			ButtonClear = null;
			BtnNextPage = null;
			BtnPrePage = null;
			TextPageNum = null;
			BtnItem = null;
			BtnCard = null;
			
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
