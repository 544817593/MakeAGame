using System.Collections.Generic;
using System.Threading;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPieceSystem : ISystem
    {
        List<ViewPiece> pieceFriendList { get; }    // 友方棋子
        List<Monster> pieceEnemyList { get; }    // 敌方棋子

        ViewPiece undead { get; set; } // 亡灵

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
        // 检查传入的id是当前场上的Monster还是ViewPiece
        public bool IsPieceMonster(int pieceId);
        // 根据id获取ViewPiece
        public ViewPiece getViewPieceById(int pieceId);
        // 根据id获取Monster
        public Monster getMonsterById(int pieceId);

    }
    
    public class PieceSystem: AbstractSystem, IPieceSystem
    {
        private ViewDirectionWheel viewDirectionWheel;
        public List<ViewPiece> pieceFriendList { get; } = new List<ViewPiece>();
        public List<Monster> pieceEnemyList { get; } = new List<Monster>();

        public ViewPiece undead { get; set; }

        private ViewPiece lastSpawnedFriend;
        private ViewPiece lastSpawnedInvestigator;
        private Monster lastSpawnedMonster;
        private bool CheckControl = false;

        protected override void OnInit()
        {

            this.RegisterEvent<SelectMapStartEvent>(e => SetPieceCollidersEnable(false));
            this.RegisterEvent<SelectMapEndEvent>(e => SetPieceCollidersEnable(true));
            this.RegisterEvent<CombatVictoryEvent>(e => ClearPieceLists());
            this.RegisterEvent<CombatDefeatEvent>(e => ClearPieceLists());
            this.RegisterEvent<UnloadSceneEvent>(e => ClearPieceLists());
        }

        void SetPieceCollidersEnable(bool isEnable)
        {
            foreach (var vPiece in pieceFriendList)
            {
                vPiece.SetColliderEnable(isEnable);
            }

            foreach (var monster in pieceEnemyList)
            {
                monster.SetColliderEnable(isEnable);
            }
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

            if (crtSelectedPiece == vpb)
            {
                viewDirectionWheel.gameObject.SetActive(false);
                viewDirectionWheel.crtDirection = DirEnum.None;
                crtSelectedPiece = null;
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
            var pieceScreenPos = Camera.main.WorldToScreenPoint(viewPB.transform.position);
            viewDirectionWheel.gameObject.transform.localPosition = Extensions.ScreenToUIPos(pieceScreenPos);
            viewDirectionWheel.gameObject.SetActive(true);
            if (viewPiece.card.charaName == "弗朗西斯·维兰德·瑟斯顿")
            {
                CheckControl = true;
            }
            else { CheckControl = false; }
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
                if (crtSelectedPiece.pieceAnimator != null)
                {
                    crtSelectedPiece.PieceFlip(newDirection);
                }
                crtSelectedPiece.direction = newDirection;
                // 替换当前方向的资源图片
                Sprite curDirection = Resources.Load<Sprite>(ViewDirectionWheel.CurDirectionDict[crtSelectedPiece.direction]);
                crtSelectedPiece.gameObject.transform.Find("CurMoveDirection").GetComponent<SpriteRenderer>().sprite = curDirection;
                Debug.Log($"change piece direction to {newDirection}");
              
            }

            viewDirectionWheel.gameObject.SetActive(false);
            viewDirectionWheel.crtDirection = DirEnum.None;
            crtSelectedPiece = null;
           
            Dialogue dialogue = GameObject.Find("Dialogue")?.GetComponent<Dialogue>();
            if (dialogue != null)
            {
                if (CheckControl == true)
                {
                    dialogue.getControl = true;
                }
                else
                {
                    dialogue.getControl = false;
                }
                dialogue.InGameControl();
            }
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

        public bool IsPieceMonster(int pieceId)
        {
            foreach(Monster monster in pieceEnemyList)
            {
                if (monster.pieceId == pieceId) return true;
            }
            foreach(ViewPiece piece in pieceFriendList)
            {
                if (piece.pieceId == pieceId) return false;
            }
            Debug.Log("当前地图上没有棋子匹配想查找的棋子id");
            return false;
        }

        public ViewPiece getViewPieceById(int pieceId)
        {
            foreach (ViewPiece piece in pieceFriendList)
            {
                if (piece.pieceId == pieceId) return piece;
            }
            Debug.Log($"id: {pieceId}，此id不在友方棋子列表内");
            return null;
        }
        
        public Monster getMonsterById(int pieceId)
        {
            foreach (Monster piece in pieceEnemyList)
            {
                if (piece.pieceId == pieceId) return piece;
            }
            Debug.Log($"id: {pieceId}，此id不在怪物棋子列表内");
            return null;
        }

        private void ClearPieceLists()
        {
            Debug.LogWarning("CLEAR PIECE LIST");
            foreach(ViewPiece piece in pieceFriendList)
            {
                GameObject.Destroy(piece);
            }
            foreach (Monster monster in pieceEnemyList)
            {
                GameObject.Destroy(monster);
            }
            pieceFriendList.Clear();
            pieceEnemyList.Clear();
        }
    }
}