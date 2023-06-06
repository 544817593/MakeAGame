using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;

namespace Game
{
    public class PutPieceCommand: AbstractCommand
    {
        // 首先，我已经知道棋子可以放置
        // 然后，我需要取到是哪张牌，并由此取到角色信息，并取到棋子信息
        // 最后，我需要取到格子list
        private ViewCard viewCard;
        private List<BoxGrid> grids;

        public PutPieceCommand(ViewCard _viewCard, List<BoxGrid> _grids)
        {
            viewCard = _viewCard;
            grids = _grids;
        }
        
        protected override void OnExecute()
        {
            Debug.Log("PutPieceCommand");
            
            // 先移出手牌
            this.SendCommand(new SubHandCardCommand(viewCard));
            // 扣除混沌值
            UIKit.GetPanel<UIHandCard>().OnSanChange(-(int)viewCard.card.sanCost);
            // todo 再添加棋子
            this.GetSystem<IPieceSystem>().AddPieceFriend(viewCard.card, grids);

            // 直接调用，会导致棋子事件尚未注册时就调用CheckAllPieceAtkRange，棋子收不到进入战斗的事件
            // OnPutPieceFinish();
            
            // todo 先借dotween写一个笨笨的延时
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
                {
                    OnPutPieceFinish();
                })
                .SetDelay(0.01f);
        }

        void OnPutPieceFinish()
        {
            InitFeatures();
            // Debug.Log("[TODO] add piece");
            this.GetSystem<IPieceBattleSystem>().CheckAllPieceAtkRange();
        }

        void InitFeatures()
        {
            ViewPiece piece = this.GetSystem<IPieceSystem>().GetLastSpawnedFriend(false);
            SpecialitiesSpawnCheckEvent e = new SpecialitiesSpawnCheckEvent { piece = piece };
            this.SendEvent(e);
        }
    }
}