using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelChange
{
    /// <summary>
    /// 关闭传入的panel，根据点击的按钮跳转到对应panel
    /// </summary>
    /// <param name="currentPanel"></param>
    /// <param name="button"></param>
    private static bool EnhanceCardsInited = false;
    public static void ChangeShopPanel(UIPanel currentPanel, Button button)
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log($"点击了{button.name}按钮");
            if (button.name == "Buy")
            {
                UIKit.OpenPanel<ShopBuyUI.ShopBuyUI>();
            }
            else if(button.name == "Sell")
            {
                UIKit.OpenPanel<ShopSellUI.ShopSellUI>();
            }
            else if(button.name == "Enhance")
            {
                if(!EnhanceCardsInited) 
                {
                    
                }
                UIKit.OpenPanel<ShopEnhanceUI.ShopEnhanceUI>();
            }
            else if(button.name == "Close")
            {
                currentPanel.CloseSelf();
            }
            
        });
    }
}