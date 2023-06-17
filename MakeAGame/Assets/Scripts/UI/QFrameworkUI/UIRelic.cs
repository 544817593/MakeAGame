using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
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
			
			Tooltip.gameObject.SetActive(false);
			
			// todo test
			AddRelic();
			AddRelic();
		}

		private List<ViewRelic> viewRelics = new List<ViewRelic>();

		public void AddRelic()
		{
			var newRelic = new ViewRelic(ContentPanel.transform);
			newRelic.transform.SetParent(ContentPanel.transform);
			newRelic.OnFocus = ShowTooltip;
			newRelic.OnUnfocus = HideTooltip;
			// LayoutRebuilder.ForceRebuildLayoutImmediate(ContentPanel.rectTransform);
			viewRelics.Add(newRelic);
		}

		void ShowTooltip(ViewRelic viewRelic)
		{
			// Vector3 tooltipPos = viewRelic.transform.position;
			// tooltipPos.y += 50f;

			// Tooltip.transform.position = tooltipPos;
			// var pos = Tooltip.rectTransform.position;
			// Tooltip.rectTransform.position = pos + new Vector3(0, 10, 0);
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
