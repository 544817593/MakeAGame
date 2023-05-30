using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace BagUI
{
	// Generate Id:8828e7d5-d256-4295-b9da-c5e4fcf14f4f
	public partial class BagUIPanel
	{
		public const string Name = "BagUIPanel";
		
		[SerializeField]
		public RectTransform Ref;
		[SerializeField]
		public UnityEngine.UI.Button Card;
		[SerializeField]
		public UnityEngine.UI.Button Item;
		[SerializeField]
		public UnityEngine.UI.Button Skill;
		[SerializeField]
		public UnityEngine.UI.Button Close;
		[SerializeField]
		public RectTransform SlotPosition;
		[SerializeField]
		public UnityEngine.UI.Image CardDescribtion;
		[SerializeField]
		public UnityEngine.UI.Image Image;
		[SerializeField]
		public TMPro.TextMeshProUGUI CharName;
		[SerializeField]
		public TMPro.TextMeshProUGUI SizeData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AtkDmgData;
		[SerializeField]
		public TMPro.TextMeshProUGUI MoveDirData;
		[SerializeField]
		public TMPro.TextMeshProUGUI MoveSpeedData;
		[SerializeField]
		public TMPro.TextMeshProUGUI HPData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AtkSpeedData;
		[SerializeField]
		public TMPro.TextMeshProUGUI CDTimeData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AtkRangeData;
		[SerializeField]
		public TMPro.TextMeshProUGUI DeathDescTextData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AddtionalPropertyData;
		[SerializeField]
		public UnityEngine.UI.Button BtnNextPage;
		[SerializeField]
		public UnityEngine.UI.Button BtnPrePage;
		[SerializeField]
		public TMPro.TextMeshProUGUI TextPageNum;
		
		private BagUIPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Ref = null;
			Card = null;
			Item = null;
			Skill = null;
			Close = null;
			SlotPosition = null;
			CardDescribtion = null;
			Image = null;
			CharName = null;
			SizeData = null;
			AtkDmgData = null;
			MoveDirData = null;
			MoveSpeedData = null;
			HPData = null;
			AtkSpeedData = null;
			CDTimeData = null;
			AtkRangeData = null;
			DeathDescTextData = null;
			AddtionalPropertyData = null;
			BtnNextPage = null;
			BtnPrePage = null;
			TextPageNum = null;
			
			mData = null;
		}
		
		public BagUIPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		BagUIPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new BagUIPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
