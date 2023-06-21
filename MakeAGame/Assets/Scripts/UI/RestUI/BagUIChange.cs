using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
public class BagUIChange 
{
    public static void ChangeBagPanel(UIPanel currentPanel, Button button)
    {
        button.onClick.AddListener(() =>
        {
            
            if (button.name == "Card")
            {
                currentPanel.Hide();
                UIKit.OpenPanel<BagUI.BagUIPanel>();
            }
            else if (button.name == "Item")
            {
                currentPanel.Hide();
                UIKit.OpenPanel<ItemUI.ItemUIPanel>();
            }
           

        });
    }
}
