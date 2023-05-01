using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace DialogueUI
{
	// Generate Id:29201322-fe05-4153-aae7-943ea97e62e5
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
