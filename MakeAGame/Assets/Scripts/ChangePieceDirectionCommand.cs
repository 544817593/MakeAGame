using QFramework;

namespace Game
{
    /// <summary>
    /// 改变棋子方向
    /// </summary>
    public class ChangePieceDirectionCommand: AbstractCommand
    {
        private PieceMoveDirection direction;

        public ChangePieceDirectionCommand()
        {
        }
        
        protected override void OnExecute()
        {
            var pieceSystem = this.GetSystem<IPieceSystem>();
            pieceSystem.ChangePieceDirection();
        }
    }
}