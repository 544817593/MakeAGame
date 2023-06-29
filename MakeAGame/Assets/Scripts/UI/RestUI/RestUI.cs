using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using System.Runtime.ConstrainedExecution;

namespace RestUI
{
	public class RestUIData : UIPanelData
	{
        public ISkillSystem skillSystem = GameEntry.Interface.GetSystem<ISkillSystem>();
    }
	public partial class RestUI : UIPanel
	{
        // 已解锁的技能列表
        List<SkillNameEnum> UnlockedSkillsList = new List<SkillNameEnum>();
        // 已装备的技能列表
        List<SkillNameEnum> EquippedSkillsList = new List<SkillNameEnum>(2);

        Dictionary<Button,int> unlockSkillToIdx = new Dictionary<Button,int>();

        // 背包每页的格子数量上限
        private const int gridNum = 8;
        private int curPage = 1;
        private int totalPage = 1;
        // 每页显示的元素索引区间[lowerIndex, upperIndex]
        private int upperIndex = gridNum - 1;
        private int lowerIndex = 0;
        private int rollDicetime = 1;
        private string choosePropertyString = "无";
        private PlayerStatsEnum chooseProperty = PlayerStatsEnum.None;
        private int diceSum = 0;
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as RestUIData ?? new RestUIData();
            // please add init code here

            //测试数据用
            //foreach(SkillNameEnum skillName in SkillSystem.skillIDNameDic.Keys)
            //{
            //    if(skillName != SkillNameEnum.None)
            //    {
            //        mData.skillSystem.UnlockSkill(skillName);
            //    }
            //}
            //mData.skillSystem.AddEquippedSkills(SkillNameEnum.None);
            //mData.skillSystem.AddEquippedSkills(SkillNameEnum.None);

            rollDicetime = 1;
            UnlockedSkillsList = mData.skillSystem.GetUnlockedSkills();
            EquippedSkillsList = mData.skillSystem.GetEquippedSkillsList();
            
            InitView();
            AllButtonListen();
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

        private void InitView()
        {
            RollDiceTime.text = $"当前投掷机会：{rollDicetime}";
            BuffInfo.text = $"当前选择属性：{choosePropertyString}";
            ChangeSkillPanel.gameObject.SetActive(false);
            RulesCanvas.gameObject.SetActive(false);
            ChoosePropertyCanvas.gameObject.SetActive(false);
            DiceResultSuccess.gameObject.SetActive(false);
            DiceResultFail.gameObject.SetActive(false);
            RefreshStats();
        }

