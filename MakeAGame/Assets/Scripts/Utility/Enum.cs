/// <summary>
/// 移动方向
/// </summary>
public enum DirEnum
{
    Top, Right, Down, Left, Topleft, Topright, Downright, Downleft, None
}

/// <summary>
/// 时间流逝速度
/// </summary>
public enum TimeMultiplierEnum
{
    Normal, Fast, Superfast, Slow, Superslow
}

/// <summary>
/// 玩家属性
/// </summary>
public enum PlayerStatsEnum
{
    Strength, Spirit, Skill, Stamina, Charisma
}

/// <summary>
/// 技能名称
/// </summary>
public enum SkillNameEnum
{
    None, Alienation1, Earthquake1, Darkarrival, Focus1,
    Alienation2, Focus2
}

/// <summary>
/// 特性
/// </summary>
public enum PropertyEnum
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

/// <summary>
/// 不同房间类型
/// </summary>
public enum RoomEnum
{
    Explore, // 探索房
    Combat, // 战斗房
    NPC, // NPC房间
    Merchant, // 商人房
    Rest, // 休息房
    Boss // Boss房
}