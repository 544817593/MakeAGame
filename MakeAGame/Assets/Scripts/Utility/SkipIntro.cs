using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipIntro : MonoBehaviour
{
    [SerializeField] private Button skipBtn;

    void ToCombatScene()
    {
        StartCoroutine(GameManager.Instance.gameSceneMan.LoadScene("Combat", false));
        StartCoroutine(GameManager.Instance.gameSceneMan.UnloadScene("Main"));       
    }

    private void Awake()
    {
        skipBtn.onClick.AddListener( () => ToCombatScene());
    }
}
