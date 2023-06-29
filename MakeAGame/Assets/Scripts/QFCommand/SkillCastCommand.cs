using PauseUI;
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
            switch (desiredSkill)
            {
                case SkillNameEnum.None:
                    break;
                case SkillNameEnum.Alienation1:
                    GameManager.Instance.soundMan.Play_alienation1_sound();
                    Alienation1 alienation1 = new Alienation1();
                    Alienation.alienationLevel = 1;
                    alienation1.SkillStart(GameObject.FindGameObjectWithTag("PauseButton"), 1);
                    break;
                case SkillNameEnum.Alienation2:
                    GameManager.Instance.soundMan.Play_alienation2_sound();
                    Alienation2 alienation2 = new Alienation2();
                    Alienation.alienationLevel = 2;
                    alienation2.SkillStart(GameObject.FindGameObjectWithTag("PauseButton"), 2);
                    break;
                case SkillNameEnum.Focus1:
                    GameManager.Instance.soundMan.Play_focus1_sound();
                    this.SendEvent(new SkillLockCameraEvent()
                    {
                        cameraDist = GameManager.Instance.camMan.GetMinScrollLimit(),
                        duration = 10f
                    });
                    GameManager.Instance.playerMan.player.SetTurnPieceCount(10); // 棋子移动次数10
                    break;
                case SkillNameEnum.Focus2:
                    GameManager.Instance.soundMan.Play_focus2_sound();
                    this.SendEvent(new SkillLockCameraEvent()
                    {
                        cameraDist = GameManager.Instance.camMan.GetMinScrollLimit(),
                        duration = 10f
                    });
                    GameManager.Instance.playerMan.player.SetTurnPieceCount(20); // 棋子移动次数20
                    break;
            }
        }


    }

}
