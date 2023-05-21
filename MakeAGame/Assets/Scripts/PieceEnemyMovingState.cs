using UnityEngine;

namespace Game
{
    public class PieceEnemyMovingState : PieceState
    {
        private float timer;    // 计时器
        private float moveDur;  // 移动速度（秒/每行动）
        private Monster viewPieceEnemy;
        
        public PieceEnemyMovingState(ViewPieceBase view): base(view)
        {
            stateEnum = PieceStateEnum.Moving;
            
            viewPieceEnemy = view as Monster;
        }
        
        public override void EnterState()
        {
            ResetMoveSpeed();
        }

        void ResetMoveSpeed()
        {
            moveDur = viewPieceEnemy.data.moveSpeed * viewPieceEnemy.crtTimeMultiplier;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer > moveDur)
            {
                viewPieceEnemy.Move();

                timer = 0;
                ResetMoveSpeed();
            }
        }
        

        public override void ExitState()
        {
            
        }
    }
}