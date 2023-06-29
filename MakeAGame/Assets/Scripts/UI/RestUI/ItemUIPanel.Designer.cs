using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ItemUI
{
	// Generate Id:e12de080-f493-445d-b78c-0a2382c19e9b
	public partial class ItemUIPanel
	{
		public const string Name = "ItemUIPanel";
		
		[SerializeField]
		public RectTransform Ref;
		[SerializeField]
		public UnityEngine.UI.Button Card;
		[SerializeField]
		public UnityEngine.UI.Button Item;
		[SerializeField]
		public UnityEngine.UI.Button Close;
		[SerializeField]
		public RectTransform SlotPosition;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition0;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition1;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition2;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition3;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition4;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition5;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition6;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition7;
		[SerializeField]
		public UnityEngine.UI.Image ItemPosition8;
		[SerializeField]
		public UnityEngine.UI.Image ItemPositio10;
		[SerializeField]
		public UnityEngine.UI.Button BtnNextPage;
		[SerializeField]
		public UnityEngine.UI.Button BtnPrePage;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextPageNum;
		[SerializeField]
		public UnityEngine.UI.Image TextPanel;
		[SerializeField]
		public TMPro.TextMeshProUGUI DescriptionT;
		
		private ItemUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Ref = null;
			Card = null;
			Item = null;
			Close = null;
			SlotPosition = null;
			ItemPosition0 = null;
			ItemPosition1 = null;
			ItemPosition2 = null;
			ItemPosition3 = null;
			ItemPosition4 = null;
			ItemPosition5 = null;
			ItemPosition6 = null;
			ItemPosition7 = null;
			ItemPosition8 = null;
			ItemPositio10 = null;
			BtnNextPage = null;
			BtnPrePage = null;
			TextPageNum = null;
			TextPanel = null;
			DescriptionT = null;
			
			mData = null;
		}
		
		public ItemUIPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ItemUIPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ItemUIPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
