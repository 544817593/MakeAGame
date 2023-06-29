using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:7d258bb3-85bd-42da-a7b5-113ec3b98e39
	public partial class UIExplore
	{
		public const string Name = "UIExplore";
		
		
		private UIExploreData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIExploreData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIExploreData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIExploreData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
