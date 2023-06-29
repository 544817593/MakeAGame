using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;

namespace Skill_Info
{
	public class Skill_InfoPanelData : UIPanelData
	{
	}
	public partial class Skill_InfoPanel : UIPanel, ICanGetSystem
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as Skill_InfoPanelData ?? new Skill_InfoPanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
		
		// TODO：需要技能的描述
		public void LoadSkillData(SkillNameEnum skillName)
		{
			SkillName.text = SkillSystem.skillChineseNameDict[skillName];
			SkillDescription.text = SkillSystem.skillDescriptionDict[skillName];
        }

        public IArchitecture GetArchitecture()
        {
			return GameEntry.Interface;
        }
    }
}
