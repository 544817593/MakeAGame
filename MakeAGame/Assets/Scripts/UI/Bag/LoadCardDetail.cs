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
    RarityEnum rarity;
    private BagUIPanel m_BagUI;
    string result;
    string feature;
    void Start()
    {
        m_BagUI = UIKit.GetPanel<BagUIPanel>();
        rarity = 0;
    }
    public void ShowDetail(Card Vcard)
    {
        result = null;
        feature = null;
        rarity = Vcard.rarity;
        m_BagUI.Image.sprite =Extensions.GetRaritySprite(rarity) ;
          
        m_BagUI.CharName.text = Vcard.charaName.ToString();
        //m_BagUI.SizeData.text = card..pieceSize.ToString();
        foreach(DirEnum m_d in Vcard.moveDirections)
        {
            result += m_d.ToString() + " ";
        }
        m_BagUI.MoveDirData.text = result;
       
        
        m_BagUI.MoveSpeedData.text = Vcard.moveSpd.ToString();
        m_BagUI.HPData.text = Vcard.hp.ToString();
        m_BagUI.AtkDmgData.text = Vcard.damage.ToString();
        m_BagUI.AtkSpeedData.text = Vcard.atkSpd.ToString();
        //_cdTimeT.text = card.characterInfo.cdTime.ToString();
        m_BagUI.AtkRangeData.text = Vcard.atkRange.ToString();
        m_BagUI.DeathDescTextData.text = Vcard.deathFuncDesc.ToString();
        foreach (FeatureEnum m_f in Vcard.features)
        {
            feature += m_f.ToString() + " ";
        }
        m_BagUI.AddtionalPropertyData.text = feature;




    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
