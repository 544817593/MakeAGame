using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using QFramework;
using System;
using UnityEngine.UI;
using DialogueUI;
using UnityEditor;
public class CombatDialogueControl : MonoBehaviour, IController, ICanSendEvent
{
    [SerializeField]
    GameObject m_gameObject;
    public bool start_dialogue;
   
    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }

    // Start is called before the first frame update
    void Start()
    {
   
        if (SceneFlow.combatSceneCount == 1)
        {
          
            m_gameObject.SetActive(true);
            m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/Chapter1.1.json");
            m_gameObject.GetComponent<Dialogue>().popName = "弗朗西斯·维兰德·瑟斯顿";
            GameManager.Instance.PauseGame();
            UIKit.HidePanel<UIHandCard>();
           
            this.RegisterEvent<CombatVictoryEvent>(e => OnCombatVictoryEvent());

        }
      
        else
        {
            ResKit.Init();
            m_gameObject.SetActive(false);
            this.RegisterEvent<CombatVictoryEvent>(e => OnNormalCombatVictoryEvent());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(start_dialogue ==true)
        {
            m_gameObject.SetActive(true);
            m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/Chapter1.3.json");
            m_gameObject.GetComponent<Dialogue>().popName = "猫头鹰";
            GameManager.Instance.PauseGame();
            UIKit.HidePanel<UIHandCard>();
        }
    }

   
    private void OnCombatVictoryEvent()
    {
      
        m_gameObject.GetComponent<Dialogue>().WaitForPass();
        UIKit.GetPanel<DialoguePanel>().NPC.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/NPC/弗朗西斯");
        GameManager.Instance.PauseGame();
       
    }
    private void OnNormalCombatVictoryEvent()
    {
        GameManager.Instance.PauseGame();
        UIKit.HideAllPanel();
        UIKit.ShowPanel<RewardUI.RewardUIPanel>();
    }

}
