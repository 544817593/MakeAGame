using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 一场对战的相关信息：存在发起攻击方和受攻击方
    /// 可能有1v1、1vN、Nv1、NvN的情况，此处以攻击方为唯一主体，记录所有会被它的一次攻击击中的对手
    /// </summary>
    // public class PieceBattlePair
    // {
    //     public ViewPieceBase attacker;
    //     public List<ViewPieceBase> defender = new List<ViewPieceBase>();
    //
    //     public PieceBattlePair(ViewPieceBase _attacker, List<ViewPieceBase> _defender)
    //     {
    //         attacker = _attacker;
    //         defender = _defender;
    //     }
    // }
}