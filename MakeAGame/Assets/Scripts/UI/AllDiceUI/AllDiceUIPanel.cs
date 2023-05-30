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
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as AllDiceUIPanelData ?? new AllDiceUIPanelData();
			// please add init code here

			// Test Data
			PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Charisma, 6);
			PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Spirit, 6);
			PlayerManager.Instance.player.ModifyStats(PlayerStatsEnum.Stamina, 6);

			RoolDice();
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

		private void RoolDice()
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
			
				MakeDecision();
			});
			
        }

		private void MakeDecision()
        {
			
			Debug.Log(PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Charisma));
			if (totalDiceP <= PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Charisma)&& totalDiceP<= PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Spirit)&&totalDiceP <= PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Stamina))
            {
				Debug.Log("Success");
            }
			else
            {
				Debug.Log("Add Debuff");
            }
        }
	}
}
