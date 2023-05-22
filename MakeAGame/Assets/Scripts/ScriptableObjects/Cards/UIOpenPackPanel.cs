using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIOpenPackPanelData ?? new UIOpenPackPanelData();
			// please add init code here
			openFinish = false;
			Number_Of_Packs = 1;
			count = 0;
			PackModel.finish.RegisterWithInitValue(finish =>
			{
				finish = openFinish;
			}).UnRegisterWhenGameObjectDestroyed(gameObject);

			Pack.BtnOpen.onClick.AddListener(() => 
			{
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
				yield return new WaitForSeconds(0.5f);
				CardPosition1.Show();
				yield return new WaitForSeconds(0.5f);
				CardPosition2.Show();
				yield return new WaitForSeconds(0.5f);
				CardPosition3.Show();
				yield return new WaitForSeconds(0.5f);
				CardPosition4.Show();
				yield return new WaitForSeconds(0.5f);
				CardPosition5.Show();
				Pack.Hide();

			}

			Number_Of_Packs--;
			
		}

		

		
	}
}
