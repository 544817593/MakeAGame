using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QFramework;
using BagUI;

namespace PackOpen
{
	public class ShowCard : MonoBehaviour
	{
		public UIOpenPackPanel m_ui;
		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		/// <summary>
		/// 保存新开的卡包到牌库
		/// </summary>
		public void SaveNewCard(Game.Card cardData)
		{
			UIKit.GetPanel<BagUIPanel>().AddCard(cardData);
			
		}
		/// <summary>
		/// 显示翻开的卡牌
		/// </summary>
		public void ShowNewCard()
		{
			GameManager.Instance.soundMan.Play_Flip();
			int newCardId ;
			
			if (m_ui.blueSecure ==true)
			{
				newCardId = PackProbability.DrawCard(0, false, true);
				m_ui.blueDrawn = true;
				m_ui.blueSecure = false;
			}
			else if (m_ui.count == 1 && !m_ui.greenDrawn)
			{
				newCardId = PackProbability.DrawCard(0, true);
			}
			else
			{
				newCardId = PackProbability.DrawCard(0); // 暂时设为0（卡包为基础包）
				if (IdToSO.FindCardSOByID(newCardId).rarity == RarityEnum.Green) m_ui.greenDrawn = true;
				if (IdToSO.FindCardSOByID(newCardId).rarity == RarityEnum.Blue)
				{
					m_ui.blueDrawn = true;
					m_ui.blueSecure = false;
				}
			}

			if (( GameSceneManager.Instance.GetCurrentSceneName() == "Intro") )
			{
				if (m_ui.count == 3)
				{
					newCardId = 1;
				}
				else
				{
					newCardId = 4;
				}
				m_ui.greenDrawn = false;
				m_ui.blueDrawn = false;
				m_ui.blueSecure = false;
			}


			//Debug.Log("Card drawn with ID: " + newCardId);


			Game.ISpawnSystem spawnSystem = Game.GameEntry.Interface.GetSystem<Game.ISpawnSystem>();
			
			GameObject new_Card;

			spawnSystem.SpawnCard(newCardId);
			new_Card = spawnSystem.GetLastSpawnedCard();
				//new_Card = Instantiate(Resources.Load("Prefabs/CardItem"), transform.position, Quaternion.identity) as GameObject;
			
			Game.Card createCard = new_Card.GetComponent<Game.ViewBagCard>().card;
			new_Card.transform.SetParent(GameObject.Find("Cardtemp")?.transform, false);
			new_Card.transform.position = transform.position;
			new_Card.GetComponent<Game.ViewBagCard>().normalScale = 0.15f;
			new_Card.GetComponent<Game.ViewBagCard>().largeScale = 0.2f;

			
			//createCard.CreateCard(newCardId);
			//Game.Card card_base = new_Card.GetComponent<Game.Card>();
			//card_base(newCardId);




			//gameObject.SetActive(false);


			m_ui.count--;

			gameObject.SetActive(false);
			SaveNewCard(createCard);
			//if (m_ui.count <= 0)
			//{
			//	if (m_ui.blueDrawn == false) m_ui.blueSecure = true;
			//	Invoke("OpenNewPack", 3f);

			//}
			//Invoke("OpenNewPack", 3f);
			if (m_ui.count <=0)
			{
				if (m_ui.blueDrawn == false) m_ui.blueSecure = true;
				Invoke("OpenNewPack", 3f);

				}
		}
		/// <summary>
		/// 如果有多的卡包，开启新卡包
		/// </summary>
		public void AddRewardCard()
        {
			int newCardId = 9;
			Game.ISpawnSystem spawnSystem = Game.GameEntry.Interface.GetSystem<Game.ISpawnSystem>();

			GameObject new_Card;

			spawnSystem.SpawnCard(newCardId);
			new_Card = spawnSystem.GetLastSpawnedCard();
			//new_Card = Instantiate(Resources.Load("Prefabs/CardItem"), transform.position, Quaternion.identity) as GameObject;

			Game.Card createCard = new_Card.GetComponent<Game.ViewBagCard>().card;
			new_Card.transform.SetParent(GameObject.Find("Cardtemp")?.transform, false);
			new_Card.transform.position = transform.position;
			new_Card.GetComponent<Game.ViewBagCard>().normalScale = 0.15f;
			new_Card.GetComponent<Game.ViewBagCard>().largeScale = 0.2f;
			gameObject.SetActive(false);
			SaveNewCard(createCard);

		}
		public void OpenNewPack()
		{


			Destroy(GameObject.Find("Cardtemp"));
			m_ui.Pack.Show();
			m_ui.Pack.BtnOpen.interactable = true;
			m_ui.openFinish = false;
			if (m_ui.Number_Of_Packs == 0)
			{
				m_ui.openFinish = true;
				m_ui.Hide();
				PackModel.finish.RegisterWithInitValue(finish =>
				{
					finish = m_ui.openFinish;
				});
			}
			

		}
	}
}

