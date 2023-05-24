using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using System.Collections.Generic;


namespace RewardUI
{
	public class RewardUIPanelData : UIPanelData
	{
		public IInventorySystem BagSystem = GameEntry.Interface.GetSystem<IInventorySystem>();
	}
	public partial class RewardUIPanel : UIPanel
	{
		private List<Item> AllItem = new List<Item>();
		private Item RewardItem = null;
		private int Choice = 0;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as RewardUIPanelData ?? new RewardUIPanelData();
			UpdateReward();
			// please add init code here
			ChooseReward();

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

		private void UpdateReward()
        {
			AllItem = mData.BagSystem.GetItemList();
			CoinAmount.text = Random.Range(10, 100).ToString();
			int i = Random.Range(0,AllItem.Count-1);
			RewardItem = AllItem[i];
			Item.GetComponent<Image>().sprite = RewardItem.data.sprite;

		}
		
		private void ChooseReward()
        {
			legacy.onClick.AddListener(() => 
			{
				Choice = 1;
			});
			Coin.onClick.AddListener(() =>
			{
				Choice = 2;
			});
			Item.onClick.AddListener(() =>
			{
				Choice = 3;
			});
			Confirm.onClick.AddListener(() =>
			{
				if(Choice ==3)
                {
					mData.BagSystem.AddItem(RewardItem);					
				}
				LoadScene();

			});
		}
		private void LoadScene()
		{
			StartCoroutine(GameManager.Instance.gameSceneMan.LoadScene("NormalRoom", false));
			StartCoroutine(GameManager.Instance.gameSceneMan.UnloadScene("Combat"));
			

		}
	}
}
