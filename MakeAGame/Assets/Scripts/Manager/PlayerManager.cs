using Game;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance; // 玩家管理器示例
    private InventorySystem inventory;
    public SOPlayer player;

    /// <summary>
    /// 玩家管理器Getter
    /// </summary>
    public static PlayerManager Instance { get { return instance; } }

    private void Awake()
    {
        // 确保单例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // 把自身赋予GM
        GameManager.Instance.playerMan = this;

        inventory = GameEntry.Interface.GetSystem<InventorySystem>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
