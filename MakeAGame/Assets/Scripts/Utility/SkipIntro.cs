using BagUI;
using Game;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipIntro : MonoBehaviour
{
    [SerializeField] private Button skipBtn;
    [SerializeField] private Button animBtn;

    void ToCombatScene()
    {
        ICardGeneratorSystem cardGeneratorSystem = GameEntry.Interface.GetSystem<ICardGeneratorSystem>();
        IInventorySystem inventorySystem = GameEntry.Interface.GetSystem<IInventorySystem>();

        for (int j = 0; j < 2; j++)
        {
            for (int i = 1; i < 6; i++)
            {
                Card createdCard = new Card(i);
                inventorySystem.SpawnBagCardInBag(createdCard);
            }
        }

        SceneFlow.Pre_Room = "Main";
        GameObject.Find("GameSceneManager")?.transform.GetComponent<Game.SceneFlow>().LoadRoom();

    }

    void ToAnimScene()
    {
        StartCoroutine(GameManager.Instance.gameSceneMan.LoadScene("Animation", false));
        StartCoroutine(GameManager.Instance.gameSceneMan.UnloadScene("Main"));
    }

    private void Awake()
    {
        skipBtn.onClick.AddListener( () => ToCombatScene());
        animBtn.onClick.AddListener(() => ToAnimScene());
    }
}
