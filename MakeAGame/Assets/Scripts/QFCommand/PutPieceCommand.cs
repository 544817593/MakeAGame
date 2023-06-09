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
            if (ItemController.Instance.sanCostProtection == 0)
            {
                CostSanEvent e = new CostSanEvent() {sanCost = (int) viewCard.card.sanCost};
                this.SendEvent<CostSanEvent>(e);
                UIKit.GetPanel<UIHandCard>().OnSanChange(-e.sanCost);
            }
            else
            {
                ItemController.Instance.sanCostProtection -= 1;
            }
            // 永久增加卡牌的混沌值消耗
            if (ItemController.Instance.sanCostIncreaseProtection == 0)
            {
                viewCard.card.sanCost += (int)(0.2 * viewCard.card.sanCost);
            }
            else
            {
                ItemController.Instance.sanCostIncreaseProtection -= 1;
            }
            
            // 再添加棋子
            this.GetSystem<IPieceSystem>().AddPieceFriend(viewCard.card, grids);

            // 直接调用，会导致棋子事件尚未注册时就调用CheckAllPieceAtkRange，棋子收不到进入战斗的事件
            // todo 本质原因是start的调用会在instantiate后一帧，监听事件的时机可能需要统一调整
            // OnPutPieceFinish();
            
            // 延时一帧
            this.GetSystem<IUpdateSystem>().DelayExecute(OnPutPieceFinish, true, 1);
        }

        void OnPutPieceFinish()
        {
            InitFeatures();
            // 落地检查攻击范围
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