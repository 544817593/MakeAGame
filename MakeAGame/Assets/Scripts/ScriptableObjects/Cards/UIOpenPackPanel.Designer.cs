using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PackOpen
{
	// Generate Id:91937b24-e602-4efd-a4a0-b5a80c185dd1
	public partial class UIOpenPackPanel
	{
		public const string Name = "UIOpenPackPanel";
		
		[SerializeField]
		public Pack Pack;
		[SerializeField]
		public RectTransform CardPosition;
		[SerializeField]
		public UnityEngine.UI.Image CardPosition1;
		[SerializeField]
		public UnityEngine.UI.Image CardPosition2;
		[SerializeField]
		public UnityEngine.UI.Image CardPosition3;
		[SerializeField]
		public UnityEngine.UI.Image CardPosition4;
		[SerializeField]
		public UnityEngine.UI.Image CardPosition5;
		
		private UIOpenPackPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Pack = null;
			CardPosition = null;
			CardPosition1 = null;
			CardPosition2 = null;
			CardPosition3 = null;
			CardPosition4 = null;
			CardPosition5 = null;
			
			mData = null;
		}
		
		public UIOpenPackPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIOpenPackPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIOpenPackPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
