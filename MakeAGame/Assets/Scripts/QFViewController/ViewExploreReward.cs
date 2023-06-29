using UnityEngine;

namespace Game
{
    public class ViewExploreReward: MonoBehaviour
    {
        public Transform nodeStatus;
        public Transform nodeRelic;

        public ExploreReward reward { get; private set; }

        public void SetReward(ExploreReward _reward)
        {
            reward = _reward;
            if (reward.addNum > 0)  // 玩家属性奖励
            {
                nodeStatus.gameObject.SetActive(true);
                nodeRelic.gameObject.SetActive(false);
            }
            else if (reward.relicID > 0)    // 遗物奖励
            {
                nodeStatus.gameObject.SetActive(false);
                nodeRelic.gameObject.SetActive(true);

                SpriteRenderer sprRelic = nodeRelic.GetChild(0).GetComponent<SpriteRenderer>();
                sprRelic.sprite = Extensions.GetRelicSpriteByID(reward.relicID);
            }
        }
    }
}