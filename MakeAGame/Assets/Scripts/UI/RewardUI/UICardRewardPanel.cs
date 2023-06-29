using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace CardRewardUI
{
	public class UICardRewardPanelData : UIPanelData
	{
	}
	public partial class UICardRewardPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UICardRewardPanelData ?? new UICardRewardPanelData();
			// please add init code here
			Confirm.onClick.AddListener(()=> ConfirmReward());

		}

        private void Awake()
        {
			AddRewardCard();
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

		public void ConfirmReward()
        {
			GameObject.Find("GameSceneManager")?.transform.GetComponent<Game.SceneFlow>().LoadRoom();
		}
		public void AddRewardCard()
		{
		
			int newCardId = 9;
			Game.ISpawnSystem spawnSystem = Game.GameEntry.Interface.GetSystem<Game.ISpawnSystem>();

			GameObject new_Card;

			spawnSystem.SpawnCard(newCardId);
			new_Card = spawnSystem.GetLastSpawnedCard();
			//new_Card = Instantiate(Resources.Load("Prefabs/CardItem"), transform.position, Quaternion.identity) as GameObject;

			Game.Card createCard = new_Card.GetComponent<Game.ViewBagCard>().card;
			new_Card.transform.SetParent(Card.transform, false);
			new_Card.transform.position = Card.transform.position;
			new_Card.GetComponent<Game.ViewBagCard>().normalScale = 0.15f;
			new_Card.GetComponent<Game.ViewBagCard>().largeScale = 0.2f;

			Card.GetComponent<PackOpen.ShowCard>().SaveNewCard(createCard);

		}
	}
}