        private void RefreshStats()
        {
            StrengthText.text = $"力量：{GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Strength)}";
            SpiritText.text = $"精神：{GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Spirit)}";
            SkillText.text = $"技巧：{GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Skill)}";
            StaminaText.text = $"体力：{GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Stamina)}";
            ChrismaText.text = $"魅力：{GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Charisma)}";
        }
        /// <summary>
        /// 全部按钮监听
        /// </summary>
		private void AllButtonListen()
		{
            PageChange();
			// 摇骰子按钮监听，执行摇骰子逻辑，替换显示的骰子图
            BtnRollDice.onClick.AddListener(() =>
            {
                if(rollDicetime > 0 && chooseProperty != PlayerStatsEnum.None)
                {
                    List<int> randNumList = new List<int>();
                    // 摇6个骰子
                    for (int i = 1; i <= 6; ++i)
                    {
                        int randNum = RollDiceFunc(0, 3);
                        randNumList.Add(randNum);
                        diceSum += randNum;
                    }
                    int idx = 0;
                    foreach (Transform transform in AllDice.GetComponentInChildren<Transform>())
                    {
                        //Debug.Log(transform.gameObject.name);
                        Assert.IsTrue(idx < randNumList.Count);
                        transform.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Rest/{randNumList[idx]}");
                        idx++;
                    }
                    rollDicetime--;
                    RollDiceTime.text = $"当前投掷机会：{rollDicetime}";
                    // 判定成功 选择的属性+1
                    if(diceSum <= GameManager.Instance.playerMan.player.GetStats(chooseProperty))
                    {
                        GameManager.Instance.playerMan.player.ModifyStats(chooseProperty, 1);
                        DiceResultSuccess.gameObject.SetActive(true);
                        DiceResultFail.gameObject.SetActive(false);
                        RefreshStats();
                    }
                    else // 判定失败
                    {
                        DiceResultSuccess.gameObject.SetActive(false);
                        DiceResultFail.gameObject.SetActive(true);
                    }
                    
                }
            });
			// 点击切换技能按钮，设置为active
            BtnChangeSkill.onClick.AddListener(() =>
			{
				ChangeSkillPanel.gameObject.SetActive(true);
                RefreshSkillPanelLayout();
            });
            // 点击关闭切换技能页面，设置为inactive
            CloseChangeSkillPanel.onClick.AddListener(() =>
            {
                ChangeSkillPanel.gameObject.SetActive(false);
            });
            // 如果1号位装备了技能，点击后卸下当前技能
            CurAbility01.onClick.AddListener(() =>
            {
                if (CurAbility01.GetComponent<Image>().sprite != Resources.Load<Sprite>("Sprites/Abilities/技能-圆盘"))
                {
                    CurAbility01.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Abilities/技能-圆盘");
                    EquippedSkillsList[0] = SkillNameEnum.None;
                }
                else
                {
                    Debug.Log("当前位置没有装备技能");
                }
            });
            // 如果2号位装备了技能，点击后卸下当前技能
            CurAbility02.onClick.AddListener(() =>
            {
                if (CurAbility02.GetComponent<Image>().sprite != Resources.Load<Sprite>("Sprites/Abilities/技能-圆盘"))
                {
                    CurAbility02.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Abilities/技能-圆盘");
                    EquippedSkillsList[1] = SkillNameEnum.None;
                }
                else
                {
                    Debug.Log("当前位置没有装备技能");
                }
            });
            // 点击离开房间，关闭当前panel
            BtnLeaveRoom.onClick.AddListener(() =>
            {
                CloseSelf();
                GameObject.Find("GameSceneManager")?.transform.GetComponent<Game.SceneFlow>().LoadRoom();
            });
            // 规则介绍按钮
            BtnRules.onClick.AddListener(() =>
            {
                if (!RulesCanvas.gameObject.activeInHierarchy)
                {
                    RulesCanvas.gameObject.SetActive(true);
                }
                else
                {
                    RulesCanvas.gameObject.SetActive(false);
                }
            });
            // 选择属性按钮
            BtnChooseProperty.onClick.AddListener(() => 
            {
                if (!ChoosePropertyCanvas.gameObject.activeInHierarchy)
                {
                    ChoosePropertyCanvas.gameObject.SetActive(true);
                }
                else
                {
                    ChoosePropertyCanvas.gameObject.SetActive(false);
                }
            });
            // 关闭选择属性canvas
            CloseChooseProperty.onClick.AddListener(() =>
            {
                ChoosePropertyCanvas.gameObject.SetActive(false);
            });
            Strength.onClick.AddListener(() =>
            {
                chooseProperty = PlayerStatsEnum.Strength;
                choosePropertyString = "力量";
                BuffInfo.text = $"当前选择属性：{choosePropertyString}";
            });
            Spirit.onClick.AddListener(() =>
            {
                chooseProperty = PlayerStatsEnum.Spirit;
                choosePropertyString = "精神";
                BuffInfo.text = $"当前选择属性：{choosePropertyString}";
            });
            Skill.onClick.AddListener(() =>
            {
                chooseProperty = PlayerStatsEnum.Spirit;
                choosePropertyString = "技巧";
                BuffInfo.text = $"当前选择属性：{choosePropertyString}";
            });
            Stamina.onClick.AddListener(() =>
            {
                chooseProperty = PlayerStatsEnum.Spirit;
                choosePropertyString = "体力";
                BuffInfo.text = $"当前选择属性：{choosePropertyString}";
            });
            Chrisma.onClick.AddListener(() =>
            {
                chooseProperty = PlayerStatsEnum.Spirit;
                choosePropertyString = "魅力";
                BuffInfo.text = $"当前选择属性：{choosePropertyString}";
            });
        }

