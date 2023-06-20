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
    public bool NoIntroNPC = false;
    // Start is called before the first frame update
    void Awake()
    {
       
        if (SceneFlow.NpcSceneCount == 1)
        {
            m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/Chapter1.2.json");
            m_gameObject.GetComponent<Dialogue>().m_checkControl = m_checkControl;
            m_gameObject.GetComponent<Dialogue>().m_showGift = m_showGift;
            m_gameObject.GetComponent<Dialogue>().bgPath = "Sprites/SceneBackground/吟游诗人";
        }
        else
        {
            m_gameObject.GetComponent<Dialogue>().m_checkControl = null;
            m_gameObject.GetComponent<Dialogue>().m_showGift = null;
           // m_gameObject.GetComponent<Dialogue>().npc = null;
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
            NoIntroNPC = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(NoIntroNPC == true)
        {
            NewDia();
        }
    }

    public void NewDia()
    {
        if(m_gameObject.GetComponent<Dialogue>().d_finish == true)
        {
            m_gameObject.GetComponent<Dialogue>().npc.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/NPC/NPC-修女"); 
            m_gameObject.GetComponent<Dialogue>().bgPath = "Sprites/SceneBackground/修女"; 
            m_gameObject.GetComponent<Dialogue>().ink_file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Dialogue/Chapter1.5.json");
            NoIntroNPC = false;
        }
        
    }
}
