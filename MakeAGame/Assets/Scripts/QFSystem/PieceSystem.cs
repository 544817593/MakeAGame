using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPieceSystem : ISystem
    {
        List<ViewPiece> pieceFriendList { get; }    // 友方棋子

        public bool AddPieceFriend(Card card, List<BoxGrid> grids);
    }
    
    public class PieceSystem: AbstractSystem, IPieceSystem
    {
        public List<ViewPiece> pieceFriendList { get; } = new List<ViewPiece>();

        protected override void OnInit()
        {
            
        }
        
        public bool AddPieceFriend(Card card, List<BoxGrid> grids)
        {
            Debug.Log($"PieceSystem: AddPieceFriend {card.charaName}");
            
            // 棋子实例化，挂载组件，部分初始化
            GameObject pieceGO = this.GetSystem<IPieceGeneratorSystem>().CreatePieceFriend();
            var viewPiece = pieceGO.AddComponent<ViewPiece>();
            // 接收数据，初始化显示
            viewPiece.SetDataWithCard(card);
            // viewCard.InitView(); // 在这里写会先于start执行，不对    // 转由start触发
            viewPiece.SetGrids(grids);
            
            

            // 数值变化
            pieceFriendList.Add(viewPiece);

            // 通知UI变化   // 通过事件注册
            // OnAddCardTest.Trigger(handCardList.Count - 1);

            return true;
        }
    }
}