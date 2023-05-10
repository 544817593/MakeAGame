using System.Collections.Generic;
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
            // todo 再添加棋子
            
            Debug.Log("[TODO] add piece");
        }
    }
}