        /// <summary>
        /// 切换技能面板的初始化和刷新
        /// </summary>
        private void RefreshSkillPanelLayout()
        {
            totalPage = UnlockedSkillsList.Count != 0 ? (int)Math.Ceiling((double)UnlockedSkillsList.Count / gridNum) : 1;
            TextPageNum.text = $" {curPage} / {totalPage}";
            int idx = lowerIndex;
            foreach (Transform transform in UnlockedAbilityBackground.GetComponentInChildren<Transform>())
            {
                GameObject curObj = transform.gameObject;
                // 读取已解锁技能列表，超过idx上限的按钮设置为inactive
                if (idx <= upperIndex && idx < UnlockedSkillsList.Count)
                {
                    curObj.SetActive(true);
                    // 替换技能图标
                    transform.GetComponent<Image>().sprite = mData.skillSystem.GetSkillIconSprite(UnlockedSkillsList[idx]);
                    Button curbtn = transform.GetComponent<Button>();
                    unlockSkillToIdx[curbtn] = idx;
                    //Debug.Log(btnIdx);
                    //监听已解锁的技能，点击按钮后如果技能1空闲放到技能1，如果技能2空闲放到技能2，都不空闲则提示
                    curbtn.onClick.AddListener(() =>
                    {
                        Debug.Log(mData.skillSystem.GetEquippedSkillsList().Count);
                        //Debug.Log(UnlockedSkillsList.Count);
                        //Debug.Log(btnIdx);
                        if (EquippedSkillsList.Count >= 1 && EquippedSkillsList[0] == UnlockedSkillsList[unlockSkillToIdx[curbtn]])
                        {
                            Debug.Log("该技能已装备");
                        }
                        else if (EquippedSkillsList.Count >= 2 && EquippedSkillsList[1] == UnlockedSkillsList[unlockSkillToIdx[curbtn]])
                        {
                            Debug.Log("该技能已装备");
                        }
                        else if (CurAbility01.GetComponent<Image>().sprite == null)
                        {
                            mData.skillSystem.ChangeEquippedSkillsList(0, UnlockedSkillsList[unlockSkillToIdx[curbtn]]);
                            CurAbility01.GetComponent<Image>().sprite = curbtn.GetComponent<Image>().sprite;
                        }
                        else if (CurAbility02.GetComponent<Image>().sprite == null)
                        {
                            mData.skillSystem.ChangeEquippedSkillsList(1, UnlockedSkillsList[unlockSkillToIdx[curbtn]]);
                            CurAbility02.GetComponent<Image>().sprite = curbtn.GetComponent<Image>().sprite;
                        }
                        else
                        {
                            Debug.Log("先卸下一个技能才能装备该技能");
                        }
                    });
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
                idx++;
            }
        }
        /// <summary>
        /// 更新当前已解锁技能页的index区间
        /// </summary>
        private void UpdateIndex()
        {
            int bagItemCount = UnlockedSkillsList.Count;
            lowerIndex = (curPage - 1) * gridNum;
            // 索引上限为当前页*格子数量-1，如果超过list大小，则为list的元素数量-1
            upperIndex = curPage * gridNum - 1 >= bagItemCount ? bagItemCount - 1 : curPage * gridNum - 1;
        }
        /// <summary>
        /// 已解锁技能页面切换按钮监听
        /// </summary>
        private void PageChange()
        {
            BtnNextPage.onClick.AddListener(() =>
            {
                if (curPage >= totalPage)
                {
                    Debug.Log("无法往后翻页");
                }
                else
                {
                    curPage++;
                    UpdateIndex();
                    RefreshSkillPanelLayout();
                }

            });
            BtnPrePage.onClick.AddListener(() =>
            {
                if (curPage == 1)
                {
                    Debug.Log("无法往前翻页");
                }
                else
                {
                    curPage--;
                    UpdateIndex();
                    RefreshSkillPanelLayout();
                }
            });
        }
        /// <summary>
        /// 在[lowerBound, upperBound)范围内随机选择一个整数，不包含upperBound
        /// </summary>
        private int RollDiceFunc(int lowerBound, int uppperBound)
        {
            return UnityEngine.Random.Range(lowerBound, uppperBound);
        }
    }
}
