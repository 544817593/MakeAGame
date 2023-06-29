using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game
{
    public class PieceEnemyAttackingState : PieceState
    {
        private float timer;    // 计时器
        private float atkDur;  // 攻击速度（秒/每行动）
        private Monster viewPieceEnemy;
        
        public PieceEnemyAttackingState(ViewPieceBase view): base(view)
        {
            stateEnum = PieceStateEnum.Attacking;
            
            viewPieceEnemy = view as Monster;
        }
        
        public override void EnterState()
        {
            ResetAttackSpeed();
        }

        void ResetAttackSpeed()
        {
            atkDur = viewPieceEnemy.data.atkSpeed;
        }

        public override void Update()
        {
            timer += Time.deltaTime * viewPieceEnemy.crtTimeMultiplier;
            
            viewPieceEnemy.actionBar.SetBarFillAmount((atkDur - timer) / atkDur);
            
            if (timer > atkDur)
            {
                viewPieceEnemy.Attack();

                timer = 0;
                ResetAttackSpeed();
            }
        }
        

        public override void ExitState()
        {

        }
    }
}