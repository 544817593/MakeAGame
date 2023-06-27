using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Game;
using cfg.GameData;

namespace PieceInfo
{
	public class PieceInfoPanelData : UIPanelData
	{
	}
	public partial class PieceInfoPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PieceInfoPanelData ?? new PieceInfoPanelData();
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

		public void LoadPieceData(ViewPiece piece)
		{
			CharName.text = $"{piece.card.charaName}";
            RarityImage.sprite = Extensions.GetRaritySprite(piece.rarity);
            MoveSpeedData.text = $"{piece.moveSpeed}";
			HPData.text = $"{piece.hp} / {piece.maxHp}";
			AtkDmgData.text = $"{piece.atkDmg}";
			AtkSpeedData.text = $"{piece.atkSpeed}";
			DefenseData.text = $"{piece.defense}";
			AccuracyData.text = $"{piece.accuracy}";
            AtkRangeData.text = $"{piece.atkRange}";

			string featureText = "";
			foreach (FeatureEnum feature in piece.features.Value)
			{
				if(feature != FeatureEnum.None)
				{
                    SOFeature featureSO = IdToSO.FindFeatureSOByEnum(feature);
                    featureText += featureSO.featureName + "   ";
                }
			}
			if (featureText != "")
			{
				FeatureData.text = featureText;
			}

			string statusText = "";
			foreach(BuffType status in piece.listBuffs)
			{
				statusText += Extensions.buffToName[status] + "   ";
			}
			if(statusText != "")
			{
				StatusData.text = statusText;
			}

			string additionalPropertyText = "";
			foreach(FeatureEnum addiProp in piece.card.so.specialFeatures)
			{
				if(addiProp != FeatureEnum.None)
				{
                    SOFeature featureSO = IdToSO.FindFeatureSOByEnum(addiProp);
                    additionalPropertyText += $"{featureSO.featureName}: {featureSO.featureDesc} \n";
                }
            }
			if(additionalPropertyText != "")
			{
                AddtionalPropertyData.text = additionalPropertyText;
            }
        }

	}
}
