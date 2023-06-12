using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace RestUI
{
	// Generate Id:3136be85-6bd5-41c0-bf70-81e26a766917
	public partial class RestUI
	{
		public const string Name = "RestUI";
		
		[SerializeField]
		public UnityEngine.UI.Button ShopNPC;
		[SerializeField]
		public UnityEngine.UI.Image Player;
		[SerializeField]
		public TMPro.TextMeshProUGUI BuffInfo;
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
		[SerializeField]
		public UnityEngine.UI.Button BtnChangeSkill;
		[SerializeField]
		public UnityEngine.UI.Button BtnLeaveRoom;
		[SerializeField]
		public RectTransform ChangeSkillPanel;
		[SerializeField]
		public UnityEngine.UI.Image CurAbilityBackground;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextCurAbility;
		[SerializeField]
		public UnityEngine.UI.Button CurAbility01;
		[SerializeField]
		public UnityEngine.UI.Button CurAbility02;
		[SerializeField]
		public UnityEngine.UI.Image UnlockedAbilityBackground;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextUnlockedAbility;
		[SerializeField]
		public UnityEngine.UI.Button UnlockedAbility01;
		[SerializeField]
		public UnityEngine.UI.Button UnlockedAbility02;
		[SerializeField]
		public UnityEngine.UI.Button UnlockedAbility03;
		[SerializeField]
		public UnityEngine.UI.Button UnlockedAbility04;
		[SerializeField]
		public UnityEngine.UI.Button UnlockedAbility05;
		[SerializeField]
		public UnityEngine.UI.Button UnlockedAbility06;
		[SerializeField]
		public UnityEngine.UI.Button CloseChangeSkillPanel;
		
		private RestUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ShopNPC = null;
			Player = null;
			BuffInfo = null;
			AllDice = null;
			Dice01 = null;
			Dice02 = null;
			Dice03 = null;
			Dice04 = null;
			Dice05 = null;
			Dice06 = null;
			BtnRollDice = null;
			BtnChangeSkill = null;
			BtnLeaveRoom = null;
			ChangeSkillPanel = null;
			CurAbilityBackground = null;
			TextCurAbility = null;
			CurAbility01 = null;
			CurAbility02 = null;
			UnlockedAbilityBackground = null;
			TextUnlockedAbility = null;
			UnlockedAbility01 = null;
			UnlockedAbility02 = null;
			UnlockedAbility03 = null;
			UnlockedAbility04 = null;
			UnlockedAbility05 = null;
			UnlockedAbility06 = null;
			CloseChangeSkillPanel = null;
			
			mData = null;
		}
		
		public RestUIData Data
		{
			get
			{
				return mData;
			}
		}
		
		RestUIData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new RestUIData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
