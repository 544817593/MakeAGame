using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace InventoryQuickslotUI
{
	// Generate Id:43142805-471d-4f6c-bc7d-0c6274f30031
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
