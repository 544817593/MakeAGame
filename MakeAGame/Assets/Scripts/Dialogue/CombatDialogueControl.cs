using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
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
