using InventoryQuickslotUI;
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

        /// <summary>
        /// 技能盘UI
        /// </summary>
        UIAbilityPanel abilityPanel { get; set; }

    }
    public class SkillSystem : AbstractSystem, ISkillSystem, ICanSendCommand
    {
        public List<SkillNameEnum> unlockedSkillsList { get; private set; } // 已解锁的技能
        public List<SkillNameEnum> equippedSkillsList { get; private set; } // 装备中的技能
        public List<SkillNameEnum> availableSkillsList { get; private set; } // 可使用的技能
      
        public UIAbilityPanel abilityPanel { get; set; }

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
            {SkillNameEnum.LastResort, "LastResort" },
            {SkillNameEnum.DimentionalPortal, "DimentionalPortal" },
            {SkillNameEnum.Inferno, "Inferno" },
            {SkillNameEnum.Oceanic, "Oceanic" }
        }; // 所有技能枚举map到对应资源的名字
        // 中文名
        public static Dictionary<SkillNameEnum, string> skillChineseNameDict = new Dictionary<SkillNameEnum, string>()
        {
            {SkillNameEnum.None, ""},
            {SkillNameEnum.Alienation1, "异化I" },
            {SkillNameEnum.Alienation2, "异化II" },
            {SkillNameEnum.Earthquake1, "地震I" },
            {SkillNameEnum.Darkarrival, "黑暗降临" },
            {SkillNameEnum.Focus1, "聚焦I" },
            {SkillNameEnum.Focus2, "聚焦II" },
            {SkillNameEnum.Ghost, "疾跑" },
            {SkillNameEnum.LastResort, "破釜沉舟" },
            {SkillNameEnum.DimentionalPortal, "次元门" },
            {SkillNameEnum.Inferno, "炼狱" },
            {SkillNameEnum.Oceanic, "沧海" }
        };
        // 技能描述 TODO: 更新
        public static Dictionary<SkillNameEnum, string> skillDescriptionDict = new Dictionary<SkillNameEnum, string>()
        {
            {SkillNameEnum.None, ""},
            {SkillNameEnum.Alienation1, "引导者将游戏中的暂停按钮异化30秒。Trainee单击异化后的按钮，敌方时停7秒，我方卡牌在这期间不会因为寿命原因而消失。" },
            {SkillNameEnum.Alienation2, "引导者将游戏中的暂停按钮异化45秒。Trainee单击异化后的按钮，敌方时停12秒，我方卡牌在这期间不会因为寿命原因而消失。" },
            {SkillNameEnum.Earthquake1, "引导者疯狂抖动游戏屏幕，使得房间内的怪物无法移动，持续15秒。" },
            {SkillNameEnum.Darkarrival, "引导者帮助Trainee将房间内一半的怪物生命降为1，但Trainee将会失明7秒。" },
            {SkillNameEnum.Focus1, "你的镜头出现了一些问题，需要10秒才能修好，但你的改变移动方向次数似乎增加了10次。" },
            {SkillNameEnum.Focus2, "你的镜头出现了一些问题，需要10秒才能修好，但你的改变移动方向次数似乎增加了20次。" },
            {SkillNameEnum.Ghost, "当你剩余的改变移动方向次数低于10时，可使用并增加10次数。" },
            {SkillNameEnum.LastResort, "你所有手牌的精神消耗永久减少50%，你本关将不能再抽牌。你的死面牌的伤害和治疗量翻倍，时间类卡牌持续时间翻倍。" },
            {SkillNameEnum.DimentionalPortal, "引导者帮你把接下来30秒的怪物刷新给关闭掉。" },
            {SkillNameEnum.Inferno, "将所有可改变时间流逝速度地块的倍率变为最高级别，并附加场景效果:落火。落火：每个地块每秒有1%的概率降下燃烧的陨石，对砸到的敌方单位造成50点伤害。" },
            {SkillNameEnum.Oceanic, "将所有可改变地块的属性变为水，并附加场景效果:潮汐。潮汐：每7秒在地图随机一排引发海啸，从左到右行进，对卷入的敌人造成20点伤害并永久降低一半的移动速度。" }
        };


        protected override void OnInit()
        {
            unlockedSkillsList = new List<SkillNameEnum>();
            equippedSkillsList = new List<SkillNameEnum>(2) {SkillNameEnum.None, SkillNameEnum.None }; // 只可以携带两个技能
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
                {SkillNameEnum.LastResort, "LastResort" },
                {SkillNameEnum.DimentionalPortal, "DimentionalPortal" },
                {SkillNameEnum.Inferno, "Inferno" },
                {SkillNameEnum.Oceanic, "Oceanic" }
            };
            skillChineseNameDict = new Dictionary<SkillNameEnum, string>()
            {
                {SkillNameEnum.None, ""},
                {SkillNameEnum.Alienation1, "异化I" },
                {SkillNameEnum.Alienation2, "异化II" },
                {SkillNameEnum.Earthquake1, "地震I" },
                {SkillNameEnum.Darkarrival, "黑暗降临" },
                {SkillNameEnum.Focus1, "聚焦I" },
                {SkillNameEnum.Focus2, "聚焦II" },
                {SkillNameEnum.Ghost, "疾跑" },
                {SkillNameEnum.LastResort, "破釜沉舟" },
                {SkillNameEnum.DimentionalPortal, "次元门" },
                {SkillNameEnum.Inferno, "炼狱" },
                {SkillNameEnum.Oceanic, "沧海" }
            };

            skillDescriptionDict = new Dictionary<SkillNameEnum, string>()
            {
                {SkillNameEnum.None, ""},
                {SkillNameEnum.Alienation1, "引导者将游戏中的暂停按钮异化30秒。Trainee单击异化后的按钮，敌方时停7秒，我方卡牌在这期间不会因为寿命原因而消失。" },
                {SkillNameEnum.Alienation2, "引导者将游戏中的暂停按钮异化45秒。Trainee单击异化后的按钮，敌方时停12秒，我方卡牌在这期间不会因为寿命原因而消失。" },
                {SkillNameEnum.Earthquake1, "引导者疯狂抖动游戏屏幕，使得房间内的怪物无法移动，持续15秒。" },
                {SkillNameEnum.Darkarrival, "引导者帮助Trainee将房间内一半的怪物生命降为1，但Trainee将会失明7秒。" },
                {SkillNameEnum.Focus1, "你的镜头出现了一些问题，需要10秒才能修好，但你的改变移动方向次数似乎增加了10次。" },
                {SkillNameEnum.Focus2, "你的镜头出现了一些问题，需要10秒才能修好，但你的改变移动方向次数似乎增加了20次。" },
                {SkillNameEnum.Ghost, "当你剩余的改变移动方向次数低于10时，可使用并增加10次数。" },
                {SkillNameEnum.LastResort, "你所有手牌的精神消耗永久减少50%，你本关将不能再抽牌。你的死面牌的伤害和治疗量翻倍，时间类卡牌持续时间翻倍。" },
                {SkillNameEnum.DimentionalPortal, "引导者帮你把接下来30秒的怪物刷新给关闭掉。" },
                {SkillNameEnum.Inferno, "将所有可改变时间流逝速度地块的倍率变为最高级别，并附加场景效果:落火。落火：每个地块每秒有1%的概率降下燃烧的陨石，对砸到的敌方单位造成50点伤害。" },
                {SkillNameEnum.Oceanic, "将所有可改变地块的属性变为水，并附加场景效果:潮汐。潮汐：每7秒在地图随机一排引发海啸，从左到右行进，对卷入的敌人造成20点伤害并永久降低一半的移动速度。" }
            };

            UIKit.OpenPanel<UIAbilityPanel>();
            abilityPanel = UIKit.GetPanel<UIAbilityPanel>();
            UIKit.HidePanel<UIAbilityPanel>();
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
                if (equippedSkillsList[0] == SkillNameEnum.None)
                {
                    abilityPanel.Skill1.image.sprite = GetSkillIconSprite(skillName);
                    abilityPanel.Skill1.image.color = new Color(255, 255,255, 255);
                    equippedSkillsList[0] = skillName;
                }
                else if (equippedSkillsList[1] == SkillNameEnum.None)
                {
                    abilityPanel.Skill2.image.sprite = GetSkillIconSprite(skillName);
                    abilityPanel.Skill2.image.color = new Color(255, 255, 255, 255);
                    equippedSkillsList[1] = skillName;
                }
                
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
            GameManager.Instance.soundMan.Play_UI_Click_CastSpell();
        }

        public bool CanUseSkill(SkillNameEnum skill)
        {
            if (!GetAvailableSkillsList().Contains(skill)) return false;
            return IsSkillUnlocked(skill);
        }
    }
}

