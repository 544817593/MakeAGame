using Game;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class FeatureController : MonoBehaviour, IController
{
    // 伤害修改值，保持伤害加成/减免都以基础伤害为基准计算，防止重复叠加
    // 比如基础伤害100，特性1增加20%，特性2增加10%，那么最终伤害为130，并非132
    int damageAdjust; 

    /// <summary>
    /// 攻击时生效的特性，优先计算伤害(如翻倍)，其次计算效果(如吸血)，最后计算其它(如攻击后加攻速)
    /// </summary>
    public Action<SpecialitiesAttackCheckEvent> OnPieceAttackFeatureCheck { get; private set; }
    public Action<SpecialitiesDefendCheckEvent> OnPieceDefendFeatureCheck { get; private set; }
    public Action<SpecialitiesMoveCheckEvent> OnPieceMoveFeatureCheck { get; private set; }

    void Start()
    {
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


        // 移动时触发的特性检查
        OnPieceMoveFeatureCheck += Writer;
        OnPieceMoveFeatureCheck += TinyCreature;
        OnPieceMoveFeatureCheck += Lazy_;
        OnPieceMoveFeatureCheck += Laborer;
        

        this.RegisterEvent<SpecialitiesAttackCheckEvent>(OnPieceAttackFeatureCheck);
        this.RegisterEvent<SpecialitiesDefendCheckEvent>(OnPieceDefendFeatureCheck);
        this.RegisterEvent<SpecialitiesMoveCheckEvent>(OnPieceMoveFeatureCheck);

        // 测试用代码
        // this.GetSystem<IPieceBattleSystem>().StartBattle(new ViewPieceBase(), new List<ViewPieceBase>());
    }

    private void Laborer(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece is Monster)
        {
            Monster monster = obj.piece as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Laborer))
            {
                obj.boxgrid.LevelDownTimeMultiplier();
            }
        }
        else
        {
            ViewPiece piece = obj.piece as ViewPiece;
            if (piece.card.HasFeature("打工者"))
            {
                obj.boxgrid.LevelDownTimeMultiplier();
            }
        }
    }

    private void Lazy_(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece is Monster)
        {
            Monster monster = obj.piece as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Lazy))
            {
                obj.boxgrid.LevelUpTimeMultiplier();
            }
        }
        else
        {
            ViewPiece piece = obj.piece as ViewPiece;
            if (piece.card.HasFeature("游手好闲"))
            {
                obj.boxgrid.LevelUpTimeMultiplier();
            }
        }
    }

    private void TinyCreature(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece is Monster)
        {
            Monster monster = obj.piece as Monster;
            if (monster.features.Value.Contains(PropertyEnum.TinyCreature))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 20 && monster.GetPieceState() == PieceStateEnum.Moving)
                {
                    monster.GetEnemyMovingState().movementCooldown = 0.01f;
                }
            }
        }
        else
        {
            ViewPiece piece = obj.piece as ViewPiece;
            if (piece.card.HasFeature("小型动物"))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 20 && piece.GetPieceState() == PieceStateEnum.Moving)
                {
                    piece.GetFriendMovingState().movementCooldown = 0.01f;
                }
            }
        }
    }

    private void Writer(SpecialitiesMoveCheckEvent obj)
    {
        if (obj.piece is ViewPiece)
        {
            ViewPiece piece = obj.piece as ViewPiece;
            if (piece.card.HasFeature("作家"))
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
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            if (monster.features.Value.Contains(PropertyEnum.SoundSensitive))
            {
                if (obj.isMagic) damageAdjust += (int)(0.1 * obj.damage);
                if (!obj.isMagic) damageAdjust -= (int)(0.25 * obj.damage);
            }
        }
        else
        {
            ViewPiece piece = obj.target as ViewPiece;
            if (piece.card.HasFeature("声音敏感"))
            {
                if (obj.isMagic) damageAdjust += (int)(0.1 * obj.damage);
                if (!obj.isMagic) damageAdjust -= (int)(0.25 * obj.damage);
            }
        }
    }

    private void Feline_Def(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Feline))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 25) obj.damage = 0;
            }
        }
        else
        {
            ViewPiece piece = obj.target as ViewPiece;
            if (piece.card.HasFeature("猫科"))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 25) obj.damage = 0;
            }
        }
    }

    private void Aquatic(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Aquatic)) 
            { 
                if (obj.boxgrid.terrain.Value == (int)TerrainEnum.Water)
                {
                    damageAdjust -= (int)(0.1f * obj.damage);
                }
            }
        }
        else
        {
            ViewPiece piece = obj.target as ViewPiece;
            if (piece.card.HasFeature("水生物"))
            {
                if (obj.boxgrid.terrain.Value == (int)TerrainEnum.Water)
                {
                    damageAdjust -= (int)(0.1f * obj.damage);
                }
            }
        }
    }

    private void MagicResistant(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            if (monster.features.Value.Contains(PropertyEnum.MagicResistant))
            {
                if (obj.isMagic) damageAdjust -= (int)(0.25 * obj.damage);
                if (!obj.isMagic) damageAdjust += (int)(0.1 * obj.damage);
            }
        }
        else
        {
            ViewPiece piece = obj.target as ViewPiece;
            if (piece.card.HasFeature("抗魔者"))
            {
                if (obj.isMagic) damageAdjust -= (int)(0.25 * obj.damage);
                if (!obj.isMagic) damageAdjust += (int)(0.1 * obj.damage);
            }
        }
    }

    private void Camouflaged(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Camouflaged))
            {
                if (obj.boxgrid.terrain.Value == (int)TerrainEnum.Road)
                {
                    int rnd = Random.Range(1, 101);
                    if (rnd <= 10) obj.damage = 0;
                }
            }
        }
        else
        {
            ViewPiece piece = obj.target as ViewPiece;
            if (piece.card.HasFeature("隐身"))
            {
                if (obj.boxgrid.terrain.Value == (int)TerrainEnum.Road)
                {
                    int rnd = Random.Range(1, 101);
                    if (rnd <= 10) obj.damage = 0;
                }
            }
        }
    }

    private void Anthropologist(SpecialitiesDefendCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Anthropologist))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 25) obj.damage = 0;
            }
        }
        else
        {
            ViewPiece piece = obj.target as ViewPiece;
            if (piece.card.HasFeature("人类学家"))
            {
                int rnd = Random.Range(1, 101);
                if (rnd <= 25) obj.damage = 0;
            }
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
        ViewPiece piece = obj.attacker as ViewPiece;
        if (piece.card.HasFeature("贪婪"))
        {
            int rnd = UnityEngine.Random.Range(1, 101);
            if (rnd <= 10) GameManager.Instance.playerMan.player.AddGold(2);
        }
    }

    private void Avian(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            ViewPiece piece = obj.attacker as ViewPiece;
            Monster monster = obj.target as Monster;
            if (piece.card.HasFeature("鸟类") &&
               monster.features.Value.Contains(PropertyEnum.Aquatic))
            {
                damageAdjust += (int)0.25f * obj.damage;
            }
        }
        else
        {
            Monster monster = obj.attacker as Monster;
            ViewPiece piece = obj.target as ViewPiece;
            if (monster.features.Value.Contains(PropertyEnum.Avian) &&
                piece.card.HasFeature("水生物"))
            {
                damageAdjust += (int)0.25f * obj.damage;
            }
        }
    }

    private void Feline_Atk(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            ViewPiece piece = obj.attacker as ViewPiece;
            Monster monster = obj.target as Monster;
            if (piece.card.HasFeature("猫科") &&
                (monster.features.Value.Contains(PropertyEnum.Avian) ||
                monster.features.Value.Contains(PropertyEnum.Rodent)))
            {
                int rnd = UnityEngine.Random.Range(1, 101);
                if (rnd <= 20) damageAdjust += obj.damage;
            }
        }
        else
        {
            Monster monster = obj.attacker as Monster;
            ViewPiece piece = obj.target as ViewPiece;
            if (monster.features.Value.Contains(PropertyEnum.Feline) &&
                (piece.card.HasFeature("鸟类") || piece.card.HasFeature("鼠类")))
            {
                int rnd = UnityEngine.Random.Range(1, 101);
                if (rnd <= 20) damageAdjust += obj.damage;
            }
        }
    }

    private void AnimalKiller(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            ViewPiece piece = obj.attacker as ViewPiece;
            Monster monster = obj.target as Monster;
            if (piece.card.HasFeature("动物杀手") && 
                monster.features.Value.Contains(PropertyEnum.TinyCreature))
            {
                piece.card.atkSpd += 0.1f;
            }
        }
        else
        {
            Monster monster = obj.attacker as Monster;
            ViewPiece piece = obj.target as ViewPiece;
            if (monster.features.Value.Contains(PropertyEnum.AnimalKiller) &&
                piece.card.HasFeature("小型动物"))
            {
                monster.atkSpeed.Value += 0.1f;
            }
        }
    }

    private void Bloodthirsty(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            ViewPiece piece = obj.attacker as ViewPiece;
            if (piece.card.HasFeature("嗜血者") && obj.hit)
            {
                piece.card.hp += (int)(0.25f * obj.damage);
                if (piece.card.hp > piece.card.maxHp) piece.card.hp = piece.card.maxHp;
            }
        }
        else
        {
            Monster monster = obj.attacker as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Bloodthirsty) && obj.hit)
            {
                monster.hp.Value += (int)(0.25f * obj.damage);
                if (monster.hp > monster.maxHp) monster.hp = monster.maxHp;
            }
        }
    }

    private void Toxicologist(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            ViewPiece piece = obj.attacker as ViewPiece;
            if (piece.card.HasFeature("施毒者"))
            {
                GameManager.Instance.buffMan.AddBuff(new DebuffPoison(obj.target, 5f));
            }
        }
        else
        {
            Monster monster = obj.attacker as Monster;
            if (monster.features.Value.Contains(PropertyEnum.Toxicologist))
            {
                GameManager.Instance.buffMan.AddBuff(new DebuffPoison(obj.target, 5f));
            }
        }
    }

    private void Dominant(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            ViewPiece piece = obj.attacker as ViewPiece;
            Monster monster = obj.target as Monster;
            if (piece.card.HasFeature("霸道") && monster.rarity == 0)
            {
                damageAdjust += (int)0.25f * obj.damage;
            }
        }
        else
        {
            Monster monster = obj.attacker as Monster;
            ViewPiece piece = obj.target as ViewPiece;           
            if (monster.features.Value.Contains(PropertyEnum.Dominant) && piece.card.rarity == 0)
            {
                damageAdjust += obj.damage + (int)0.25f * obj.damage;
            }
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}
