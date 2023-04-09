using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TotalPoint : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text_totalP; 
   // private GameObject m_Message;


    public int total_Point; // Point can be used to assign attributes；
    string currentTotalText; // store current text 
    // Start is called before the first frame update
    void Start()
    {
        total_Point = 5;
        currentTotalText = text_totalP.text;
    }

    // Update is called once per frame
    void Update()
    {
        ShowTotalPoint();
    }

    void ShowTotalPoint()
    {
        if(total_Point >=0)
        {
            text_totalP.text = currentTotalText + total_Point;
        }
    }
}
