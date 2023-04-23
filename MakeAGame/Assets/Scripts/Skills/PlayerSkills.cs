using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    // public event EventHandler<MsgSkillUnlocked> OnSkillUnlocked;
    // public event EventHandler<MsgEquippedSkillsChange> OnEquippedSkillsChange;


    private List<SkillName> unlockedSkillsList; // 已解锁的技能
    private List<SkillName> equippedSkillsList; // 装备中的技能
    private List<SkillName> availableSkillsList; // 可使用的技能

    /// <summary>
    /// PlayerSkills构造函数
    /// </summary>
    public PlayerSkills()
    {
        unlockedSkillsList = new List<SkillName>();
        equippedSkillsList = new List<SkillName>(2); // 只可以携带两个技能
        availableSkillsList = new List<SkillName>();
    }

    /// <summary>
    /// 获取已解锁的技能列表
    /// </summary>
    /// <returns>已解锁的技能列表</returns>
    public List<SkillName> GetUnlockedSkills()
    {
        return unlockedSkillsList;
    }

    /// <summary>
    /// 解锁一个技能
    /// </summary>
    /// <param name="skillName">技能名</param>
    private void UnlockSkill(SkillName skillName)
    {
        if (!IsSkillUnlocked(skillName))
        {
            unlockedSkillsList.Add(skillName);
            // If OnSkillUnlocked != null then Invoke an event
            // OnSkillUnlocked?.Invoke(this, new MsgSkillUnlocked { skillName = skillName });
        }
    }

    /// <summary>
    /// 检查技能是否已经解锁
    /// </summary>
    /// <param name="skillName">技能名</param>
    /// <returns>返回是否解锁</returns>
    public bool IsSkillUnlocked(SkillName skillName)
    {
        return unlockedSkillsList.Contains(skillName);
    }

    /// <summary>
    /// 获取技能的前置解锁条件
    /// </summary>
    /// <param name="skillname">技能名</param>
    /// <returns>需要解锁的前置技能</returns>
    public SkillName GetSkillRequirement(SkillName skillname)
    {
        switch (skillname)
        {
            case SkillName.Focus2:
                return SkillName.Focus1;
            case SkillName.Alienation2:
                return SkillName.Alienation1;
        }
        return SkillName.None;
    }

    /// <summary>
    /// 尝试解锁技能
    /// </summary>
    /// <param name="skillName"></param>
    /// <returns>解锁技能成功或者失败</returns>
    public bool TryUnlockSkill(SkillName skillName)
    {
        if (CanUnlock(skillName))
        {
            UnlockSkill(skillName);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 检查技能可否被解锁
    /// </summary>
    /// <param name="skillName">技能名字</param>
    /// <returns>是否可解锁</returns>
    public bool CanUnlock(SkillName skillName)
    {
        SkillName skillRequirement = GetSkillRequirement(skillName);
        if (skillRequirement != SkillName.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 获取可使用技能列表
    /// </summary>
    /// <returns>可使用的技能</returns>
    public List<SkillName> GetAvailableSkillsList()
    {
        return availableSkillsList;
    }

    /// <summary>
    /// 设置可使用的技能列表
    /// </summary>
    /// <param name="skillNames"></param>
    public void SetAvailableSkillsList(List<SkillName> skillNames)
    {
        availableSkillsList = skillNames;
    }

    /// <summary>
    /// 获得已装备的技能列表
    /// </summary>
    /// <returns>已装备的技能列表</returns>
    public List<SkillName> GetEquippedSkillsList()
    {
        return equippedSkillsList;
    }

    /// <summary>
    /// 添加一个技能到已装备技能栏
    /// </summary>
    /// <param name="skillName">技能名</param>
    public void AddEquippedSkills(SkillName skillName)
    {
        equippedSkillsList.Add(skillName);
        //OnEquippedSkillsChange?.Invoke(this, new MsgEquippedSkillsChange { });
    }

    /// <summary>
    /// 从已装备技能栏更换一个技能
    /// </summary>
    /// <param name="position">位置，1或者2</param>
    /// <param name="skillName">技能名</param>
    public void ChangeEquippedSkillsList(int position, SkillName skillName)
    {
        if (!IsSkillUnlocked(skillName)) return;

        // 至少两个技能已解锁
        if (GetUnlockedSkills().Count > 1)
        {
            // 技能装备在技能栏1中但是想换到0
            if (position == 0 && (int)GetEquippedSkillsList()[1] == (int)skillName)
            {
                SkillName temp = GetEquippedSkillsList()[0];
                equippedSkillsList[position] = skillName;
                equippedSkillsList[1] = temp;
            }
            // 反过来同理
            else if (position == 1 && (int)GetEquippedSkillsList()[0] == (int)skillName)
            {
                SkillName temp = GetEquippedSkillsList()[1];
                equippedSkillsList[position] = skillName;
                equippedSkillsList[0] = temp;
            }
            // 解锁技能大于等于2，想换成一个新技能
            else
            {
                equippedSkillsList[position] = skillName;
            }
        }
        // 只有一个技能解锁了
        else
        {
            equippedSkillsList[0] = skillName;
        }

        // OnEquippedSkillsChange?.Invoke(this, new MsgEquippedSkillsChange { });
    }

    /// <summary>
    /// 转换技能名为实际文字
    /// </summary>
    /// <param name="id">要转换的技能名</param>
    /// <returns>技能的实际文字</returns>
    public string SkillIdToName(SkillName id)
    {
        switch (id)
        {
            case SkillName.None:
                return "无";
            case SkillName.Alienation1:
                return "异化I";
            case SkillName.Earthquake1:
                return "地震I";
            case SkillName.Darkarrival:
                return "黑暗降临";
            case SkillName.Focus1:
                return "聚焦I";
            case SkillName.Alienation2:
                return "异化II";
            case SkillName.Focus2:
                return "聚焦II";
            default:
                return "";
        }
    }


}
