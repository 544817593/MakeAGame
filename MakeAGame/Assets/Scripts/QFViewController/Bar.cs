using UnityEngine;

namespace Game
{
    /// <summary>
    /// 血条、能量条等使用sprite的条状UI的基类，遵循一定的命名和结构标准可以通用
    /// </summary>
    public class Bar
    {
        private Transform transform;
        private Transform nodeBar;  // 实际缩放的部分

        public Bar(Transform trans)
        {
            transform = trans;
            nodeBar = transform.Find("Bar");
        }

        public void SetBarFillAmount(float fillAmount)
        {
            fillAmount = Mathf.Clamp(fillAmount, 0, 1f);
            nodeBar.localScale = new Vector3(fillAmount, 1f);
        }
    }
}