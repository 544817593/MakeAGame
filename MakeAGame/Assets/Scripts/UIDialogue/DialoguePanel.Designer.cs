using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace DialogueUI
{
	// Generate Id:9c912580-ff2b-46d2-bf86-b68e14f03aa8
	public partial class DialoguePanel
	{
		public const string Name = "DialoguePanel";
		
		[SerializeField]
		public UnityEngine.UI.Image NPC;
		[SerializeField]
		public UnityEngine.UI.Image Player;
		[SerializeField]
		public RectTransform dialogue;
		[SerializeField]
		public Choice choice;
		[SerializeField]
		public UnityEngine.UI.Image DialogueBg;
		
		private DialoguePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			NPC = null;
			Player = null;
			dialogue = null;
			choice = null;
			DialogueBg = null;
			
			mData = null;
		}
		
		public DialoguePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		DialoguePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new DialoguePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
