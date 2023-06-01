using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPieceSystem : ISystem
    {
        List<ViewPiece> pieceFriendList { get; }    // 友方棋子
        List<Monster> pieceEnemyList { get; }    // 敌方棋子

        public bool AddPieceFriend(Card card, List<BoxGrid> grids);

        public bool AddPieceEnemy(Monster monster, List<BoxGrid> grids);

        public ViewPiece GetLastSpawnedFriend(bool investigator);

        public Monster GetLastSpawnedMonster();

        public void RemovePiece(ViewPieceBase vpb);

        // 唤出方向轮盘显示在棋子下
        public void ShowDirectionWheel(ViewPieceBase viewPB);
        // 改变棋子方向
        public void ChangePieceDirection();
        
        // 获得两个棋子的最小距离（占地多个格子的情况下）
        public int GetPieceDist(ViewPieceBase vpb1, ViewPieceBase vpb2);
    }
    
    public class PieceSystem: AbstractSystem, IPieceSystem
    {
        private ViewDirectionWheel viewDirectionWheel;
        public List<ViewPiece> pieceFriendList { get; } = new List<ViewPiece>();
        public List<Monster> pieceEnemyList { get; } = new List<Monster>();

        private ViewPiece lastSpawnedFriend;
        private ViewPiece lastSpawnedInvestigator;
        private Monster lastSpawnedMonster;

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
            lastSpawnedFriend = viewPiece;
            if (viewPiece.rarity == RarityEnum.Orange) lastSpawnedInvestigator = viewPiece;

            // 通知UI变化   // 通过事件注册
            // OnAddCardTest.Trigger(handCardList.Count - 1);

            return true;
        }
        
        public bool AddPieceEnemy(Monster monster, List<BoxGrid> grids)
        {
            Debug.Log($"PieceSystem: AddPieceEnemy {monster.data.monsterId}");
            
            // // 棋子实例化，挂载组件，部分初始化
            // GameObject pieceGO = this.GetSystem<IPieceGeneratorSystem>().CreatePieceFriend();
            // var viewPiece = pieceGO.AddComponent<ViewPiece>();
            // // 接收数据，初始化显示
            // viewPiece.SetDataWithCard(card);
            // // viewCard.InitView(); // 在这里写会先于start执行，不对    // 转由start触发
            monster.SetGrids(grids);
            monster.InitState();

            // 数值变化
            pieceEnemyList.Add(monster);
            lastSpawnedMonster = monster;

            return true;
        }

        public ViewPiece GetLastSpawnedFriend(bool investigator)
        {
            if (investigator) return lastSpawnedInvestigator;
            return lastSpawnedFriend;
        }

        public Monster GetLastSpawnedMonster()
        {
            return lastSpawnedMonster;
        }
            
        public void RemovePiece(ViewPieceBase vpb)
        {
            if (vpb is ViewPiece vp)
            {
                pieceFriendList.Remove(vp);
            }
            else if (vpb is Monster m)
            {
                pieceEnemyList.Remove(m);
            }
        }

        private ViewPieceBase crtSelectedPiece;
        public void ShowDirectionWheel(ViewPieceBase viewPB)
        {
            Debug.Log($"show wheel at {viewPB.ToString()}");

            crtSelectedPiece = viewPB;
            
            CheckDirectionWheelExist();
            
            var viewPiece = viewPB as ViewPiece;
            viewDirectionWheel.SetValidDirections(viewPiece.dirs);
            viewDirectionWheel.gameObject.transform.localPosition = Extensions.WorldToUIPos(viewPB.transform.position);
            viewDirectionWheel.gameObject.SetActive(true);
        }
        
        void CheckDirectionWheelExist()
        {
            if (viewDirectionWheel == null)
            {
                var handcardPanel = UIKit.GetPanel<UIHandCard>();
                viewDirectionWheel = handcardPanel.viewDirectionWheel;
            }
        }

        public void ChangePieceDirection()
        {
            Debug.Log($"ChangePieceDirection ret1: {crtSelectedPiece == null} ret2: {viewDirectionWheel.crtDirection == DirEnum.None}");

            // 无效操作
            if (crtSelectedPiece == null || viewDirectionWheel.crtDirection == DirEnum.None)
            {
            }
            else
            {
                var newDirection = viewDirectionWheel.crtDirection;
                crtSelectedPiece.direction = newDirection;
                Debug.Log($"change piece direction to {newDirection}");   
            }

            viewDirectionWheel.gameObject.SetActive(false);
            viewDirectionWheel.crtDirection = DirEnum.None;
            crtSelectedPiece = null;
        }

        public int GetPieceDist(ViewPieceBase vpb1, ViewPieceBase vpb2)
        {
            int minDist = 100;
            BoxGrid grid1;
            BoxGrid grid2;
            int tmpDist = minDist;
            for (int i = 0; i < vpb1.pieceGrids.Count; i++)
            {
                grid1 = vpb1.pieceGrids[i];
                for (int j = 0; j < vpb2.pieceGrids.Count; j++)
                {
                    grid2 = vpb2.pieceGrids[j];
                    tmpDist = Mathf.Abs(grid1.row - grid2.row) + Mathf.Abs(grid1.col - grid2.col);
                    if (tmpDist < minDist)
                        minDist = tmpDist;
                }
            }

            return minDist;
        }
    }
}