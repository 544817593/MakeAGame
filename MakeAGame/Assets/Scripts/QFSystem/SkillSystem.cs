using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public interface ISkillSystem : ISystem
    {
        /// <summary>
        /// 获取已解锁的技能列表
        /// </summary>
        /// <returns>已解锁的技能列表</returns>
        public List<SkillNameEnum> GetUnlockedSkills();

        /// <summary>
        /// 解锁一个技能
        /// </summary>
        /// <param name="skillName">技能名</param>
        public void UnlockSkill(SkillNameEnum skillName);

        /// <summary>
        /// 检查技能是否已经解锁
        /// </summary>
        /// <param name="skillName">技能名</param>
        /// <returns>返回是否解锁</returns>
        public bool IsSkillUnlocked(SkillNameEnum skillName);

        /// <summary>
        /// 获取技能的前置解锁条件
        /// </summary>
        /// <param name="skillname">技能名</param>
        /// <returns>需要解锁的前置技能</returns>
        public SkillNameEnum GetSkillRequirement(SkillNameEnum skillname);

        /// <summary>
        /// 尝试解锁技能
        /// </summary>
        /// <param name="skillName"></param>
        /// <returns>解锁技能成功或者失败</returns>
        public bool TryUnlockSkill(SkillNameEnum skillName);

        /// <summary>
        /// 检查技能可否被解锁
        /// </summary>
        /// <param name="skillName">技能名字</param>
        /// <returns>是否可解锁</returns>
        public bool CanUnlock(SkillNameEnum skillName);

        /// <summary>
        /// 获取可使用技能列表
        /// </summary>
        /// <returns>可使用的技能</returns>
        public List<SkillNameEnum> GetAvailableSkillsList();

        /// <summary>
        /// 设置可使用的技能列表
        /// </summary>
        /// <param name="skillNames"></param>
        public void SetAvailableSkillsList(List<SkillNameEnum> skillNames);

        /// <summary>
        /// 获得已装备的技能列表
        /// </summary>
        /// <returns>已装备的技能列表</returns>
        public List<SkillNameEnum> GetEquippedSkillsList();

        /// <summary>
        /// 添加一个技能到已装备技能栏
        /// </summary>
        /// <param name="skillName">技能名</param>
        public void AddEquippedSkills(SkillNameEnum skillName);

        /// <summary>
        /// 从已装备技能栏更换一个技能
        /// </summary>
        /// <param name="position">位置，1或者2</param>
        /// <param name="skillName">技能名</param>
        public void ChangeEquippedSkillsList(int position, SkillNameEnum skillName);

        /// <summary>
        /// 转换技能名为实际文字
        /// </summary>
        /// <param name="id">要转换的技能名</param>
        /// <returns>技能的实际文字</returns>
        public string SkillIdToName(SkillNameEnum id);

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="topSkill">是否为上面的（Q）技能</param>
        public void CastSkill(bool topSkill);

        /// <summary>
        /// Whether a skill is unlocked and player have skill usage left
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool CanUseSkill(SkillNameEnum skill);

        /// <summary>
        /// Get skill's UI icon
        /// </summary>
        /// <param name="skill">Skill name enum</param>
        /// <returns></returns>
        public Sprite GetSkillIconSprite(SkillNameEnum skill);
    }
    public class SkillSystem : AbstractSystem, ISkillSystem, ICanSendCommand
    {
        public List<SkillNameEnum> unlockedSkillsList { get; private set; } // 已解锁的技能
        public List<SkillNameEnum> equippedSkillsList { get; private set; } // 装备中的技能
        public List<SkillNameEnum> availableSkillsList { get; private set; } // 可使用的技能

        public static Dictionary<SkillNameEnum, string> skillIDNameDic = new Dictionary<SkillNameEnum, string>()
        {
            {SkillNameEnum.None, ""},
            {SkillNameEnum.Alienation1, "Alienation1" },
            {SkillNameEnum.Alienation2, "Alienation2" },
            {SkillNameEnum.Earthquake1, "Earthquake1" },
            {SkillNameEnum.Darkarrival, "Darkarrival" },
            {SkillNameEnum.Focus1, "Focus1" },
            {SkillNameEnum.Focus2, "Focus2" },
            {SkillNameEnum.Ghost, "Ghost" },
            {SkillNameEnum.DimentionalPortal, "DimentionalPortal" },
            {SkillNameEnum.Inferno, "Inferno" },
            {SkillNameEnum.Oceanic, "Oceanic" }
        }; // 所有技能枚举map到对应资源的名字

        protected override void OnInit()
        {
            unlockedSkillsList = new List<SkillNameEnum>();
            equippedSkillsList = new List<SkillNameEnum>(2); // 只可以携带两个技能
            availableSkillsList = new List<SkillNameEnum>();
            skillIDNameDic = new Dictionary<SkillNameEnum, string>()
            {
                {SkillNameEnum.None, ""},
                {SkillNameEnum.Alienation1, "Alienation1" },
                {SkillNameEnum.Alienation2, "Alienation2" },
                {SkillNameEnum.Earthquake1, "Earthquake1" },
                {SkillNameEnum.Darkarrival, "Darkarrival" },
                {SkillNameEnum.Focus1, "Focus1" },
                {SkillNameEnum.Focus2, "Focus2" },
                {SkillNameEnum.Ghost, "Ghost" },
                {SkillNameEnum.DimentionalPortal, "DimentionalPortal" },
                {SkillNameEnum.Inferno, "Inferno" },
                {SkillNameEnum.Oceanic, "Oceanic" }
            };
        }

        public List<SkillNameEnum> GetUnlockedSkills()
        {
            return unlockedSkillsList;
        }

        public void UnlockSkill(SkillNameEnum skillName)
        {
            if (!IsSkillUnlocked(skillName))
            {
                unlockedSkillsList.Add(skillName);
                // If OnSkillUnlocked != null then Invoke an event
                // OnSkillUnlocked?.Invoke(this, new MsgSkillUnlocked { skillName = skillName });
            }
        }

        public bool IsSkillUnlocked(SkillNameEnum skillName)
        {
            return unlockedSkillsList.Contains(skillName);
        }

        public SkillNameEnum GetSkillRequirement(SkillNameEnum skillname)
        {
            switch (skillname)
            {
                case SkillNameEnum.Focus2:
                    return SkillNameEnum.Focus1;
                case SkillNameEnum.Alienation2:
                    return SkillNameEnum.Alienation1;
            }
            return SkillNameEnum.None;
        }

        public bool TryUnlockSkill(SkillNameEnum skillName)
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

        public bool CanUnlock(SkillNameEnum skillName)
        {
            SkillNameEnum skillRequirement = GetSkillRequirement(skillName);
            if (skillRequirement != SkillNameEnum.None)
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

        public List<SkillNameEnum> GetAvailableSkillsList()
        {
            return availableSkillsList;
        }

        public void SetAvailableSkillsList(List<SkillNameEnum> skillNames)
        {
            availableSkillsList = skillNames;
        }

        public List<SkillNameEnum> GetEquippedSkillsList()
        {
            return equippedSkillsList;
        }

        public void AddEquippedSkills(SkillNameEnum skillName)
        {
            equippedSkillsList.Add(skillName);
            //OnEquippedSkillsChange?.Invoke(this, new MsgEquippedSkillsChange { });
        }

        public void ChangeEquippedSkillsList(int position, SkillNameEnum skillName)
        {
            if (!IsSkillUnlocked(skillName)) return;

            // 至少两个技能已解锁
            if (GetUnlockedSkills().Count > 1)
            {
                // 技能装备在技能栏1中但是想换到0
                if (position == 0 && (int)GetEquippedSkillsList()[1] == (int)skillName)
                {
                    SkillNameEnum temp = GetEquippedSkillsList()[0];
                    equippedSkillsList[position] = skillName;
                    equippedSkillsList[1] = temp;
                }
                // 反过来同理
                else if (position == 1 && (int)GetEquippedSkillsList()[0] == (int)skillName)
                {
                    SkillNameEnum temp = GetEquippedSkillsList()[1];
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

        public string SkillIdToName(SkillNameEnum id)
        {
            switch (id)
            {
                case SkillNameEnum.None:
                    return "";
                case SkillNameEnum.Alienation1:
                    return "Alienation1";
                case SkillNameEnum.Earthquake1:
                    return "Earthquake1";
                case SkillNameEnum.Darkarrival:
                    return "Darkarrival";
                case SkillNameEnum.Focus1:
                    return "Focus1";
                case SkillNameEnum.Alienation2:
                    return "Alienation2";
                case SkillNameEnum.Focus2:
                    return "Focus2";
                case SkillNameEnum.Ghost:
                    return "Ghost";
                case SkillNameEnum.LastResort:
                    return "LastResort";
                case SkillNameEnum.DimentionalPortal:
                    return "DimentionalPortal";
                case SkillNameEnum.Inferno:
                    return "Inferno";
                case SkillNameEnum.Oceanic:
                    return "Oceanic";
                default:
                    return "";
            }
        }

        public Sprite GetSkillIconSprite(SkillNameEnum skill)
        {
            return Resources.Load<Sprite>("Sprites/Abilities/" + skill.ToString());
        }

        public void CastSkill(bool leftSkill)
        {
            this.SendCommand(new SkillCastCommand(leftSkill));
        }

        public bool CanUseSkill(SkillNameEnum skill)
        {
            if (!GetAvailableSkillsList().Contains(skill)) return false;
            return IsSkillUnlocked(skill);
        }
    }
}

