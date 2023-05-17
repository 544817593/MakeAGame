using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using QFramework;
using DialogueUI;

public class NPC : MonoBehaviour
{
    public TextAsset ink_file;
    public GameObject dialogueP;
    public BoxCollider2D m_Col;
    public Texture2D defaultCursor;
    public Texture2D newCursor;
    Dialogue m_Dialogue;
    public GameObject _bag;
  
     CheckBag _checkBag;
   private bool _alreadytalk;
    // Start is called before the first frame update
    void Start()
    {
       
        m_Dialogue = dialogueP.GetComponent<Dialogue>();
        _checkBag = _bag.GetComponent<CheckBag>();
        m_Col.enabled = false;
        _alreadytalk = false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        ActiveNpc();
    }

    void ActiveNpc()
    {
        if (m_Dialogue != null && m_Dialogue.d_finish == true && _checkBag.isOpen ==false)
        {
            m_Col.enabled = true;
        }
        else { m_Col.enabled = false; }
    }

    private void OnMouseDown()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        LoadDialogue();
        _alreadytalk = true;
        
    }
    private void OnMouseEnter()
    {
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void LoadDialogue()
    {
        UIKit.GetPanel<DialoguePanel>().NPC.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        m_Dialogue.npc.SetActive(false);
        m_Dialogue.ShowBG(3);
        dialogueP.SetActive(true);
        UIKit.ShowPanel<DialoguePanel>();
        m_Dialogue.ink_file = ink_file; 
        m_Dialogue.story = new Story(ink_file.text);
        if(_alreadytalk == true)
        {
            m_Dialogue.story.ChoosePathString("New");
        }
       

    }
}
