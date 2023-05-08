using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Game;

namespace InventoryQuickslotUI
{
	public class UIAbilityPanelData : UIPanelData
	{
	}
	public partial class UIAbilityPanel : UIPanel
	{
        [SerializeField] private List<Sprite> skillSprites; // 技能图片
        private bool isSkill1Active = false;
        private bool isSkill2Active = false;
        private int selectedButton = -1; // Keyboard shortcuts, 0 = Q; 1 = E
        ISkillSystem skillSystem;

        private void Awake()
        {
            skillSystem = GameEntry.Interface.GetSystem<ISkillSystem>();
            //playerSkills = GameManager.Instance.playerMan.GetPlayerSkillTree();
            //playerSkills.OnEquippedSkillsChange += PlayerSkills_OnEquippedSkillsChange;
            Skill1.image.color = new Color(255, 255, 255, 0);
            Skill2.image.color = new Color(255, 255, 255, 0);

            Skill1.onClick.AddListener(() => skillSystem.CastSkill(true));
            Skill2.onClick.AddListener(() => skillSystem.CastSkill(false));
        }

        private void Update()
		{
            // 技能快捷键
            if (Input.GetKeyDown(KeyCode.Q))
            {
                skillSystem.CastSkill(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (skillSystem.GetEquippedSkillsList().Count > 1)
                {
                    skillSystem.CastSkill(false);
                }

            }
        }

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIAbilityPanelData ?? new UIAbilityPanelData();
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

    }
}
