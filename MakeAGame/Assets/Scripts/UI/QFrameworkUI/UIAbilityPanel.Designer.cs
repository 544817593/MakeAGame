using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace InventoryQuickslotUI
{
	// Generate Id:034c6c16-1567-4720-8aaf-60558d4a891e
	public partial class UIAbilityPanel
	{
		public const string Name = "UIAbilityPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button Skill1;
		[SerializeField]
		public UnityEngine.UI.Button Skill2;
		[SerializeField]
		public UnityEngine.UI.Image AbilityBarPanel;
		
		private UIAbilityPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Skill1 = null;
			Skill2 = null;
			AbilityBarPanel = null;
			
			mData = null;
		}
		
		public UIAbilityPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIAbilityPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIAbilityPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
