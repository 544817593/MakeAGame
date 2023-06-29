using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 由InitCombatCommand发出的持续抽牌事件，参数结构体在InventorySystem下
    /// </summary>
    public struct RefillHandCardEvent
    {
        public float drawCardCooldown;
    }

    public class CostSanEvent
    {
        public int sanCost;
    }

}