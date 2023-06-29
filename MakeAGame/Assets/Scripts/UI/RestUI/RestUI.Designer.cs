using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace RestUI
{
	// Generate Id:ea8aa9f8-4557-4373-9fe3-c83ee6570e37
	public partial class RestUI
	{
		public const string Name = "RestUI";
		
		[SerializeField]
		public UnityEngine.UI.Button ShopNPC;
		[SerializeField]
		public UnityEngine.UI.Image Player;
		[SerializeField]
		public TMPro.TextMeshProUGUI RollDiceTime;
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
		public UnityEngine.UI.Button BtnRules;
		[SerializeField]
		public UnityEngine.UI.Button BtnChooseProperty;
		[SerializeField]
		public TMPro.TextMeshProUGUI DiceResultSuccess;
		[SerializeField]
		public TMPro.TextMeshProUGUI DiceResultFail;
		[SerializeField]
		public Canvas RulesCanvas;
		[SerializeField]
		public Canvas ChoosePropertyCanvas;
		[SerializeField]
		public UnityEngine.UI.Button Strength;
		[SerializeField]
		public TMPro.TextMeshProUGUI StrengthText;
		[SerializeField]
		public UnityEngine.UI.Button Spirit;
		[SerializeField]
		public TMPro.TextMeshProUGUI SpiritText;
		[SerializeField]
		public UnityEngine.UI.Button Skill;
		[SerializeField]
		public TMPro.TextMeshProUGUI SkillText;
		[SerializeField]
		public UnityEngine.UI.Button Stamina;
		[SerializeField]
		public TMPro.TextMeshProUGUI StaminaText;
		[SerializeField]
		public UnityEngine.UI.Button Chrisma;
		[SerializeField]
		public TMPro.TextMeshProUGUI ChrismaText;
		[SerializeField]
		public UnityEngine.UI.Button CloseChooseProperty;
		[SerializeField]
		public RectTransform ChangeSkillPanel;
		[SerializeField]
		public UnityEngine.UI.Image CurAbilityBackground;
		[SerializeField]
		public UnityEngine.UI.Button CurAbility01;
		[SerializeField]
		public UnityEngine.UI.Button CurAbility02;
		[SerializeField]
		public UnityEngine.UI.Image UnlockedAbilityBackground;
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
		public UnityEngine.UI.Button UnlockedAbility07;
		[SerializeField]
		public UnityEngine.UI.Button UnlockedAbility08;
		[SerializeField]
		public UnityEngine.UI.Button BtnNextPage;
		[SerializeField]
		public UnityEngine.UI.Button BtnPrePage;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextPageNum;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextCurAbility;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextUnlockedAbility;
		[SerializeField]
		public UnityEngine.UI.Button CloseChangeSkillPanel;
		
		private RestUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ShopNPC = null;
			Player = null;
			RollDiceTime = null;
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
			BtnRules = null;
			BtnChooseProperty = null;
			DiceResultSuccess = null;
			DiceResultFail = null;
			RulesCanvas = null;
			ChoosePropertyCanvas = null;
			Strength = null;
			StrengthText = null;
			Spirit = null;
			SpiritText = null;
			Skill = null;
			SkillText = null;
			Stamina = null;
			StaminaText = null;
			Chrisma = null;
			ChrismaText = null;
			CloseChooseProperty = null;
			ChangeSkillPanel = null;
			CurAbilityBackground = null;
			CurAbility01 = null;
			CurAbility02 = null;
			UnlockedAbilityBackground = null;
			UnlockedAbility01 = null;
			UnlockedAbility02 = null;
			UnlockedAbility03 = null;
			UnlockedAbility04 = null;
			UnlockedAbility05 = null;
			UnlockedAbility06 = null;
			UnlockedAbility07 = null;
			UnlockedAbility08 = null;
			BtnNextPage = null;
			BtnPrePage = null;
			TextPageNum = null;
			TextCurAbility = null;
			TextUnlockedAbility = null;
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
