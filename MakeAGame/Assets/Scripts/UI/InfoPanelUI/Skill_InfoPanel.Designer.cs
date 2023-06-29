using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Skill_Info
{
	// Generate Id:d265b5e8-4788-46c1-a6c2-17c928eaf897
	public partial class Skill_InfoPanel
	{
		public const string Name = "Skill_InfoPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image Skill_InfoBackground;
		[SerializeField]
		public TMPro.TextMeshProUGUI SkillName;
		[SerializeField]
		public TMPro.TextMeshProUGUI SkillDescription;
		
		private Skill_InfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Skill_InfoBackground = null;
			SkillName = null;
			SkillDescription = null;
			
			mData = null;
		}
		
		public Skill_InfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		Skill_InfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new Skill_InfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
