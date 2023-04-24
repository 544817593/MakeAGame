using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace BagUI
{
	// Generate Id:032738e8-3852-4c43-91f9-434ce5cc988b
	public partial class BagUI
	{
		public const string Name = "BagUI";
		
		
		private BagUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public BagUIData Data
		{
			get
			{
				return mData;
			}
		}
		
		BagUIData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BagUIData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
