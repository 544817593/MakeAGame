using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PauseUI
{
	// Generate Id:54d84bcb-d0ca-4af4-b604-225fcf226b99
	public partial class Pause
	{
		public const string Name = "Pause";
		
		[SerializeField]
		public UnityEngine.UI.Button PauseButton;
		[SerializeField]
		public UnityEngine.UI.Image PauseMenu;
		[SerializeField]
		public UnityEngine.UI.Button BackButton;
		[SerializeField]
		public UnityEngine.UI.Button ExitButton;
		
		private PauseData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PauseButton = null;
			PauseMenu = null;
			BackButton = null;
			ExitButton = null;
			
			mData = null;
		}
		
		public PauseData Data
		{
			get
			{
				return mData;
			}
		}
		
		PauseData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PauseData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
