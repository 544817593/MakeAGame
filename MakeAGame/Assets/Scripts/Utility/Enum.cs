/// <summary>
/// 移动方向
/// </summary>
public enum DirEnum
{
    None, Top, Right, Down, Left, Topleft, Topright, Downright, Downleft
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
    None, Strength, Spirit, Skill, Stamina, Charisma, Total
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
    Anthropologist, Writer, Hydrophobia, Insomnia, Psychologist, Dominant,
    SolitaryHero, Determined, Toxicologist, Camouflaged, Bloodthirsty, MagicResistant,
    Airborne, AnimalKiller, Aquatic, Feline, SoundSensitive, TinyCreature, Avian,
    Rodent, Human, Lazy, Laborer, Greedy
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
/// 道具类型，枚举顺序【不可以】改变，用来整理背包显示顺序
/// </summary>
public enum ItemUseTimeEnum
{
    Combat, // 战斗场景，主要是药剂
    AnyTime, // 任何时候可以使用的道具
    Merchant, // 商店场景，主要是强化道具
    NotUsable // 无法使用的道具，主要是任务道具、功能型道具
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

/// <summary>
/// 棋子状态
/// </summary>
public enum PieceStateEnum
{
    Idle, Moving, Attacking
}
