using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DialogueUI;
using UnityEngine.UI;
public class CheckControl : MonoBehaviour
{
    
    public CapsuleCollider2D m_Col;
    public GameObject dialogueP;
    public Texture2D defaultCursor;
    public Texture2D newCursor;
    Dialogue m_Dialogue;
    public bool c_finish;
    // Start is called before the first frame update
    void Start()
    {
        m_Dialogue = dialogueP.GetComponent<Dialogue>();
        m_Col.enabled = false;
        c_finish = false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        ActiveNpc();
    }

    void ActiveNpc()
    {
        if(m_Dialogue != null && m_Dialogue.waitForControl)
        {
            m_Col.enabled = true;
        }
        else { m_Col.enabled = false; }
    }
    private void OnMouseDown()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        c_finish = true;
        UIKit.GetPanel<DialoguePanel>().NPC.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        m_Dialogue.ShowBG(3);
        m_Dialogue.npc.SetActive(false);
        UIKit.ShowPanel<DialoguePanel>();


    }
    private void OnMouseEnter()
    {
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

   
}
