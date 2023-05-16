using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace InventoryQuickslotUI
{
	// Generate Id:c84c6d2d-d246-423d-8715-aff5c7ae84de
	public partial class UIInventoryQuickSlot
	{
		public const string Name = "UIInventoryQuickSlot";
		
		[SerializeField]
		public UnityEngine.UI.Button CombatSceneButton;
		
		private UIInventoryQuickSlotData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			CombatSceneButton = null;
			
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
