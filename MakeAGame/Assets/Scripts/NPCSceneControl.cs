using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEditor;

public class NPCSceneControl : MonoBehaviour
{
    [SerializeField]
    GameObject m_gameObject;
    public CheckControl m_checkControl;
    public ShowGift m_showGift;
    // Start is called before the first frame update
    void Awake()
    {
       
        if (SceneFlow.NpcSceneCount == 1)
        {
            m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/Chapter1.2.json");
            m_gameObject.GetComponent<Dialogue>().m_checkControl = m_checkControl;
            m_gameObject.GetComponent<Dialogue>().m_showGift = m_showGift;
        }
        else
        {
            m_gameObject.GetComponent<Dialogue>().m_checkControl = null;
            m_gameObject.GetComponent<Dialogue>().m_showGift = null;
            int x = Random.Range(0, 3);
            switch (x)
            {
                case 0:
                    m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/NPC1.json");
                    break;
                case 1:
                    m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/NPC2.json");
                    break;
                case 2:
                    m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/NPC3.json");
                    break;

            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
