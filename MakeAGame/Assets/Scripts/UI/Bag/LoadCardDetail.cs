using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game;
using BagUI;
using QFramework;
public class LoadCardDetail : MonoBehaviour
{

    // Start is called before the first frame update
    int rarity;
    public BagUIPanel m_BagUI;
    void Start()
    {
        rarity = 0;
    }
    public void ShowDetail(Card card)
    {
        rarity = card.rarity;
        m_BagUI.Image.sprite = Game.Extensions.GetRaritySprite(rarity) ;
          
        m_BagUI.CharName.text = card.charaName;
        //m_BagUI.SizeData.text = card..pieceSize.ToString();
        m_BagUI.MoveDirData.text = card.moveDirections.ToString();
        m_BagUI.MoveSpeedData.text = card.moveSpd.ToString();
        m_BagUI.HPData.text = card.hp.ToString();
        m_BagUI.AtkDmgData.text = card.so.attack.ToString();
        m_BagUI.AtkSpeedData.text = card.so.attackSpd.ToString();
        //_cdTimeT.text = card.characterInfo.cdTime.ToString();
        m_BagUI.AtkRangeData.text = card.damage.ToString();
        m_BagUI.DeathDescTextData.text = card.deathFuncDesc;

        m_BagUI.AddtionalPropertyData.text = card.specialFeature.ToString();




    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
