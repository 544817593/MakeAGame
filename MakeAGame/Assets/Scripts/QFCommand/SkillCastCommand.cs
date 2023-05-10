using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillCastCommand : AbstractCommand
    {
        private readonly bool leftSkill;

        public SkillCastCommand(bool leftSkill)
        {
            this.leftSkill = leftSkill;
        }

        protected override void OnExecute()
        {
            ISkillSystem skillSystem = this.GetSystem<ISkillSystem>();
            SkillNameEnum desiredSkill = leftSkill ? skillSystem.GetEquippedSkillsList()[0] : skillSystem.GetEquippedSkillsList()[1];
            if (!skillSystem.CanUseSkill(desiredSkill)) return;
            // TODO 通过技能名字Enum施放技能
            
        }


    }

}
