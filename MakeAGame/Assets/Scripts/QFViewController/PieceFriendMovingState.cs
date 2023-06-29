using UnityEngine;

namespace Game
{
    public class PieceFriendMovingState : PieceState
    {
        private ViewPiece viewPieceFriend;
        public float movementCooldown; // 移动时间冷却，冷却完毕即可移动，每次移动后重置为怪物移动速度
        private float lastUpdateTime; // 上一次触发Update函数的时间
        private float timeSinceUpdate; // 上一次触发Update函数后经过了多久
        // 从多少秒开始倒计时的，也就是上一次同步movementCooldown时的移动速度;如果移动速度倒计时中途被改变，通过这个调整偏差
        private float recordedMoveSpeed;
        private float extraMultiplier = 1;

        public PieceFriendMovingState(ViewPieceBase view): base(view)
        {
            stateEnum = PieceStateEnum.Moving;
            
            viewPieceFriend = view as ViewPiece;
        }
        
        public override void EnterState()
        {
            lastUpdateTime = Time.time;
            ResetMoveSpeed();
        }

        void ResetMoveSpeed()
        {
            movementCooldown = viewPieceFriend.moveSpeed;
            recordedMoveSpeed = viewPieceFriend.moveSpeed;
            extraMultiplier = 1;
        }

        public override void Update()
        {
            // 如果移动速度中途被改变，进行同步
            if (viewPieceFriend.moveSpeed != recordedMoveSpeed)
            {
                extraMultiplier = viewPieceFriend.moveSpeed / recordedMoveSpeed;
            }

            timeSinceUpdate = Time.time - lastUpdateTime; // 计算两次Update的时间差
            // 根据格子时间倍率减少怪物的移动冷却时间
            movementCooldown -= (timeSinceUpdate * viewPieceFriend.crtTimeMultiplier) / extraMultiplier;
            
            viewPieceFriend.actionBar.SetBarFillAmount(movementCooldown / recordedMoveSpeed);

            // 冷却完毕
            if (movementCooldown <= 0)
            {
                viewPieceFriend.Move();
                ResetMoveSpeed(); // 移动冷却时间重置为移动速度
            }
            lastUpdateTime = Time.time;
        }
        

        public override void ExitState()
        {
            
        }
    }
}