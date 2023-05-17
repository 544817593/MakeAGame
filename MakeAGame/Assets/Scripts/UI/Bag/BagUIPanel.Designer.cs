using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace BagUI
{
	// Generate Id:73587783-3d18-47a2-a403-cba076677e99
	public partial class BagUIPanel
	{
		public const string Name = "BagUIPanel";
		
		[SerializeField]
		public RectTransform Ref;
		[SerializeField]
		public UnityEngine.UI.Button Card;
		[SerializeField]
		public UnityEngine.UI.Button Item;
		[SerializeField]
		public UnityEngine.UI.Button Skill;
		[SerializeField]
		public UnityEngine.UI.Button Close;
		
		private BagUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Ref = null;
			Card = null;
			Item = null;
			Skill = null;
			Close = null;
			
			mData = null;
		}
		
		public BagUIPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		BagUIPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BagUIPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
