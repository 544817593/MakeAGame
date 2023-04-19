using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:845fa3bd-f435-4278-823d-5a2f2eca27cf
	public partial class UIHandCard
	{
		public const string Name = "UIHandCard";
		
		
		private UIHandCardData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIHandCardData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHandCardData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHandCardData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
