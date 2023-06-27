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
			// TODO 需要所有feature的对应中文名
			foreach (FeatureEnum feature in piece.features.Value)
			{
				if(feature != FeatureEnum.None)
				{
                    featureText += feature.ToString() + " ";
                }
			}
			if (featureText != "")
			{
				FeatureData.text = featureText;
			}
			string statusText = "";
			foreach(BuffType status in piece.listBuffs)
			{
				statusText += status.ToString() + " ";
			}
			if(statusText != "")
			{
				StatusData.text = statusText;
			}
			// TODO 额外属性的获取位置？
			string additionalPropertyText = "";
			foreach(FeatureEnum addiProp in piece.card.so.specialFeatures)
			{
				if(addiProp != FeatureEnum.None)
				{
                    additionalPropertyText += addiProp.ToString() + " ";
                }
            }
			if(additionalPropertyText != "")
			{
                AddtionalPropertyData.text = additionalPropertyText;
            }
        }

	}
}
