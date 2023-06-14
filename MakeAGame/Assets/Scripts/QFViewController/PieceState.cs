namespace Game
{
    public abstract class PieceState
    {
        protected ViewPieceBase viewPieceBase;
        public PieceStateEnum stateEnum;

        public PieceState(ViewPieceBase view)
        {
            viewPieceBase = view;
        }
        
        /// <summary>
        /// 进入该状态时触发一次
        /// </summary>
        public abstract void EnterState();
        /// <summary>
        /// 一般在真正的update中调用
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// 退出该状态时触发一次
        /// </summary>
        public abstract void ExitState();
    }
}