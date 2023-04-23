/// <summary>
/// 移动方向
/// </summary>
public enum Dir
{
    Top, Right, Down, Left, Topleft, Topright, Downright, Downleft, None
}

/// <summary>
/// 时间流逝速度
/// </summary>
public enum TimeMultiplier
{
    Normal, Fast, Superfast, Slow, Superslow
}

/// <summary>
/// 玩家属性
/// </summary>
public enum PlayerStats
{
    Strength, Spirit, Skill, Stamina, Charisma
}

/// <summary>
/// 技能名称
/// </summary>
public enum SkillName
{
    None, Alienation1, Earthquake1, Darkarrival, Focus1,
    Alienation2, Focus2
}

/// <summary>
/// 特性
/// </summary>
public enum Property
{
    None
}

/// <summary>
/// 地形
/// </summary>
public enum TerrainEnum
{
    Empty, Road, Wall, Water, Fire, Poison
}