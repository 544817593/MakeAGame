using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadCardDetail : MonoBehaviour
{
    public Game.Card card;
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI _Name;

    private TextMeshProUGUI _SizeT;
    private TextMeshProUGUI _MoveDirT;
    [SerializeField]
    private TextMeshProUGUI _MoveSpeedT;
    [SerializeField]
    private TextMeshProUGUI _hpT;
    [SerializeField]
    private TextMeshProUGUI _atkDmgT;
    [SerializeField]
    private TextMeshProUGUI _atkSpeedT;
    private TextMeshProUGUI _cdTimeT;
    [SerializeField]
    private TextMeshProUGUI _atkRangeT;
    [SerializeField]
    private TextMeshProUGUI _deathDescT;
    [SerializeField]
    private TextMeshProUGUI _additionalPropertyT;
    [SerializeField]
    private Image m_Image;
    int rarity;
    void Start()
    {
        rarity = 0;
    }
    public void ShowDetail(Game.Card card)
    {
        rarity = card.rarity;
        {
            if (rarity == 0)
            {
                m_Image.sprite = Game.Extensions.GetRaritySprite(0) ;
            }
            else if (rarity == 1)
            {
                m_Image.sprite = Game.Extensions.GetRaritySprite(1);
            }
            else if (rarity == 2)
            {
                m_Image.sprite = Game.Extensions.GetRaritySprite(2);
            }
            else if (rarity == 3)
            {
                m_Image.sprite = Game.Extensions.GetRaritySprite(3);
            }
            else if (rarity == 4)
            {
                m_Image.sprite = Game.Extensions.GetRaritySprite(4);
            }
        }
        _Name.text = card.charaName;
       // _SizeT.text = card.characterInfo.pieceSize.ToString();
        // _MoveDirT.text = card.listMoveDir.ToString();
        _MoveSpeedT.text = card.moveSpd.ToString();
        _hpT.text = card.hp.ToString();
        _atkDmgT.text = card.so.attack.ToString();
        _atkSpeedT.text = card.so.attackSpd.ToString();
        //_cdTimeT.text = card.characterInfo.cdTime.ToString();
        _atkRangeT.text = card.damage.ToString();
        _deathDescT.text = card.deathFuncDesc;

        _additionalPropertyT.text = card.specialFeature.ToString();




    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
