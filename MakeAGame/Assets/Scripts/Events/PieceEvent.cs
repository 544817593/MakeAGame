using System.Collections.Generic;

namespace Game
{
    // 棋子相关事件

    #region 移动相关

    /// <summary>
    /// 移动前 （只是冷却时间到了准备移动，未经过任何判定）
    /// </summary>
    public struct PieceMoveReadyEvent
    {
        public ViewPieceBase viewPieceBase;
    }
    
    public struct PieceMoveFinishEvent  // 移动后
    {
        public ViewPieceBase viewPieceBase;
    }

    #endregion

    #region 战斗相关
    
    public struct PieceAttackStartEvent // 开始战斗（而不是攻击）事件
    {
        public ViewPieceBase viewPieceBase;
    }
    
    public struct PieceAttackEndEvent   // 结束战斗事件
    {
        public ViewPieceBase vpb;
    }

    public struct PieceUnderAttackEvent
    {
        public ViewPieceBase viewPieceBase;
    }
    
    public class PieceAttackReadyEvent  // 即将发起攻击
    {
        // 一些战斗数据...
        public float damage;
        public float accuracy;
    }
    
    public class PieceAttackFinishEvent // 攻击处理完毕
    {
        
    }
    
    public class PieceHitReadyEvent // 即将受到攻击
    {
        public ViewPieceBase piece;
    }
    
    public class PieceHitFinishEvent    // 受击处理完毕
    {
        public ViewPieceBase piece;
        public ViewPieceBase attacker;
        public float damage;
    }

    #endregion

    #region 特性检测相关

    /// <summary>
    /// 进攻时计算完基础伤害和命中后进行特性检测
    /// </summary>
    public class SpecialitiesAttackCheckEvent
    {
        public ViewPieceBase attacker; // 攻击方
        public ViewPieceBase target; // 防守方
        public int damage; // 伤害
        public bool hit; // 是否命中
    }

    /// <summary>
    /// 受到伤害时计算完伤害和命中后进行特性检测
    /// </summary>
    public class SpecialitiesDefendCheckEvent
    {
        public ViewPieceBase attacker; // 攻击方
        public ViewPieceBase target; // 防守方
        public bool isMagic; // 伤害是否为魔法伤害
        public int damage; // 伤害
        public List<BoxGrid> boxgrids; // 受到攻击的格子(单位可能并非1*1)
    }

    /// <summary>
    /// 移动时进行的特性检测，已经判定完且可以移动，在实际移动前生效
    /// </summary>
    public class SpecialitiesMoveCheckEvent
    {
        public ViewPieceBase piece; // 棋子
        public BoxGrid boxgrid; // 将要移动到的格子

    }

    /// <summary>
    /// 棋子生成/放置后进行的特性效果
    /// </summary>
    public class SpecialitiesSpawnCheckEvent
    {
        // 二者一个为null
        public ViewPieceBase piece; // 棋子
        public bool isTargetMonster; // 棋子是否为怪物
    }

    /// <summary>
    /// 死面牌释放时的特性/额外属性检查
    /// </summary>
    public class SpecialitiesDeathExecuteEvent
    {
        public ViewCard viewCard;
        public List<BoxGrid> grids;
    }

    /// <summary>
    /// 棋子死亡时的特性/额外属性检查
    /// </summary>
    public class SpecialitiesPieceDieEvent
    {
        public ViewPieceBase viewPiece;
    }

    #endregion
}