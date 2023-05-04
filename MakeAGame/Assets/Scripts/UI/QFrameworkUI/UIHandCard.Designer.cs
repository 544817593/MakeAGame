using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Game
{
	// Generate Id:15d7bd2d-8f5b-44a7-843e-74407d790bc5
	public partial class UIHandCard
	{
		public const string Name = "UIHandCard";
		
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
