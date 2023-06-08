using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PackOpen
{
	// Generate Id:fe6ca777-b21e-4b88-82bd-9f683a10d381
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
		[SerializeField]
		public RectTransform Point;
		[SerializeField]
		public RectTransform Point1;
		[SerializeField]
		public RectTransform Point2;
		[SerializeField]
		public RectTransform Point3;
		[SerializeField]
		public RectTransform Point4;
		[SerializeField]
		public RectTransform Point5;
		
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
			Point = null;
			Point1 = null;
			Point2 = null;
			Point3 = null;
			Point4 = null;
			Point5 = null;
			
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
