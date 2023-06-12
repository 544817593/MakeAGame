using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace InventoryQuickslotUI
{
	// Generate Id:1017da5f-dc0b-4f15-abdc-cc695a5bddbc
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
