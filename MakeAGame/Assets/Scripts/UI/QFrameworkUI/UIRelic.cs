using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using UnityEditor;

namespace Game
{
	public class UIRelicData : UIPanelData
	{
	}
	public partial class UIRelic : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIRelicData ?? new UIRelicData();
			// please add init code here
			
			var relicSystem = GameEntry.Interface.GetSystem<IRelicSystem>();
			relicSystem.ui = this;
			RefreshData(relicSystem.GetRelics());

			Tooltip.gameObject.SetActive(false);
			tmpTooltip = Tooltip.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		}

		private TextMeshProUGUI tmpTooltip;
		
		private List<ViewRelic> viewRelics = new List<ViewRelic>();

		public void RefreshData(List<RelicBase> relicData)
		{
			foreach (var data in relicData)
			{
				AddRelic(data);
			}
		}
		
		public void AddRelic(RelicBase relic)
		{
			var newRelic = new ViewRelic(ContentPanel.transform);
			newRelic.transform.SetParent(ContentPanel.transform);
			newRelic.OnFocus = ShowTooltip;
			newRelic.OnUnfocus = HideTooltip;
			newRelic.InitWithData(relic);
			
			viewRelics.Add(newRelic);
		}

		void ShowTooltip(ViewRelic viewRelic)
		{
			string text = String.Empty;
			text += viewRelic.relicData.so.relicName + "\n";
			text += viewRelic.relicData.so.desc + "\n";
			string effectDesc = viewRelic.relicData.so.effectDesc;
			for (int i = 0; i < viewRelic.relicData.crtParams.Count; i++)
			{
				float param = viewRelic.relicData.crtParams[i];
				effectDesc = effectDesc.Replace("{" + i.ToString() + "}", param.ToString());
			}
			text += effectDesc;

			tmpTooltip.text = text;
			Tooltip.gameObject.SetActive(true);
		}

		void HideTooltip()
		{
			Tooltip.gameObject.SetActive(false);
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
	}
}
