using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public BoxCollider2D m_Col;
    public GameObject dialogueP;
    public Texture2D defaultCursor;
    public Texture2D doorCursor;
    Dialogue m_Dialogue;
    public GameObject _bag;
    
    CheckBag _checkBag;
    // Start is called before the first frame update
    void Start()
    {
        m_Dialogue = dialogueP.GetComponent<Dialogue>();
        _checkBag = _bag.GetComponent<CheckBag>();
        m_Col.enabled = false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void ActiveDoor()
    {
        if (m_Dialogue.d_finish == true && _checkBag.isOpen == false)
        {
            m_Col.enabled = true;
        }
        else { m_Col.enabled = false; }
    }
    private void OnMouseDown()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
       // LoadScene();
       

    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(doorCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    //public void LoadScene()
    //{
    //    Loading.instance.LoadingScene(GameManager.Instance.currSceneName);
    //    // �˴�������ڽ��棬�ټ���DoNotDestroy�����ڵ�����
    //    GameManager.Instance.mapMan.gameObject.SetActive(true);
    //    GameManager.Instance.pieceMan.gameObject.SetActive(true);

    //}
    // Update is called once per frame
    void Update()
    {
        ActiveDoor();
    }
}
