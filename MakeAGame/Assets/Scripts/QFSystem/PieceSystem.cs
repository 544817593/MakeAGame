using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPieceSystem : ISystem
    {
        List<ViewPiece> pieceFriendList { get; }    // 友方棋子

        public bool AddPieceFriend(Card card, List<BoxGrid> grids);

        // 唤出方向轮盘显示在棋子下
        public void ShowDirectionWheel(ViewPieceBase viewPB);
        // 改变棋子方向
        public void ChangePieceDirection();
    }
    
    public class PieceSystem: AbstractSystem, IPieceSystem
    {
        private ViewDirectionWheel viewDirectionWheel;
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
            viewPiece.InitState();
            

            // 数值变化
            pieceFriendList.Add(viewPiece);

            // 通知UI变化   // 通过事件注册
            // OnAddCardTest.Trigger(handCardList.Count - 1);

            return true;
        }

        private ViewPieceBase crtSelectedPiece;
        public void ShowDirectionWheel(ViewPieceBase viewPB)
        {
            Debug.Log($"show wheel at {viewPB.ToString()}");

            crtSelectedPiece = viewPB;
            
            CheckDirectionWheelExist();
            
            // Debug.Log(Camera.main.WorldToScreenPoint(viewPB.transform.position));
            // Debug.Log(Extensions.WorldToUIPos(viewPB.transform.position));
            var viewPiece = viewPB as ViewPiece;
            viewDirectionWheel.SetValidDirections(viewPiece.card.moveDirections);
            viewDirectionWheel.gameObject.transform.localPosition = Extensions.WorldToUIPos(viewPB.transform.position);
            viewDirectionWheel.gameObject.SetActive(true);
        }

        // private string directionWheelResPath = "Prefabs/DirectionWheel";
        void CheckDirectionWheelExist()
        {
            if (viewDirectionWheel == null)
            {
                var handcardPanel = UIKit.GetPanel<UIHandCard>();
                viewDirectionWheel = handcardPanel.viewDirectionWheel;

                // var prefab = Resources.Load(directionWheelResPath);
                // var go = (GameObject) GameObject.Instantiate(prefab);
                // viewDirectionWheel = new ViewDirectionWheel(go);
                // viewDirectionWheel.gameObject.SetActive(false);
            }
        }

        public void ChangePieceDirection()
        {
            Debug.Log($"ChangePieceDirection ret1: {crtSelectedPiece == null} ret2: {viewDirectionWheel.crtDirection == PieceMoveDirection.None}");

            // 无效操作
            if (crtSelectedPiece == null || viewDirectionWheel.crtDirection == PieceMoveDirection.None)
            {
            }
            else
            {
                var newDirection = viewDirectionWheel.crtDirection;
                crtSelectedPiece.direction = newDirection;
                Debug.Log($"change piece direction to {newDirection}");   
            }

            viewDirectionWheel.gameObject.SetActive(false);
            viewDirectionWheel.crtDirection = PieceMoveDirection.None;
            crtSelectedPiece = null;
        }
        
        
    }
}