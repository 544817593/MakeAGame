using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGift : MonoBehaviour
{
    public GameObject m_giftB;
    public bool m_check;
    // Start is called before the first frame update
    void Start()
    {
        m_giftB.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCheck()
    {
        m_check = true;
    }
    
    public void PopGift()
    {
        m_giftB.SetActive(true);
    }

    
}
