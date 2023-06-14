using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using QFramework;
public class CombatDialogueControl : MonoBehaviour
{
    [SerializeField]
    GameObject m_gameObject;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneFlow.combatSceneCount == 1)
        {
            m_gameObject.SetActive(true);
            GameManager.Instance.PauseCombat();
            UIKit.HidePanel<UIHandCard>();
            
        }else
        {
            m_gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
