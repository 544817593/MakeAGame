/// <summary>
/// 移动方向
/// </summary>
public enum Dir
{
    TOP, RIGHT, DOWN, LEFT,
    TOPLEFT, TOPRIGHT,
    DOWNRIGHT, DOWNLEFT,
    NONE
}

/// <summary>
/// 时间流逝速度
/// </summary>
public enum TimeMultiplier
{
    NORMAL, FAST, SUPERFAST, SLOW, SUPERSLOW
}

/// <summary>
/// 玩家属性
/// </summary>
public enum PlayerStats
{
    STRENGTH, SPIRIT, SKILL, STAMINA, CHRISMA
}

/// <summary>
/// 技能名称
/// </summary>
public enum SkillName
{
    NONE, ALIENATION1, EARTHQUAKE1, DARKARRIVAL, FOCUS1,
    ALIENATION2, FOCUS2
}

/// <summary>
/// 特性
/// </summary>
public enum Property
{

}

/// <summary>
/// 地形
/// </summary>
public enum TerrainEnum
{
    Empty, Road, Wall, Water, Fire, Poison
}