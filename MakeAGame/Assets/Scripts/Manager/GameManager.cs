using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null; // GM实例

    // 其他管理器，由管理器自身创建时赋给GM
    public CameraManager camMan;
    public PlayerManager playerMan;
    public GameSceneManager gameSceneMan;
    public MonsterManager monsterMan;

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
}
