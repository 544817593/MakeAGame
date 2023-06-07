namespace Game
{
    // 道具相关事件
    public struct UseItemEvent
    {
        public Item item;
        public ViewCard viewCard;
        public ViewBagCard viewBagCard;
        public FeatureEnum soFeature;
        public Monster monster;
        public ViewPiece viewPiece;
    }
}