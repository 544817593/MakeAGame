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
            UIKit.GetPanel<DialoguePanel>().NPC.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/NPC/弗朗西斯");
            UIKit.GetPanel<DialoguePanel>().NPC.GetComponent<Image>().LocalScale(0.5f, 1.0f, 1f);
            m_gameObject.GetComponent<Dialogue>().ink_file = Resources.Load<TextAsset>("Dialogue/Chapter1.1");            
            m_gameObject.GetComponent<Dialogue>().popName = "弗朗西斯·维兰德·瑟斯顿";
            GameManager.Instance.PauseGame();
            UIKit.HidePanel<UIHandCard>();
           
            this.RegisterEvent<CombatVictoryEvent>(e => OnCombatVictoryEvent()).UnRegisterWhenGameObjectDestroyed(this);

        }
        else if(SceneFlow.combatSceneCount == 2)
        {
            m_gameObject.SetActive(true);
            m_gameObject.GetComponent<Dialogue>().ink_file = Resources.Load<TextAsset>("Dialogue/Chapter1.3");
            GameManager.Instance.PauseGame();
            UIKit.HidePanel<UIHandCard>();
            this.RegisterEvent<CombatVictoryEvent>(e => OnNormalCombatVictoryEvent()).UnRegisterWhenGameObjectDestroyed(this);
        }
        else
        {
            ResKit.Init();
            m_gameObject.SetActive(false);
            this.RegisterEvent<CombatVictoryEvent>(e => OnNormalCombatVictoryEvent()).UnRegisterWhenGameObjectDestroyed(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

   
    private void OnCombatVictoryEvent()
    {
        UIKit.GetPanel<DialoguePanel>().NPC.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/NPC/弗朗西斯");
        UIKit.GetPanel<DialoguePanel>().NPC.GetComponent<Image>().LocalScale(0.7f, 0.7f, 1f);
        m_gameObject.GetComponent<Dialogue>().WaitForPass();
       
        GameManager.Instance.PauseGame();
       
    }
    private void OnNormalCombatVictoryEvent()
    {
        GameManager.Instance.PauseGame();
        UIKit.HideAllPanel();
        UIKit.OpenPanel<RewardUI.RewardUIPanel>();
    }

}
