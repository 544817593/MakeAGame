using UnityEngine;

namespace Game
{
    public partial class ViewPieceBase
    {
        // prefab结构相关
        protected Bar healthBar;
        public Bar actionBar;
        protected Bar lifeBar;

        void InitBind()
        {
            var hpBarTrans = transform.Find("Root/SpritePiece/HealthBar");
            healthBar = new Bar(hpBarTrans);
            var actionBarTrans = transform.Find("Root/SpritePiece/ActionBar");
            actionBar = new Bar(actionBarTrans);
            var lifeBarTrans = transform.Find("Root/SpritePiece/LifeBar");
            // 怪物没有这个东西
            if (lifeBarTrans != null)
            {
                lifeBar = new Bar(lifeBarTrans);
            }
        }
    }
}