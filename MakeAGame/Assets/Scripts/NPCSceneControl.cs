using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEditor;

public class NPCSceneControl : MonoBehaviour
{
    [SerializeField]
    GameObject m_gameObject;
    [SerializeField]
    GameObject m_door;
    [SerializeField]
    GameObject NPC1;
    [SerializeField]
    GameObject NPC2;
    [SerializeField]
    GameObject NPC3;
    public CheckControl m_checkControl;
    public ShowGift m_showGift;
    public bool NoIntroNPC = false;
    // Start is called before the first frame update
    void Awake()
    {
        m_door.SetActive(false);
        NPC1.SetActive(false);
        NPC2.SetActive(false);
        NPC3.SetActive(false);

        if (SceneFlow.NpcSceneCount == 1)
        {
            m_gameObject.GetComponent<Dialogue>().ink_file = Resources.Load<TextAsset>("Dialogue/Chapter1.2");
            m_gameObject.GetComponent<Dialogue>().m_checkControl = m_checkControl;
            m_gameObject.GetComponent<Dialogue>().m_showGift = m_showGift;
            m_gameObject.GetComponent<Dialogue>().bgPath = "Sprites/SceneBackground/吟游诗人";
            NPC1.SetActive(true);
            
        }
        else
        {
           
            m_gameObject.GetComponent<Dialogue>().m_checkControl = null;
            m_gameObject.GetComponent<Dialogue>().m_showGift = null;
            m_gameObject.GetComponent<Dialogue>().npc.SetActive(false);
           // m_gameObject.GetComponent<Dialogue>().npc = null;
            int x = Random.Range(0, 3);
            switch (x)
            {
                case 0:
                    m_gameObject.GetComponent<Dialogue>().ink_file = Resources.Load<TextAsset>("Dialogue/NPC1");
                    break;
                case 1:
                    m_gameObject.GetComponent<Dialogue>().ink_file = Resources.Load<TextAsset>("Dialogue/NPC2");
                    break;
                case 2:
                    m_gameObject.GetComponent<Dialogue>().ink_file = Resources.Load<TextAsset>("Dialogue/NPC3");
                    break;
                    
            }
            NoIntroNPC = true;
           

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_gameObject.GetComponent<Dialogue>().d_finish==true)
        {
            m_door.SetActive(true);
        }
        if (NoIntroNPC ==true)
        {
            NewDia();
        }
    }

    public void NewDia()
    {
        if(m_gameObject.GetComponent<Dialogue>().d_finish == true)
        {

            NPC2.SetActive(true);
            NPC3.SetActive(true);
            NPC1.SetActive(false);
            NoIntroNPC = false;


        }
        
    }
}
