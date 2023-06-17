using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using QFramework;
using UnityEngine.UI;
using DialogueUI;
using Ink.Runtime;

public class IntroControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int m_count;
    [SerializeField]
     GameObject door;
    public TextAsset ink_file;
    public GameObject dialogueP;
    Dialogue m_Dialogue;
    public GameObject _bag;
    public bool alreayTalk;

    CheckBag _checkBag;
    void Start()
    {
        m_Dialogue = dialogueP.GetComponent<Dialogue>();
        _checkBag = _bag.GetComponent<CheckBag>();
        door.SetActive(false);
        alreayTalk = false;
    }

    // Update is called once per frame
    void Update()
    {

        CheckIntro();
    }

    void CheckIntro()
    {
        if(m_count >=5 && m_Dialogue.d_finish == true && alreayTalk == false)
        {
            door.SetActive(true);
           
            m_Dialogue.npc.SetActive(false);
            m_Dialogue.ShowBG(3);
            dialogueP.SetActive(true);
            UIKit.ShowPanel<DialoguePanel>();
            m_Dialogue.ink_file = ink_file;
            m_Dialogue.story = new Story(ink_file.text);
            alreayTalk = true;
        }
        
    }
}
