using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace InventoryQuickslotUI
{
	// Generate Id:486f6f12-45fa-4c8b-9bfe-9f63f2ad2281
	public partial class UIInventoryQuickSlot
	{
		public const string Name = "UIInventoryQuickSlot";
		
		
		private UIInventoryQuickSlotData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIInventoryQuickSlotData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIInventoryQuickSlotData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIInventoryQuickSlotData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
