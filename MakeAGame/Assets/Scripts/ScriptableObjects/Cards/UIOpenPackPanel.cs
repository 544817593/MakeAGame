using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using QFramework;

namespace PackOpen
{
	public class UIOpenPackPanelData : UIPanelData
	{
		public int count=0;
		public int Number_Of_Packs=1;
		
		
	}
	public partial class UIOpenPackPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIOpenPackPanelData ?? new UIOpenPackPanelData();
			// please add init code here
			PackOpen.PackModel.finish.RegisterWithInitValue(finish =>
			{

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
			StartCoroutine(OpenCard());
		}
		IEnumerator OpenCard()
		{
			if (mData.Number_Of_Packs > 0)
			{
				GameObject tempCard = new GameObject("temp");
				tempCard.transform.SetParent(CardPosition.transform, false);
				mData.count = 5;

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

			mData.Number_Of_Packs--;

		}
	}
}
