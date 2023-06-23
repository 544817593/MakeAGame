using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using BagUI;

public class CheckBag : MonoBehaviour
{
    public static CheckBag instance;
    public CapsuleCollider2D m_Col;
    public GameObject dialogueP;
    public Texture2D defaultCursor;
    public Texture2D checkCursor;
    Dialogue m_Dialogue;
    public bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
      
       
        isOpen = false;
        m_Dialogue = dialogueP.GetComponent<Dialogue>();
        m_Col.enabled = false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void ActiveCheck()
    {
        if ( m_Dialogue.d_finish == true && isOpen ==false)
        {
            m_Col.enabled = true;
        }
        else { m_Col.enabled = false; }
    }
    private void OnMouseDown()
    {
        GameManager.Instance.soundMan.Play_Open_Bag();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        OpenBag();


    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(checkCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void OpenBag()
    {
       
        UIKit.ShowPanel<BagUIPanel>();
       // GameManager.Instance.StartBagMan();
        isOpen = true;
       
    }
    public void SetIsOpenFalse()
    {
        isOpen = false;
    }
    // Update is called once per frame
    void Update()
    {
        ActiveCheck();
    }
}
