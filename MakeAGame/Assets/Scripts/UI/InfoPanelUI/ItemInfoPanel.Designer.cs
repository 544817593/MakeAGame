using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ItemInfo
{
	// Generate Id:f58cc203-82f6-4ee1-b756-e744c1c9b1c7
	public partial class ItemInfoPanel
	{
		public const string Name = "ItemInfoPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image ItemInfoBackground;
		[SerializeField]
		public TMPro.TextMeshProUGUI ItemName;
		[SerializeField]
		public TMPro.TextMeshProUGUI ItemDescription;
		
		private ItemInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ItemInfoBackground = null;
			ItemName = null;
			ItemDescription = null;
			
			mData = null;
		}
		
		public ItemInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ItemInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ItemInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
