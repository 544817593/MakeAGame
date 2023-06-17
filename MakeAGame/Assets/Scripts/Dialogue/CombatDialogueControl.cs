using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using QFramework;
using System;
public class CombatDialogueControl : MonoBehaviour, IController, ICanSendEvent
{
    [SerializeField]
    GameObject m_gameObject;
    
   
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
            GameManager.Instance.PauseCombat();
            UIKit.HidePanel<UIHandCard>();
            this.RegisterEvent<CombatVictoryEvent>(e => OnCombatVictoryEvent());

        }
        else
        {
            m_gameObject.SetActive(false);
            this.RegisterEvent<CombatVictoryEvent>(e => OnNormalCombatVictoryEvent());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    private void OnCombatVictoryEvent()
    {
      
        m_gameObject.GetComponent<Dialogue>().WaitForPass();
        GameManager.Instance.PauseCombat();
       
    }
    private void OnNormalCombatVictoryEvent()
    {
        GameObject.Find("GameSceneManager")?.transform.GetComponent<SceneFlow>().LoadRoom();
    }

}
