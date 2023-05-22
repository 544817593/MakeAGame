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
        {
            if (rarity == 0)
            {
                m_BagUI.Image.sprite = Game.Extensions.GetRaritySprite(0) ;
            }
            else if (rarity == 1)
            {
                m_BagUI.Image.sprite = Game.Extensions.GetRaritySprite(1);
            }
            else if (rarity == 2)
            {
                m_BagUI.Image.sprite = Game.Extensions.GetRaritySprite(2);
            }
            else if (rarity == 3)
            {
                m_BagUI.Image.sprite = Game.Extensions.GetRaritySprite(3);
            }
            else if (rarity == 4)
            {
                m_BagUI.Image.sprite = Game.Extensions.GetRaritySprite(4);
            }
        }
        m_BagUI.CharName.text = card.charaName;
       // _SizeT.text = card.characterInfo.pieceSize.ToString();
        // _MoveDirT.text = card.listMoveDir.ToString();
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
