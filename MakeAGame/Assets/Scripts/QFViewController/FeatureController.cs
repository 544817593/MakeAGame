using Game;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class FeatureController : MonoBehaviour, IController
{
    // 伤害修改值，保持伤害加成/减免都以基础伤害为基准计算，防止重复叠加
    // 比如基础伤害100，特性1增加20%，特性2增加10%，那么最终伤害为130，并非132
    int damageAdjust;
    IPieceSystem pieceSystem;

    private static FeatureController _instance;
    public static FeatureController instance { get { return _instance; } }


    /// <summary>
    /// 攻击时生效的特性，优先计算伤害(如翻倍)，其次计算效果(如吸血)，最后计算其它(如攻击后加攻速)
    /// </summary>
    public Action<SpecialitiesAttackCheckEvent> OnPieceAttackFeatureCheck { get; private set; }
    /// <summary>
    /// 被攻击时生效的特性，先计算伤害，再计算闪避，dmg为原始伤害，isMagic为是否是魔法伤害。
    /// </summary>
    public Action<SpecialitiesDefendCheckEvent> OnPieceDefendFeatureCheck { get; private set; }
    /// <summary>
    /// 移动时进行特性检测;移动位置计算完，但实际移动生效前进行检测，因为海洋恐惧症可能阻止移动
    /// </summary>
    public Action<SpecialitiesMoveCheckEvent> OnPieceMoveFeatureCheck { get; private set; }
    /// <summary>
    /// 棋子生成/放置后进行的特性效果
    /// </summary>
    public Action<SpecialitiesSpawnCheckEvent> OnPieceSpawnFeatureCheck { get; private set; }
    /// <summary>
    /// 死面牌释放时检测
    /// </summary>
    public Action<SpecialitiesDeathExecuteEvent> OnDeathExecuteFeatureCheck { get; private set; }
    /// <summary>
    /// 棋子死亡时的检测
    /// </summary>
    public Action<SpecialitiesPieceDieEvent> OnPieceDieFeatureCheck { get; private set; }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        pieceSystem = this.GetSystem<IPieceSystem>();

        // 按顺序检查进攻类特性
        OnPieceAttackFeatureCheck += Dominant;
        OnPieceAttackFeatureCheck += Feline_Atk;
        OnPieceAttackFeatureCheck += Avian;
        // 此时伤害结算完毕
        OnPieceAttackFeatureCheck += FinalDamageCalculation_Atk;
        OnPieceAttackFeatureCheck += Toxicologist;
        OnPieceAttackFeatureCheck += Bloodthirsty;
        OnPieceAttackFeatureCheck += AnimalKiller;               
        OnPieceAttackFeatureCheck += Greedy;
        // 进攻类特性结算完毕
        OnPieceAttackFeatureCheck += AttackFeatureCheckComplete;


        // 按顺序检查防御类特性
        OnPieceDefendFeatureCheck += MagicResistant;
        OnPieceDefendFeatureCheck += SoundSensitive;
        OnPieceDefendFeatureCheck += Aquatic;
        // 此时伤害结算完毕
        OnPieceDefendFeatureCheck += FinalDamageCalculation_Def;
        OnPieceDefendFeatureCheck += Anthropologist;
        OnPieceDefendFeatureCheck += Camouflaged;       
        OnPieceDefendFeatureCheck += Feline_Def;
        OnPieceDefendFeatureCheck += Vine_Extra;

        // 移动时触发的特性检查
        OnPieceMoveFeatureCheck += Writer;
        OnPieceMoveFeatureCheck += TinyCreature;
        OnPieceMoveFeatureCheck += Lazy_;
        OnPieceMoveFeatureCheck += Laborer;
        // 移动时触发的额外属性检查
        OnPieceMoveFeatureCheck += Francis_Extra;
        OnPieceMoveFeatureCheck += Raven_Extra;

        // 棋子生成后的特性效果
        OnPieceSpawnFeatureCheck += SolitaryHero;
        OnPieceSpawnFeatureCheck += Determined;
        OnPieceSpawnFeatureCheck += Rodent;
        // 棋子生成后触发的额外属性检查
        OnPieceSpawnFeatureCheck += Francis_Extra;
        OnPieceSpawnFeatureCheck += WaterWizard_Extra;
        OnPieceSpawnFeatureCheck += RandolphCarter_Extra;
        OnPieceSpawnFeatureCheck += AbigalMiller_Extra;

        // 死面牌释放时触发的额外属性检查
        OnDeathExecuteFeatureCheck += RandolphCarter_Extra;

        // 棋子死亡时触发的额外属性检查
        OnPieceDieFeatureCheck += HaloReset;



        this.RegisterEvent<SpecialitiesAttackCheckEvent>(OnPieceAttackFeatureCheck);
        this.RegisterEvent<SpecialitiesDefendCheckEvent>(OnPieceDefendFeatureCheck);
        this.RegisterEvent<SpecialitiesMoveCheckEvent>(OnPieceMoveFeatureCheck);
        this.RegisterEvent<SpecialitiesSpawnCheckEvent>(OnPieceSpawnFeatureCheck);
        this.RegisterEvent<SpecialitiesDeathExecuteEvent>(OnDeathExecuteFeatureCheck);
        this.RegisterEvent<SpecialitiesPieceDieEvent>(OnPieceDieFeatureCheck);

        // 测试用代码
        // this.GetSystem<IPieceBattleSystem>().StartBattle(new ViewPieceBase(), new List<ViewPieceBase>());
    }

    #region 鼠类
    private void Rodent(SpecialitiesSpawnCheckEvent obj)
    {
        if (obj.piece.features.Value.Contains(FeatureEnum.Rodent))
        {
            if (obj.isTargetMonster)
            {
                StartCoroutine(RodentCO_Monster(obj.piece as Monster));
            }
            else
            {
                StartCoroutine(RodentCO_Ally(obj.piece as ViewPiece));
            }
        }
    }

    private IEnumerator RodentCO_Monster(Monster monster)
    {
        List<TimeMultiplierEnum> featureActiveEnumCondition = new List<TimeMultiplierEnum>();
        featureActiveEnumCondition.Add(TimeMultiplierEnum.Fast);
        featureActiveEnumCondition.Add(TimeMultiplierEnum.Superfast);

        bool featureActive = false;
        bool switchOffFeature = false;
        while (monster != null)
        {
            switchOffFeature = true;
            foreach(BoxGrid grid in monster.pieceGrids)
            {
                // 开启特性条件达成
                if (featureActiveEnumCondition.Contains(grid.timeMultiplier) && !featureActive)
                {
                    monster.atkSpeed.Value += 0.2f;
                    featureActive = true;
                    switchOffFeature = false;
                    break;
                }
            }
            if (featureActive && switchOffFeature)
            {
                monster.atkSpeed.Value -= 0.2f;
                featureActive = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RodentCO_Ally(ViewPiece piece)
    {
        List<TimeMultiplierEnum> featureActiveEnumCondition = new List<TimeMultiplierEnum>();
        featureActiveEnumCondition.Add(TimeMultiplierEnum.Fast);
        featureActiveEnumCondition.Add(TimeMultiplierEnum.Superfast);

        bool featureActive = false;
        bool switchOffFeature = false;
        while (piece != null)
        {
            switchOffFeature = true;
            foreach (BoxGrid grid in piece.pieceGrids)
            {
                // 开启特性条件达成
                if (featureActiveEnumCondition.Contains(grid.timeMultiplier) && !featureActive)
                {
                    piece.atkSpeed.Value += 0.2f;
                    featureActive = true;
                    switchOffFeature = false;
                    break;
                }
            }
            if (featureActive && switchOffFeature)
            {
                piece.atkSpeed.Value -= 0.2f;
                featureActive = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region 孤勇者
    private void SolitaryHero(SpecialitiesSpawnCheckEvent obj)
    {
        if (obj.piece.features.Value.Contains(FeatureEnum.SolitaryHero))
        {
            if (obj.isTargetMonster)
            {
                StartCoroutine(SolitaryHeroCO_Monster(obj.piece as Monster));

            }
            else
            {
                StartCoroutine(SolitaryHeroCO_Ally(obj.piece as ViewPiece));
            }
        }

    }

    private IEnumerator SolitaryHeroCO_Monster(Monster monster)
    {
        bool featureActive = false;
        while (monster != null)
        {
            int counter = 0;
            List<BoxGrid> surroundingGrids = FindSurroundingGrids(monster);
            List<ViewPiece> pieceFriendList = pieceSystem.pieceFriendList;
            foreach (ViewPiece piece in pieceFriendList)
            {
                foreach(BoxGrid grid in surroundingGrids)
                {
                    if (piece.pieceGrids.Contains(grid)) counter++;
                }
            }
            // 开启特性条件达成
            if (counter >= 2 && !featureActive)
            {
                monster.atkSpeed.Value += 0.2f;
                featureActive = true;
            }
            // 关闭特性条件达成
            else if (counter < 2 && featureActive)
            {
                monster.atkSpeed.Value -= 0.2f;
                featureActive = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SolitaryHeroCO_Ally(ViewPiece piece)
    {
        bool featureActive = false;
        while (piece != null)
        {
            int counter = 0;
            List<BoxGrid> surroundingGrids = FindSurroundingGrids(piece);
            List<Monster> pieceEnemyList = pieceSystem.pieceEnemyList;
            foreach (Monster monster in pieceEnemyList)
            {
                foreach (BoxGrid grid in surroundingGrids)
                {
                    if (monster.pieceGrids.Contains(grid)) counter++;
                }
            }
            // 开启特性条件达成
            if (counter >= 2 && !featureActive)
            {
                piece.atkSpeed.Value += 0.2f;
                featureActive = true;
            }
            // 关闭特性条件达成
            else if (counter < 2 && featureActive)
            {
                piece.atkSpeed.Value -= 0.2f;
                featureActive = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region 意志坚定
    private void Determined(SpecialitiesSpawnCheckEvent obj)
    {
        if (obj.piece.features.Value.Contains(FeatureEnum.Determined))
        {
            if (obj.isTargetMonster)
            {
                StartCoroutine(DeterminedCO_Monster(obj.piece as Monster));
            }
            else
            {
                StartCoroutine(DeterminedCO_Ally(obj.piece as ViewPiece));
            }
        }
    }

    private IEnumerator DeterminedCO_Monster(Monster monster)
    {
        bool featureActive = false;
        while (monster != null)
        {
            int counter = 0;
            List<BoxGrid> surroundingGrids = FindSurroundingGrids(monster);
            List<ViewPiece> pieceFriendList = pieceSystem.pieceFriendList;
            foreach (ViewPiece piece in pieceFriendList)
            {
                foreach (BoxGrid grid in surroundingGrids)
                {
                    if (piece.pieceGrids.Contains(grid)) counter++;
                }
            }
            // 开启特性条件达成
            if (counter <= 1 && !featureActive)
            {
                monster.atkSpeed.Value += 0.2f;
                featureActive = true;
            }
            // 关闭特性条件达成
            else if (counter > 1 && featureActive)
            {
                monster.atkSpeed.Value -= 0.2f;
                featureActive = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DeterminedCO_Ally(ViewPiece piece)
    {
        bool featureActive = false;
        while (piece != null)
        {
            int counter = 0;
            List<BoxGrid> surroundingGrids = FindSurroundingGrids(piece);
            List<Monster> pieceEnemyList = pieceSystem.pieceEnemyList;
            foreach (Monster monster in pieceEnemyList)
            {
                foreach (BoxGrid grid in surroundingGrids)
                {
                    if (monster.pieceGrids.Contains(grid)) counter++;
                }
            }
            // 开启特性条件达成
            if (counter <= 1 && !featureActive)
            {
                piece.atkSpeed.Value += 0.2f;
                featureActive = true;
            }
            // 关闭特性条件达成
            else if (counter > 1 && featureActive)
            {
                piece.atkSpeed.Value -= 0.2f;
                featureActive = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion


    /// <summary>
    /// 传入棋子，返还棋子周围一圈的格子
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    private List<BoxGrid> FindSurroundingGrids(ViewPieceBase piece)
    {
        List<BoxGrid> surroundingGrids = new List<BoxGrid>();
        BoxGrid[,] map = this.GetSystem<IMapSystem>().Grids();
        foreach (BoxGrid grid in piece.pieceGrids)
        {
            if (map[grid.row - 1, grid.col - 1].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
            if (map[grid.row - 1, grid.col].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
            if (map[grid.row - 1, grid.col + 1].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
            if (map[grid.row, grid.col - 1].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
            if (map[grid.row, grid.col + 1].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
            if (map[grid.row + 1, grid.col - 1].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
            if (map[grid.row + 1, grid.col].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
            if (map[grid.row + 1, grid.col + 1].terrain.Value != (int)TerrainEnum.Edge && !surroundingGrids.Contains(grid)) surroundingGrids.Add(grid);
        }
        return surroundingGrids;
    }

    /// <summary>
    /// 海洋恐惧症检测
    /// </summary>
    /// <param name="obj">棋子</param>
    /// <param name="grid">即将到达的格子</param>
    /// <returns>True为可以移动，False为不可移动</returns>
    public bool Hydrophobia(ViewPieceBase obj, BoxGrid grid)
    {
        if (obj.features.Value.Contains(FeatureEnum.Hydrophobia))
        {
            if (grid.terrain.Value == (int)TerrainEnum.Water) return false;
        }
        return true;
    }

    private void Laborer(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece.features.Value.Contains(FeatureEnum.Laborer))
        {
            obj.boxgrid.LevelDownTimeMultiplier();
        }
    }

    private void Lazy_(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece.features.Value.Contains(FeatureEnum.Lazy))
        {
            obj.boxgrid.LevelUpTimeMultiplier();
        }
    }

    private void TinyCreature(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece.features.Value.Contains(FeatureEnum.TinyCreature))
        {
            int rnd = Random.Range(1, 101);
            if (rnd <= 20 && obj.piece.GetPieceState() == PieceStateEnum.Moving)
            {
                obj.piece.GetEnemyMovingState().movementCooldown = 0.01f;
            }
        }
    }

    private void Writer(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece is ViewPiece)
        {
            if (obj.piece.features.Value.Contains(FeatureEnum.Writer))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 3)
                {
                    RarityEnum newCardRarity = 0;
                    int newCardId = -1;
                    while (newCardRarity != RarityEnum.White)
                    {
                        newCardId = PackProbability.DrawCard(0);
                        newCardRarity = IdToSO.FindCardSOByID(newCardId).rarity;
                    }
                    this.GetSystem<ISpawnSystem>().SpawnCard(newCardId);
                    Card new_Card = this.GetSystem<ISpawnSystem>().GetLastSpawnedCard().GetComponent<ViewBagCard>().card;
                    this.GetSystem<IInventorySystem>().SpawnBagCardInBag(new_Card);
                }
            }
        }
    }

    /// <summary>
    /// 防御特性影响后的攻击伤害最终计算
    /// </summary>
    /// <param name="obj"></param>
    private void FinalDamageCalculation_Def(SpecialitiesDefendCheckEvent obj)
    {
        obj.damage += damageAdjust;
        if (obj.damage < 0) obj.damage = 0;
    }

    private void SoundSensitive(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.target.features.Value.Contains(FeatureEnum.SoundSensitive))
        {
            if (obj.isMagic) damageAdjust += (int)(0.1 * obj.damage);
            if (!obj.isMagic) damageAdjust -= (int)(0.25 * obj.damage);
        }
    }

    private void Feline_Def(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.target.features.Value.Contains(FeatureEnum.Feline))
        {
            int rnd = Random.Range(1, 101);
            if (rnd <= 25) obj.damage = 0;
        }
    }

    private void Aquatic(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.target.features.Value.Contains(FeatureEnum.Aquatic))
        {
            if(obj.boxgrids.Find(grid => grid.terrain.Value == (int) TerrainEnum.Water))
            {
                damageAdjust -= (int)(0.1f * obj.damage);
            }
        }      
    }

    private void MagicResistant(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.target.features.Value.Contains(FeatureEnum.MagicResistant))
        {
            if (obj.isMagic) damageAdjust -= (int)(0.25 * obj.damage);
            if (!obj.isMagic) damageAdjust += (int)(0.1 * obj.damage);
        }     
    }

    private void Camouflaged(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.target.features.Value.Contains(FeatureEnum.Camouflaged))
        {
            if (obj.boxgrids.Find(grid => grid.terrain.Value == (int) TerrainEnum.Road))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 10) obj.damage = 0;
            }
        }    
    }

    private void Anthropologist(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.target.features.Value.Contains(FeatureEnum.Anthropologist))
        {
            int rnd = Random.Range(1, 101);
            if (rnd <= 25) obj.damage = 0;
        }        
    }

    /// <summary>
    /// 进攻类特性检测完毕
    /// </summary>
    /// <param name="obj"></param>
    private void AttackFeatureCheckComplete(SpecialitiesAttackCheckEvent obj)
    {
        damageAdjust = 0;
    }

    /// <summary>
    /// 特性影响后的攻击伤害最终计算
    /// </summary>
    /// <param name="obj"></param>
    private void FinalDamageCalculation_Atk(SpecialitiesAttackCheckEvent obj)
    {
        obj.damage += damageAdjust;
        if (obj.damage < 0) obj.damage = 0;
    }

    private void Greedy(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.attacker.features.Value.Contains(FeatureEnum.Greedy))
        {
            int rnd = UnityEngine.Random.Range(1, 101);
            if (rnd <= 10) GameManager.Instance.playerMan.player.AddGold(2);
        }
    }

    private void Avian(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.attacker.features.Value.Contains(FeatureEnum.Avian) &&
            obj.target.features.Value.Contains(FeatureEnum.Aquatic))
        {
            damageAdjust += (int)0.25f * obj.damage;
        }
    }

    private void Feline_Atk(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.attacker.features.Value.Contains(FeatureEnum.Feline) &&
            (obj.target.features.Value.Contains(FeatureEnum.Avian) ||
            obj.target.features.Value.Contains(FeatureEnum.Rodent)))
            {
                int rnd = UnityEngine.Random.Range(1, 101);
                if (rnd <= 20) damageAdjust += obj.damage;
            }
    }

    private void AnimalKiller(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.attacker.features.Value.Contains(FeatureEnum.AnimalKiller) &&
            obj.target.features.Value.Contains(FeatureEnum.TinyCreature))
        {
            obj.attacker.atkSpeed.Value += 0.1f;
        }
    }

    private void Bloodthirsty(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.attacker.features.Value.Contains(FeatureEnum.Bloodthirsty) && obj.hit)
        {
            obj.attacker.hp.Value += (int)(0.25f * obj.damage);
            if (obj.attacker.hp > obj.attacker.maxHp) obj.attacker.hp = obj.attacker.maxHp;
        }       
    }

    private void Toxicologist(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.attacker.features.Value.Contains(FeatureEnum.Toxicologist))
        {
            GameManager.Instance.buffMan.AddBuff(new DebuffPoison(obj.target, 5f));
        }       
    }

    private void Dominant(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.attacker.features.Value.Contains(FeatureEnum.Dominant) && obj.target.rarity == RarityEnum.White)
        {
            damageAdjust += obj.damage + (int)0.25f * obj.damage;
        }        
    }

    // 下面为额外属性 // **********************************************************************

    #region 弗朗西斯
    // 被弗朗西斯Buff过的棋子列表
    private List<ViewPieceBase> francisExtraAtkList = new List<ViewPieceBase>();
    // 弗朗西斯在场与否
    private ViewPieceBase francis = null;
    IEnumerator FrancisExtra_Attack(ViewPieceBase piece)
    {
        if (piece.generalId == 1) yield break;
        bool shouldHaveBuff = false;

        // 符合距离要求
        if (francis != null && pieceSystem.GetPieceDist(francis, piece) <= 4)
        {
            shouldHaveBuff = true;
        }

        if (shouldHaveBuff)
        {
            // 没有过同名Buff
            if (!francisExtraAtkList.Contains(piece))
            {
                piece.atkDmg.Value += GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Strength);
                francisExtraAtkList.Add(piece);
                Debug.Log("给棋子" + piece.pieceId + "添加弗朗西斯Buff26");
            }
        }
        else
        {
            // 如果曾经有过Buff则移除
            if (francisExtraAtkList.Contains(piece))
            {
                piece.atkDmg.Value -= GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Strength);
                francisExtraAtkList.Remove(piece);
                Debug.Log("给棋子" + piece.pieceId + "移除弗朗西斯Buff26");
            }
        }

        yield break;
    }

    // 棋子移动触发
    public void Francis_Extra(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece is Monster) return; // 怪物不触发
        if (francis == null) return; // 弗朗西斯不在场
        if (obj.piece == francis) return; // 自己不给自己上Buff
        StartCoroutine(FrancisExtra_Attack(obj.piece));
    }

    // 棋子放置触发
    public void Francis_Extra(SpecialitiesSpawnCheckEvent obj)
    {
        if (obj.piece is Monster) return; // 怪物不触发
        if (francis == null && obj.piece.generalId != 1) return; // 弗朗西斯不在场并且放置的不是弗朗西斯

        // 如果场上有弗朗西斯（且放置了一张非弗朗西斯的棋子）
        if (francis != null)
        {
            obj.piece.maxHp.Value += 2 * GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Stamina);
            obj.piece.hp.Value += 2 * GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Stamina);
            Debug.Log("给棋子" + obj.piece.pieceId + "添加弗朗西斯Buff27");
        }
        // francis为null但能跑到这里证明放置的是弗朗西斯
        else
        {
            francis = obj.piece;
            // 需要检查所有的在场棋子
            foreach (ViewPieceBase viewPiece in pieceSystem.pieceFriendList)
            {
                StartCoroutine(FrancisExtra_Attack(viewPiece));
            }
            return;
        }
        
        StartCoroutine(FrancisExtra_Attack(obj.piece));
    }
    #endregion

    #region 渡鸦
    IEnumerator Raven_Grid(BoxGrid grid)
    {
        TimeMultiplierEnum previousTime = grid.timeMultiplier;
        grid.timeMultiplier.Value = TimeMultiplierEnum.Normal;
        yield return new WaitForSeconds(20);
        grid.timeMultiplier.Value = previousTime;

    }
    public void Raven_Extra(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece.generalId != 13) return;
        StartCoroutine(Raven_Grid(obj.boxgrid));
    }


    #endregion

    #region 藤蔓
    IEnumerator Vine_Extra_MoveSpeed(ViewPieceBase piece)
    {
        if (piece != null)
        {
            float pre_value = piece.moveSpeed;
            float diff = pre_value / 2.0f;
            piece.moveSpeed.Value += diff;
            yield return new WaitForSeconds(3f);
            piece.moveSpeed.Value -= diff;
            yield break;
        }
    }
    public void Vine_Extra(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.attacker.generalId != 14) return;
        StartCoroutine(Vine_Extra_MoveSpeed(obj.target));
    }
    #endregion

    #region 水精
    public void WaterWizard_Extra(SpecialitiesSpawnCheckEvent obj)
    {
        if (obj.piece.generalId != 15) return;
        StartCoroutine(Water_Extra_Heal(obj.piece));
    }

    IEnumerator Water_Extra_Heal(ViewPieceBase piece)
    {
        while (piece.gameObject.activeInHierarchy)
        {
            BoxGrid grid = piece.pieceGrids[0];
            foreach(ViewPiece allyPiece in pieceSystem.pieceFriendList)
            {
                if (pieceSystem.GetPieceDist(piece, allyPiece) <= 3)
                {
                    if (Extensions.HasGridTypeInRange(grid, 3, TerrainEnum.Water))
                    {
                        allyPiece.hp.Value += 10;
                        if (allyPiece.hp.Value > allyPiece.maxHp) allyPiece.hp.Value = allyPiece.maxHp;
                    }
                    else
                    {
                        allyPiece.hp.Value += 20;
                        if (allyPiece.hp.Value > allyPiece.maxHp) allyPiece.hp.Value = allyPiece.maxHp;
                    }
                }

            }
            yield return new WaitForSeconds(3);
        }

    }
    #endregion

    #region 伦道夫

    // 伦道夫
    private ViewPieceBase randolph;
    public void RandolphCarter_Extra(SpecialitiesSpawnCheckEvent obj)
    {
        if (obj.piece.generalId != 2)
        {
            if (randolph == null) return;
            StartCoroutine(RandolphExtra_Immune(obj.piece));
            return;
        }
        randolph = obj.piece;
        StartCoroutine(RandolphExtra_SilverKey(obj.piece));
    }

    public void RandolphCarter_Extra(SpecialitiesDeathExecuteEvent obj)
    {
        if (randolph == null) return;
        List<(int, int)> coordinatesList = Extensions.GetCoordinatesInRange(randolph.pieceGrids[0].row, randolph.pieceGrids[0].col, 4);
        foreach (BoxGrid grid in obj.grids)
        {
            if (coordinatesList.Contains((grid.row, grid.col)))
            {
                obj.viewCard.card.deathEnhancement.damageIncrease += 2 * PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Skill);
                return;
            }
        }              
    }


    /// <summary>
    /// 穿越银匙之门：每10秒获得1点改变行动方向的机会。
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    IEnumerator RandolphExtra_SilverKey(ViewPieceBase piece)
    {
        while (piece.hp > 0)
        {
            PlayerManager.Instance.player.SetTurnPieceCount(PlayerManager.Instance.player.GetTurnPieceCount() + 1);
            yield return new WaitForSeconds(10f);
        }
    }

    /// <summary>
    /// 无可动摇：伦道夫在场时，玩家放入的棋子获得1*技巧秒物理伤害免疫。
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    IEnumerator RandolphExtra_Immune(ViewPieceBase piece)
    {
        float def = piece.defense.Value;
        piece.defense.Value = float.MaxValue;
        yield return new WaitForSeconds(PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Skill));
        piece.defense.Value = def;
    }


    #endregion

    #region 阿比盖尔
    // 阿比盖尔
    private ViewPieceBase abigal = null;

    // 放置时触发
    public void AbigalMiller_Extra(SpecialitiesSpawnCheckEvent obj)
    {
        if (abigal == null)
        {
            if (obj.piece.generalId != 3) return;
            abigal = obj.piece;
            StartCoroutine(AbigalExtra_Heal(obj.piece));
        }
        else
        {
            obj.piece.moveSpeed.Value -= PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Spirit) * 0.1f;
        }     
    }

    /// <summary>
    /// 亲和力：每3秒，距离不大于6的友军回复1*精神点寿命。
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    IEnumerator AbigalExtra_Heal(ViewPieceBase piece)
    {
        while (piece.hp > 0)
        {
            foreach (ViewPieceBase allyPiece in pieceSystem.pieceFriendList)
            {
                if (pieceSystem.GetPieceDist(allyPiece, piece) <= 6)
                {
                    ((ViewPiece)allyPiece).currLife.Value += PlayerManager.Instance.player.GetStats(PlayerStatsEnum.Spirit);
                    if (((ViewPiece)allyPiece).currLife.Value > ((ViewPiece)allyPiece).maxLife.Value) ((ViewPiece)allyPiece).currLife.Value = ((ViewPiece)allyPiece).maxLife.Value;
                }
            }
            yield return new WaitForSeconds(3);
        }

    }
    #endregion

    /// <summary>
    /// 光环类效果在棋子死亡后处理
    /// </summary>
    /// <param name="obj"></param>
    public void HaloReset(SpecialitiesPieceDieEvent obj)
    {
        Debug.Log("弗朗西斯HaloReset");
        if (obj.viewPiece is Monster) return;
        if (obj.viewPiece.rarity != RarityEnum.Orange) return;
        Debug.Log("弗朗西斯List" + francisExtraAtkList.Count);
        switch (obj.viewPiece.generalId)
        {
            case 1:
                foreach (ViewPieceBase piece in francisExtraAtkList)
                {
                    piece.atkDmg.Value -= GameManager.Instance.playerMan.player.GetStats(PlayerStatsEnum.Strength);
                    francisExtraAtkList.Remove(piece);
                    Debug.Log("给棋子" + piece.pieceId + "移除弗朗西斯Buff26");
                }
                break;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }

    
}
