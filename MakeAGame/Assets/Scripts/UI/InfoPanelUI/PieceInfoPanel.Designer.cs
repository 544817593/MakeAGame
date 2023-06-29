using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PieceInfo
{
	// Generate Id:3ea6498f-22c7-464d-8274-6a07d7d0b638
	public partial class PieceInfoPanel
	{
		public const string Name = "PieceInfoPanel";
		
		[SerializeField]
		public UnityEngine.UI.Image PieceInfo;
		[SerializeField]
		public UnityEngine.UI.Image RarityImage;
		[SerializeField]
		public TMPro.TextMeshProUGUI CharName;
		[SerializeField]
		public TMPro.TextMeshProUGUI MoveSpeedData;
		[SerializeField]
		public TMPro.TextMeshProUGUI HPData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AtkDmgData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AtkSpeedData;
		[SerializeField]
		public TMPro.TextMeshProUGUI DefenseData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AccuracyData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AtkRangeData;
		[SerializeField]
		public TMPro.TextMeshProUGUI FeatureData;
		[SerializeField]
		public TMPro.TextMeshProUGUI StatusData;
		[SerializeField]
		public TMPro.TextMeshProUGUI AddtionalPropertyData;
		
		private PieceInfoPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PieceInfo = null;
			RarityImage = null;
			CharName = null;
			MoveSpeedData = null;
			HPData = null;
			AtkDmgData = null;
			AtkSpeedData = null;
			DefenseData = null;
			AccuracyData = null;
			AtkRangeData = null;
			FeatureData = null;
			StatusData = null;
			AddtionalPropertyData = null;
			
			mData = null;
		}
		
		public PieceInfoPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PieceInfoPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PieceInfoPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
