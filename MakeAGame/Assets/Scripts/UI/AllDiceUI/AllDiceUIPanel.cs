using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Game;

namespace DiceUI
{
	public class AllDiceUIPanelData : UIPanelData
	{
	}
	public partial class AllDiceUIPanel : UIPanel
	{
		int totalDiceP;
		public bool decision =false;
		
		public bool finish = false;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as AllDiceUIPanelData ?? new AllDiceUIPanelData();
			// please add init code here


		
			
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

		public void RoolDice(PlayerStatsEnum playerStatsEnum)
        {
			BtnRollDice.onClick.AddListener(() =>
			{
				List<int> randNumList = new List<int>();
			
				totalDiceP = 0;
				for (int i = 1; i <= 6; ++i)
				{
					int randNum = Random.Range(0, 3);
					randNumList.Add(randNum);
					totalDiceP += randNum;
				}
				int idx = 0;
				foreach (Transform transform in AllDice.GetComponentInChildren<Transform>())
				{
					
					Assert.IsTrue(idx < randNumList.Count);
					transform.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Rest/{randNumList[idx]}");
					idx++;
				}
			
				MakeDecision(playerStatsEnum);

				finish = true;
			});
			
        }

		private void MakeDecision(PlayerStatsEnum playerStatsEnum)
        {
			Debug.Log(PlayerManager.Instance.player.GetStats(playerStatsEnum));
			
			if ( PlayerManager.Instance.player.GetStats(playerStatsEnum) >= 5)
            {
				Debug.Log("Success");
				decision = true;
            }
			else
            {
				Debug.Log("Add Debuff");
				decision = false;
            }
        }
	}
}
