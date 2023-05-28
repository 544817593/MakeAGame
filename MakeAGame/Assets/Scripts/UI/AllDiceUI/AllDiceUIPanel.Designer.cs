using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace DiceUI
{
	// Generate Id:db733049-5b5e-4a15-80d3-ee17ad62659e
	public partial class AllDiceUIPanel
	{
		public const string Name = "AllDiceUIPanel";
		
		[SerializeField]
		public RectTransform AllDice;
		[SerializeField]
		public UnityEngine.UI.Image Dice01;
		[SerializeField]
		public UnityEngine.UI.Image Dice02;
		[SerializeField]
		public UnityEngine.UI.Image Dice03;
		[SerializeField]
		public UnityEngine.UI.Image Dice04;
		[SerializeField]
		public UnityEngine.UI.Image Dice05;
		[SerializeField]
		public UnityEngine.UI.Image Dice06;
		[SerializeField]
		public UnityEngine.UI.Button BtnRollDice;
		
		private AllDiceUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			AllDice = null;
			Dice01 = null;
			Dice02 = null;
			Dice03 = null;
			Dice04 = null;
			Dice05 = null;
			Dice06 = null;
			BtnRollDice = null;
			
			mData = null;
		}
		
		public AllDiceUIPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		AllDiceUIPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new AllDiceUIPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
