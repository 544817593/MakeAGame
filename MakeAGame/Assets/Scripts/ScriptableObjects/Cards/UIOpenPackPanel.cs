using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using QFramework;
using UnityEngine.EventSystems;

namespace PackOpen
{
	public class UIOpenPackPanelData : UIPanelData
	{
		
		
	}
	public partial class UIOpenPackPanel : UIPanel 
	{
		public int count;
		public int Number_Of_Packs;
		public bool openFinish ;
		public bool greenDrawn; // Whether green card has been drawn in the current pack
		public bool blueDrawn; // Whether blue card has been drawn in the current pack
		public bool blueSecure; // Whether blue secure is active
		public List<Transform> card_t;
		public List<Transform> point_t;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIOpenPackPanelData ?? new UIOpenPackPanelData();
			// please add init code here
			openFinish = false;
			Number_Of_Packs = 1;
			count = 0;
			card_t = new List<Transform>();
			foreach(Transform card in CardPosition.GetComponentsInChildren<Transform>())
            {
				card_t.Add(card);
				card.Hide();
            }
			foreach (Transform point in Point.GetComponentsInChildren<Transform>())
			{
				point_t.Add(point);
			}

			
			PackModel.finish.RegisterWithInitValue(finish =>
			{
				finish = openFinish;
			}).UnRegisterWhenGameObjectDestroyed(gameObject);

			Pack.BtnOpen.onClick.AddListener(() => 
			{
				GameManager.Instance.soundMan.Play_Open_pack();
				OpenPack();
			});

			

			
			
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
		void OpenPack()
		{
			Pack.BtnOpen.interactable = false;
			// Reset green and blue drawn variables at each opening of card packs
			//Pack.CardPosition1.Show();
			greenDrawn = false;
			blueDrawn = false;
			StartCoroutine(OpenCard());
		}
		IEnumerator OpenCard()
		{
			if (Number_Of_Packs > 0)
			{
				GameObject tempCard = new GameObject("Cardtemp");
				tempCard.transform.SetParent(CardPosition.transform, false);
				count = 5;
				
				for (int i =0; i < card_t.Count; i++)
                {
					yield return new WaitForSeconds(0.5f);
					card_t[i].position = Pack.transform.position;
					card_t[i].DOScale(0.5f, 0);
					Cursor.lockState = CursorLockMode.Locked;
					card_t[i].Show();
					card_t[i].DOMove(point_t[i].position, 2);
					card_t[i].DOScale(1f, 2);
					
					
				}
				Cursor.lockState = CursorLockMode.None;
				//yield return new WaitForSeconds(0.5f);
				//CardPosition1.Show();
				//yield return new WaitForSeconds(0.5f);
				//CardPosition2.Show();
				//yield return new WaitForSeconds(0.5f);
				//CardPosition3.Show();
				//yield return new WaitForSeconds(0.5f);
				//CardPosition4.Show();
				//yield return new WaitForSeconds(0.5f);
				//CardPosition5.Show();
				Pack.Hide();

			}

			Number_Of_Packs--;
			
		}

		

		
	}
}
