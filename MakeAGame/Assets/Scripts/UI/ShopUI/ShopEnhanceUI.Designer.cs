using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ShopEnhanceUI
{
	// Generate Id:f45e594f-af80-4674-8132-227975a72917
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
		public RectTransform CardBeforeEnhance;
		[SerializeField]
		public RectTransform EnhanceItem;
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
		
		private ShopEnhanceUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Background = null;
			Close = null;
			BagPanel = null;
			CardBeforeEnhance = null;
			EnhanceItem = null;
			CardAfterEnhance = null;
			TextAfterEnhance = null;
			ButtonEnhance = null;
			ButtonClear = null;
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
