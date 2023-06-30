using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using Skill_Info;

namespace InventoryQuickslotUI
{
	public class UIAbilityPanelData : UIPanelData
	{
        
    }
	public partial class UIAbilityPanel : UIPanel
	{
        [SerializeField] private List<Sprite> skillSprites; // 技能栏技能图片
        ISkillSystem skillSystem;

        private void Awake()
        {
            skillSystem = GameEntry.Interface.GetSystem<ISkillSystem>();

            Skill1.onClick.AddListener(() => 
            {
                if (skillSystem.GetEquippedSkillsList()[0] != SkillNameEnum.None)
                {
                    skillSystem.CastSkill(true);
                }
            });
            
            Skill2.onClick.AddListener(() =>
            { 
                if (skillSystem.GetEquippedSkillsList()[1] != SkillNameEnum.None)
                {
                    skillSystem.CastSkill(false);
                }
            });


        }

        private void Update()
		{
            // 技能快捷键
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (skillSystem.GetEquippedSkillsList()[0] != SkillNameEnum.None)
                {
                    Debug.LogError("Cast1");
                    skillSystem.CastSkill(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (skillSystem.GetEquippedSkillsList()[1] != SkillNameEnum.None)
                {
                    skillSystem.CastSkill(false);
                }

            }
        }

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIAbilityPanelData ?? new UIAbilityPanelData();
            // please add init code here
            if (skillSystem.GetEquippedSkillsList()[0] != SkillNameEnum.None)
            {
                UIEventHelper mouseHelper1 = Skill1.AddComponent<UIEventHelper>(); // 初始化的时候add
                mouseHelper1.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[0]);
                mouseHelper1.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[0]);
            }
            if (skillSystem.GetEquippedSkillsList()[1] != SkillNameEnum.None)
            {
                UIEventHelper mouseHelper2 = Skill2.AddComponent<UIEventHelper>();
                mouseHelper2.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[1]);
                mouseHelper2.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[1]);
            }
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
            if (skillSystem.GetEquippedSkillsList()[0] != SkillNameEnum.None)
            {
                UIEventHelper mouseHelper1 = Skill1.GetComponent<UIEventHelper>(); // 这里get
                mouseHelper1.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[0]);
                mouseHelper1.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[0]);
            }
            if (skillSystem.GetEquippedSkillsList()[1] != SkillNameEnum.None)
            {
                UIEventHelper mouseHelper2 = Skill2.GetComponent<UIEventHelper>();
                mouseHelper2.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[1]);
                mouseHelper2.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[1]);
            }
        }
		
		protected override void OnShow()
		{
            if (skillSystem.GetEquippedSkillsList()[0] != SkillNameEnum.None)
            {
                UIEventHelper mouseHelper1 = Skill1.GetComponent<UIEventHelper>(); // 这里get
                mouseHelper1.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[0]);
                mouseHelper1.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[0]);
            }
            if (skillSystem.GetEquippedSkillsList()[1] != SkillNameEnum.None)
            {
                UIEventHelper mouseHelper2 = Skill2.GetComponent<UIEventHelper>();
                mouseHelper2.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[1]);
                mouseHelper2.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[1]);
            }
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

        private void MouseEnter(SkillNameEnum skill)
        {
            if (skillSystem.GetEquippedSkillsList().Count != 0)
            {
                Debug.Log($" SkillNameEnum {skill} mouseHelper MouseEnter");
                UIKit.OpenPanel<Skill_InfoPanel>();
                UIKit.GetPanel<Skill_InfoPanel>().LoadSkillData(skill);
            }
        }

        private void MouseExit(SkillNameEnum skill)
        {
            Debug.Log($" SkillNameEnum {skill} mouseHelper MouseExit");
            UIKit.ClosePanel<Skill_InfoPanel>();
        }
    }
}
