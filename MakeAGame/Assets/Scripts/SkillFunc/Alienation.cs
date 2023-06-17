using PauseUI;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Alienation : ICanGetSystem
    {
        protected float colorChangeTime;
        protected float timeStopTime;
        public static int alienationLevel = 0;
        public IEnumerator SkillStart(GameObject pauseButton, int skillLevel)
        {
            if (pauseButton != null && pauseButton.GetComponent<Image>() != null)
            {
                Color preColor = pauseButton.GetComponent<Image>().color;
                Color newColor = new Color(255, 0, 0, 255);
                pauseButton.GetComponent<Image>().color = newColor;
                alienationLevel = skillLevel;
                yield return new WaitForSeconds(colorChangeTime);
                // 如果等待时间过后图标没有变化说明没有点，把图片颜色换回原来的
                if (pauseButton != null && pauseButton.GetComponent<Image>() != null && pauseButton.GetComponent<Image>().color.Equals(newColor))
                {
                    Debug.Log("Alienation1图片颜色改回原本");
                    pauseButton.GetComponent<Image>().color = preColor;
                }
            }
            else
            {
                Debug.LogError("Alienation1启动失败");
                yield return null;
            }
        }

        public IEnumerator ClickAfterStart(GameObject pauseButton)
        {
            
            if (pauseButton != null && pauseButton.GetComponent<Image>() != null && pauseButton.GetComponent<Image>().color.Equals(new Color(255, 0, 0, 255)))
            {
                Color preColor = pauseButton.GetComponent<Image>().color;
                pauseButton.GetComponent<Image>().color = new Color(255, 255, 255, 255); // 暂停图片改回白色
                foreach (Monster monster in this.GetSystem<IPieceSystem>().pieceEnemyList) // 暂停怪物时间
                {
                    monster.timeStop = true;
                }
                foreach(ViewPiece piece in this.GetSystem<IPieceSystem>().pieceFriendList) // 友军不会因寿命死亡
                {
                    piece.lockLife = true;
                }

                yield return new WaitForSeconds(timeStopTime); // 等待7秒后全部恢复

                if (pauseButton != null && pauseButton.GetComponent<Image>() != null)
                {
                    pauseButton.GetComponent<Image>().color = preColor;
                }
                foreach (Monster monster in this.GetSystem<IPieceSystem>().pieceEnemyList)
                {
                    monster.timeStop = false;
                }
                foreach (ViewPiece piece in this.GetSystem<IPieceSystem>().pieceFriendList)
                {
                    piece.lockLife = false;
                }
                alienationLevel = 0;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }

    public class Alienation1 : Alienation
    {
        public Alienation1()
        {
            colorChangeTime = 30f;
            timeStopTime = 7f;
        }
    }

    public class Alienation2 : Alienation
    {
        public Alienation2()
        {
            colorChangeTime = 45f;
            timeStopTime = 12f;
        }
    }
}

