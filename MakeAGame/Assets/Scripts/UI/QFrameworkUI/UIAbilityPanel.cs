using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using Skill_Info;
using static UnityEditor.Progress;
using UnityEditor.Experimental.GraphView;

namespace InventoryQuickslotUI
{
	public class UIAbilityPanelData : UIPanelData
	{
	}
	public partial class UIAbilityPanel : UIPanel
	{
        [SerializeField] private List<Sprite> skillSprites; // 技能栏技能图片
        private bool isSkill1Active = false;
        private bool isSkill2Active = false;
        private int selectedButton = -1; // Keyboard shortcuts, 0 = Q; 1 = E
        ISkillSystem skillSystem;

        private void Awake()
        {
            skillSystem = GameEntry.Interface.GetSystem<ISkillSystem>();
            //playerSkills = GameManager.Instance.playerMan.GetPlayerSkillTree();
            //playerSkills.OnEquippedSkillsChange += PlayerSkills_OnEquippedSkillsChange;
            //Skill1.image.color = new Color(0, 0, 0, 0);
            //Skill2.image.color = new Color(0, 0, 0, 0);

            Skill1.onClick.AddListener(() => 
            {
                if (skillSystem.GetEquippedSkillsList().Count == 1)
                {
                    skillSystem.CastSkill(true);
                }
            });
            
            Skill2.onClick.AddListener(() =>
            { 
                if (skillSystem.GetEquippedSkillsList().Count == 2)
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
                if (skillSystem.GetEquippedSkillsList().Count == 1)
                {
                    skillSystem.CastSkill(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (skillSystem.GetEquippedSkillsList().Count == 2)
                {
                    skillSystem.CastSkill(false);
                }

            }
        }

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIAbilityPanelData ?? new UIAbilityPanelData();
            // please add init code here
            if (skillSystem.GetEquippedSkillsList().Count == 1)
            {
                UIEventHelper mouseHelper1 = Skill1.AddComponent<UIEventHelper>(); // 初始化的时候add
                mouseHelper1.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[0]);
                mouseHelper1.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[0]);
            }
            if (skillSystem.GetEquippedSkillsList().Count == 2)
            {
                UIEventHelper mouseHelper2 = Skill2.AddComponent<UIEventHelper>();
                mouseHelper2.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[1]);
                mouseHelper2.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[1]);
            }
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
            if (skillSystem.GetEquippedSkillsList().Count == 1)
            {
                UIEventHelper mouseHelper1 = Skill1.GetComponent<UIEventHelper>(); // 这里get
                mouseHelper1.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[0]);
                mouseHelper1.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[0]);
            }
            if (skillSystem.GetEquippedSkillsList().Count == 2)
            {
                UIEventHelper mouseHelper2 = Skill2.GetComponent<UIEventHelper>();
                mouseHelper2.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[1]);
                mouseHelper2.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[1]);
            }
        }
		
		protected override void OnShow()
		{
            if (skillSystem.GetEquippedSkillsList().Count == 1)
            {
                UIEventHelper mouseHelper1 = Skill1.GetComponent<UIEventHelper>(); // 这里get
                mouseHelper1.OnUIPointEnter = () => MouseEnter(skillSystem.GetEquippedSkillsList()[0]);
                mouseHelper1.OnUIPointExit = () => MouseExit(skillSystem.GetEquippedSkillsList()[0]);
            }
            if (skillSystem.GetEquippedSkillsList().Count == 2)
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
