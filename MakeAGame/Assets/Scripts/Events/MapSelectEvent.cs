using System.Collections.Generic;

namespace Game
{
    // 地块选取相关事件
    public struct SelectMapStartEvent // 开始选取地块
    {
    }

    public struct SelectMapEndEvent // 结束选取地块
    {
    }

    public struct PutPieceByHandCardEvent // 符合摆放棋子条件时，通知手牌
    {
        public ViewCard viewCard;
        public List<BoxGrid> pieceGrids;
    }
}