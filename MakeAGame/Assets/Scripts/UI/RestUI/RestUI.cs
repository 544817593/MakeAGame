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

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as RestUIData ?? new RestUIData();
            // please add init code here

            //测试数据用
            foreach(SkillNameEnum skillName in SkillSystem.skillIDNameDic.Keys)
            {
                if(skillName != SkillNameEnum.None)
                {
                    mData.skillSystem.UnlockSkill(skillName);
                }
            }
            mData.skillSystem.AddEquippedSkills(SkillNameEnum.None);
            mData.skillSystem.AddEquippedSkills(SkillNameEnum.None);


            UnlockedSkillsList = mData.skillSystem.GetUnlockedSkills();
            EquippedSkillsList = mData.skillSystem.GetEquippedSkillsList();
            //Sprite x = Resources.Load<Sprite>("Sprites/Rest/0");
            //Debug.Log(x != null);
            //Dice01.sprite = x;
            ChangeSkillPanel.gameObject.SetActive(false);
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
        /// <summary>
        /// 全部按钮监听
        /// </summary>
		private void AllButtonListen()
		{
            PageChange();
			// 摇骰子按钮监听，执行摇骰子逻辑，替换显示的骰子图
            BtnRollDice.onClick.AddListener(() =>
            {
				List<int> randNumList = new List<int>();
				// 摇6个骰子
				for(int i = 1; i <= 6; ++i)
				{
                    int randNum = RollDiceFunc(0, 3);
                    randNumList.Add(randNum);
                }
				int idx = 0;
                foreach(Transform transform in AllDice.GetComponentInChildren<Transform>())
				{
                    //Debug.Log(transform.gameObject.name);
					Assert.IsTrue(idx < randNumList.Count);
                    transform.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Rest/{randNumList[idx]}");
					idx++;
                }
            });

			// 点击切换技能按钮，设置为active
            BtnChangeSkill.onClick.AddListener(() =>
			{
				ChangeSkillPanel.gameObject.SetActive(true);
                // 点击关闭切换技能页面，设置为inactive
                CloseChangeSkillPanel.onClick.AddListener(() =>
                {
                    ChangeSkillPanel.gameObject.SetActive(false);
                });
				// 如果1号位装备了技能，点击后卸下当前技能
				CurAbility01.onClick.AddListener(() =>
				{
					if(CurAbility01.GetComponent<Image>().sprite != null)
					{
                        // TODO: 需要一个格子的背景图替换
                        CurAbility01.GetComponent<Image>().sprite = null;
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
                    if (CurAbility02.GetComponent<Image>().sprite != null)
                    {
                        // TODO: 需要一个格子的背景图替换
                        CurAbility02.GetComponent<Image>().sprite = null;
                        EquippedSkillsList[1] = SkillNameEnum.None;
                    }
                    else
                    {
                        Debug.Log("当前位置没有装备技能");
                    }
                });
                RefreshLayout();
            });

			// 点击离开房间，关闭当前panel
            BtnLeaveRoom.onClick.AddListener(() =>
            {
                CloseSelf();
                GameObject.Find("GameSceneManager")?.transform.GetComponent<Game.SceneFlow>().LoadRoom();
            });
        }

        /// <summary>
        /// 切换技能面板的初始化和刷新
        /// </summary>
        public void RefreshLayout()
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
                            //EquippedSkillsList[0] = UnlockedSkillsList[unlockSkillToIdx[curbtn]];
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
                    RefreshLayout();
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
                    RefreshLayout();
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
