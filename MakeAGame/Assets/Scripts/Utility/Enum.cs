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
public enum FeatureEnum
{
    None, Anthropologist, Writer, Hydrophobia, Insomnia, Psychologist, Dominant,
    SolitaryHero, Determined, Toxicologist, Camouflaged, Bloodthirsty, MagicResistant,
    Airborne, AnimalKiller, Aquatic, Feline, SoundSensitive, TinyCreature, Avian,
    Rodent, Human, Lazy, Laborer, Greedy, Cocooning, Doctor, Study, ThroughKey, Unshakable,
    Affinity, LoveCrazy, Insight, Vine, WaterHeal, Bred, Contradict, PoisonSource, Sacrifice, 
    MomBless
}

/// <summary>
/// 地形, Boxgrid.OnTerrainChanged()中设置
/// </summary>
public enum TerrainEnum
{
    Invalid, // 无效
    Road, // 普通道路
    Wall, // 墙体
    Water, Fire, Poison, // 水火毒
    Edge, // 游戏格子外
    Door, // 大门
    RoadPlaceable // 可放置棋子的道路
}

/// <summary>
/// 战斗区域外的背景图片选择
/// 【千万不要改Enum的顺序！新加Enum加在下面，不要在中间插入！】
/// </summary>
public enum EdgeSprite
{
    None, 
    Tile_Tomb, //地砖带墓碑
    Tile_Grass, //地砖带草
    Tile, //地砖
    Wall_High, //墙-上
    Wall_High_Grass, //墙-上带草
    Wall_Mid, //墙中
    Wall_Mid_Candle, //墙中带蜡烛 
    Wall_Low, // 墙下
    Carpet_Red_Left, // 红地毯左
    Carpet_Red_Middle, // 红地毯中
    Carpet_Red_Right, // 红地毯右
    Grass_Top, // 草皮-上
    Grass_Mid, // 草皮-中
    Grass_Low, // 草皮-下
    Grass_Left, // 草皮-左
    Grass_Right, // 草皮-右
    Grass_TopRight, // 草皮-右上
    Grass_TopLeft, // 草皮-左上
    Grass_LowRight, // 草皮-右下
    Grass_LowLeft, // 草皮-左下
    Grass_Haystack, // 草皮-草垛
    Tile_Jar, // 砖地-罐子
    Tile_Candle, // 砖地-蜡烛
    Stonesword_TopRight, // 石中剑-右上
    Stonesword_LowRight, // 石中剑-右下
    Stonesword_LowLeft, // 石中剑-左下
    Stoneskeleton_TopRight, // 骨架-右上
    Stoneskeleton_TopLeft, // 骨架-左上
    Stoneskeleton_LowRight, // 骨架-右下
    Stoneskeleton_LowLeft, // 骨架-左下
    SkeletonLight_Top, // 骨灯-上
    SkeletonLight_Low, // 骨灯-下

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

/// <summary>
/// 卡牌稀有度
/// </summary>
public enum RarityEnum
{
    White, Green, Blue, Purple, Orange
}

/// <summary>
/// 卡包
/// </summary>
public enum CardPackEnum
{
    None, Base, Warrior
}

/// <summary>
/// 死面的类型，用来检查是否可以被对应道具强化
/// </summary>
public enum DeathEnhanceTypeEnum
{
    None, Damage, Heal, Duration, RandDamageOnMap, FeatureRemove
}

/// <summary>
/// 物品的使用场景
/// </summary>
public enum ItemUsePlace
{
    None, Shop, Combat
}