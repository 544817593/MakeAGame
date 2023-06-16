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
                    Alienation1 alienation1 = new Alienation1();
                    Pause.alienationLevel = 1;
                    alienation1.SkillStart(GameObject.FindGameObjectWithTag("PauseButton"));
                    break;
                case SkillNameEnum.Alienation2:
                    Alienation2 alienation2 = new Alienation2();
                    Pause.alienationLevel = 2;
                    alienation2.SkillStart(GameObject.FindGameObjectWithTag("PauseButton"));
                    break;
                case SkillNameEnum.Focus1:
                    this.SendEvent(new SkillLockCameraEvent()
                    {
                        cameraDist = GameManager.Instance.camMan.GetMinScrollLimit(),
                        duration = 10f
                    });
                    GameManager.Instance.playerMan.player.SetMovePieceTime(10); // 棋子移动次数10
                    break;
                case SkillNameEnum.Focus2:
                    this.SendEvent(new SkillLockCameraEvent()
                    {
                        cameraDist = GameManager.Instance.camMan.GetMinScrollLimit(),
                        duration = 10f
                    });
                    GameManager.Instance.playerMan.player.SetMovePieceTime(20); // 棋子移动次数20
                    break;
            }
        }


    }

}
