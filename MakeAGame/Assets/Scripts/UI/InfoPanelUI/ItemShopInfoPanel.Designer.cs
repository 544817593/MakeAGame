using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ItemShopInfoPanel
{
	// Generate Id:c1282724-09fe-4662-8753-a1b5d6a3fcc4
	public partial class ItemShopInfoPanel
	{
		public const string Name = "ItemShopInfoPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image ItemInfoBackground;
		[SerializeField]
		public TMPro.TextMeshProUGUI ItemName;
		[SerializeField]
		public TMPro.TextMeshProUGUI ItemDescription;
		
		private ItemShopInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ItemInfoBackground = null;
			ItemName = null;
			ItemDescription = null;
			
			mData = null;
		}
		
		public ItemShopInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ItemShopInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ItemShopInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
