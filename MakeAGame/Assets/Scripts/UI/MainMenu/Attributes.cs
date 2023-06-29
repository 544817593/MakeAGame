using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game;

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
    int stamina; // set teh con point, can be pass to the player manager 
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
        GameManager.Instance.soundMan.Play_Add();
        if (currentP >= 1 && currentP <= 9 && totalPoint.total_Point > 0)
        {
            currentP++;

            totalPoint.total_Point--;
        }
    }
    public void MinusPoint()
    {
        GameManager.Instance.soundMan.Play_Minus();
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

    public void setStrength()
    {
        strength = currentP;
        
    }
    public int getStrength()
    {
        return strength;
    }
    public void setSpirit()
    {
        spirit = currentP;
        PlayerManager.Instance.player.SetStartStats(PlayerStatsEnum.Spirit, spirit);

    }
    public int getSpirit()
    {
        return spirit;
    }
    public void setSkill()
    {
        skill = currentP;
        PlayerManager.Instance.player.SetStartStats(PlayerStatsEnum.Skill, skill);
    }
    public int getSkill()
    {
        return skill;
    }
    public void setStamina()
    {
        stamina = currentP;
        PlayerManager.Instance.player.SetStartStats(PlayerStatsEnum.Stamina, stamina);
    }
    public int getCon()
    {
        return stamina;
    }
    public void setCharm()
    {
        charm = currentP;
        PlayerManager.Instance.player.SetStartStats(PlayerStatsEnum.Charisma, charm);
    }
    public int getCharm()
    {
        return charm;
    }
    public void setState()
    {

    }
}
