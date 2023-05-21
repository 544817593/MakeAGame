using UnityEngine;

namespace Game
{
    public class PieceFriendMovingState : PieceState
    {
        private float timer;    // 计时器
        private float moveDur;  // 移动速度（秒/每行动）
        private ViewPiece viewPieceFriend;
        
        public PieceFriendMovingState(ViewPieceBase view): base(view)
        {
            stateEnum = PieceStateEnum.Moving;
            
            viewPieceFriend = view as ViewPiece;
        }
        
        public override void EnterState()
        {
            ResetMoveSpeed();
        }

        void ResetMoveSpeed()
        {
            moveDur = viewPieceFriend.card.moveSpd * viewPieceFriend.crtTimeMultiplier;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer > moveDur)
            {
                viewPieceFriend.Move();

                timer = 0;
                ResetMoveSpeed();
            }
        }
        

        public override void ExitState()
        {
            
        }
    }
}