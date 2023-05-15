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
    None, Alienation1, Alienation2, Focus1, Focus2, Earthquake1, Darkarrival,
    Ghost, LastResort, DimentionalPortal, Inferno, Oceanic
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
    Invalid, Road, Wall, Water, Fire, Poison
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

/// <summary>
/// 格子的状态
/// </summary>
public enum GridStatusEnum
{
    Unoccupied, // 没有被占用
    AllyPiece, // 友方棋子
    MonsterPiece, // 怪物棋子
    Interactable // 可交互物
}

/// <summary>
/// 道具类型
/// </summary>
public enum ItemTypeEnum
{
    Enhancement, // 卡牌强化道具
    Coin, // 金币
    Potion, // 药剂
    Utility, // 功能性道具
    Misc // 其它
}

/// <summary>
/// 道具容量
/// </summary>
public enum ItemVolumeEnum
{
    Small, 
    Medium,
    Large
}