using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using QFramework;
using Game;
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null; // GM实例

    // 其他管理器，由管理器自身创建时赋给GM
    public CameraManager camMan;
    public PlayerManager playerMan;
    public GameSceneManager gameSceneMan;
    public BuffManager buffMan;

    /// <summary>
    /// GM的初始化以及获取GM的getter
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance; 
        }
    }

    /// <summary>
    /// 暂停游戏，例如玩家点击了设置按钮
    /// </summary>
    public void PauseGame()
    {

    }

    /// <summary>
    /// 暂停战斗，战斗外操作如对话仍可继续进行
    /// </summary>
    public void PauseCombat()
    {
        Time.timeScale = 0;
           
    }

    /// <summary>
    /// 彻底恢复游戏，从完全暂停状态
    /// </summary>
    public void ResumeGame()
    {

    }

    /// <summary>
    /// 恢复战斗
    /// </summary>
    public void ResumeCombat()
    {
        Time.timeScale = 1;
    }
    
}
