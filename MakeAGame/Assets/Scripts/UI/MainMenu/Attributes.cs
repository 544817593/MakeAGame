using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Attributes : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text_P;

    TotalPoint totalPoint;
    string currentText;
    int currentP; // current attributes point assigned
    int strength; // set the strength point, can be pass to the player manager 
    int spirit; // set teh spirit point, can be pass to the player manager 
    int skill; // set teh skill point, can be pass to the player manager 
    int con; // set teh con point, can be pass to the player manager 
    int charm; // set teh charm point, can be pass to the player manager 

    void Start()
    {
        currentP = 1;
        GameObject theTotalPoint = GameObject.Find("TotalPoint");
        totalPoint = theTotalPoint.GetComponent<TotalPoint>();
        currentText = text_P.text;
    }

    // Update is called once per frame
    void Update()
    {
        ChangePoint();
    }

    void ChangePoint()
    {
        text_P.text = currentP + currentText;
    }

    public void AddPoint()
    {
        if (currentP >= 1 && currentP <= 9 && totalPoint.total_Point > 0)
        {
            currentP++;

            totalPoint.total_Point--;
        }
    }
    public void MinusPoint()
    {

        if (currentP > 1 && currentP <= 10)
        {
            currentP--;

            totalPoint.total_Point++;
        }
    }
    public void ResetPoint()
    {
        totalPoint.total_Point = 5;
        currentP = 1;
    }
}
