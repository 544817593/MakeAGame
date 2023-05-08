using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:d76802a5-73c4-48aa-a11e-c609912f7f76
	public partial class UIHandCard
	{
		public const string Name = "UIHandCard";
		
		/// <summary>
		/// 跟随鼠标移动的棋子图标
		/// </summary>
		[SerializeField]
		public RectTransform PieceIcon;
		/// <summary>
		/// 跟随鼠标移动的棋子图标图片
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Image ImgPieceIcon;
		/// <summary>
		/// 卡牌的父节点
		/// </summary>
		[SerializeField]
		public RectTransform CardRoot;
		/// <summary>
		/// 添加手牌测试
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Button ButtonAddCardTest;
		/// <summary>
		/// 打开动画测试
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Button ButtonOpenAnim;
		/// <summary>
		/// 关闭动画测试（主要测试遮罩）
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Button ButtonCloseAnim;
		/// <summary>
		/// 减少手牌测试
		/// </summary>
		[SerializeField]
		public UnityEngine.UI.Button ButtonSubCardTest;
		
		private UIHandCardData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PieceIcon = null;
			ImgPieceIcon = null;
			CardRoot = null;
			ButtonAddCardTest = null;
			ButtonOpenAnim = null;
			ButtonCloseAnim = null;
			ButtonSubCardTest = null;
			
			mData = null;
		}
		
		public UIHandCardData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHandCardData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHandCardData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
