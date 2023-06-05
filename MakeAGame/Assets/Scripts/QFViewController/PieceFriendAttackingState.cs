using UnityEngine;

namespace Game
{
    public class PieceFriendAttackingState : PieceState
    {
        private float timer;    // 计时器
        private float atkDur;  // 攻击速度（秒/每行动）
        private ViewPiece viewPieceFriend;
        
        public PieceFriendAttackingState(ViewPieceBase view): base(view)
        {
            stateEnum = PieceStateEnum.Attacking;
            
            viewPieceFriend = view as ViewPiece;
        }
        
        public override void EnterState()
        {
            ResetAttackSpeed();
        }

        void ResetAttackSpeed()
        {
            atkDur = viewPieceFriend.atkSpeed * viewPieceFriend.crtTimeMultiplier;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            
            viewPieceFriend.actionBar.SetBarFillAmount((atkDur - timer) / atkDur);
            
            if (timer > atkDur)
            {
                viewPieceFriend.Attack();

                timer = 0;
                ResetAttackSpeed();
            }
        }
        

        public override void ExitState()
        {
            
        }
    }
}