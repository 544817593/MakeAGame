using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using TMPro;

namespace ShopBuyUI
{
	public class ShopBuyUIData : UIPanelData
	{
        public IShopSystem shopSystem = GameEntry.Interface.GetSystem<IShopSystem>();
    }
	public partial class ShopBuyUI : UIPanel
	{
		// TODO 读取玩家当前金币数量，暂时使用hardcode
		public int currentGold = 50;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ShopBuyUIData ?? new ShopBuyUIData();

            // 监听按钮点击，跳转panel
            ShopPanelChange.changeShopPanel(this, Close);
            updateAndShowShopItems();
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
        private void updateAndShowShopItems()
        {
			List<Item> shopItemList = mData.shopSystem.getshopItemList();
			int idx = 0;
			
            Debug.Log($"{this.name}");
			foreach(Transform item in GameObject.Find("ShopGridPanel").GetComponentInChildren<Transform>(includeInactive : true))
			{
                if (idx >= shopItemList.Count) // shopItemList已遍历完就结束
                {
                    return;
                }
				Debug.Log($"idx : {idx}, count : {shopItemList.Count}");
                GameObject curItem = item.gameObject;
				Item itemInList = shopItemList[idx];
				// 读取sprite，花费，数量
				curItem.SetActive(true); 
				curItem.GetComponent<Image>().sprite = itemInList.data.sprite;
				foreach (Transform texts in curItem.GetComponentInChildren<Transform>())
				{
					Debug.Log(texts.gameObject.name);
					if (texts.gameObject.name == "ItemNum")
					{
						texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.amount.ToString();
					}
					else if (texts.gameObject.name == "ItemCost")
					{
						texts.gameObject.GetComponent<TextMeshProUGUI>().text = itemInList.data.buyCost.ToString();
					}

				}
				Debug.Log(item.gameObject.name);
				idx++;
            }
        }
    }
}
