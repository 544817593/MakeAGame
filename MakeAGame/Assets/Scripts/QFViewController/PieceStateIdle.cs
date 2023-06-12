namespace Game
{
    /// <summary>
    /// 啥也不干，只是为了防止state为空
    /// </summary>
    public class PieceStateIdle: PieceState
    {
        public PieceStateIdle(ViewPieceBase view) : base(view)
        {
            stateEnum = PieceStateEnum.Idle;
        }
        
        public override void EnterState()
        {
            
        }

        public override void Update()
        {

        }

        public override void ExitState()
        {

        }
    }
}