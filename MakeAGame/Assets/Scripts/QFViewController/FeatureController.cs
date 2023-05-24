using Game;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FeatureController : MonoBehaviour, IController
{
    // 伤害修改值，保持伤害加成/减免都以基础伤害为基准计算，防止重复叠加
    // 比如基础伤害100，特性1增加20%，特性2增加10%，那么最终伤害为130，并非132
    int damageAdjust; 

    /// <summary>
    /// 攻击时生效的特性，优先计算伤害(如翻倍)，其次计算效果(如吸血)，最后计算其它(如攻击后加攻速)
    /// </summary>
    public Action<SpecialitiesAttackCheckEvent> OnPieceAttackFeatureCheck { get; private set; }
    public Action<PieceUnderAttackEvent> OnPieceUnderAttackFeatureCheck { get; private set; }
    public Action<PieceMoveReadyEvent> OnPieceMoveReadyFeatureCheck { get; private set; }
    public Action<PieceMoveFinishEvent> OnPieceMoveFinishFeatureCheck { get; private set; }

    void Start()
    {
        // 按顺序检查特性
        OnPieceAttackFeatureCheck += Dominant;
        OnPieceAttackFeatureCheck += Feline;
        OnPieceAttackFeatureCheck += Avian;
        // 伤害结算完毕
        OnPieceAttackFeatureCheck += FinalDamageCalculation;
        OnPieceAttackFeatureCheck += Toxicologist;
        OnPieceAttackFeatureCheck += Bloodthirsty;
        OnPieceAttackFeatureCheck += AnimalKiller;               
        OnPieceAttackFeatureCheck += Greedy;
        // 进攻类特性结算完毕
        OnPieceAttackFeatureCheck += AttackFeatureCheckComplete;

        this.RegisterEvent<SpecialitiesAttackCheckEvent>(OnPieceAttackFeatureCheck);
        this.RegisterEvent<PieceUnderAttackEvent>(OnPieceUnderAttackFeatureCheck);
        this.RegisterEvent<PieceMoveReadyEvent>(OnPieceMoveReadyFeatureCheck);
        this.RegisterEvent<PieceMoveFinishEvent>(OnPieceMoveFinishFeatureCheck);

        // 测试用代码
        // this.GetSystem<IPieceBattleSystem>().StartBattle(new ViewPieceBase(), new List<ViewPieceBase>());
    }

    private void AttackFeatureCheckComplete(SpecialitiesAttackCheckEvent obj)
    {
        damageAdjust = 0;
    }

    private void FinalDamageCalculation(SpecialitiesAttackCheckEvent obj)
    {
        if (damageAdjust < 0) damageAdjust = 0;
        obj.damage += damageAdjust;
    }

    private void Greedy(SpecialitiesAttackCheckEvent obj)
    {
        ViewPiece piece = obj.attacker as ViewPiece;
        if (piece.card.HasFeature(24))
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
            if (piece.card.HasFeature(19) &&
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
                piece.card.HasFeature(15))
            {
                damageAdjust += (int)0.25f * obj.damage;
            }
        }
    }

    private void Feline(SpecialitiesAttackCheckEvent obj)
    {
        if (obj.isTargetMonster)
        {
            ViewPiece piece = obj.attacker as ViewPiece;
            Monster monster = obj.target as Monster;
            if (piece.card.HasFeature(16) &&
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
                (piece.card.HasFeature(19) || piece.card.HasFeature(20)))
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
            if (piece.card.HasFeature(14) && 
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
                piece.card.HasFeature(18))
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
            if (piece.card.HasFeature(11) && obj.hit)
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
            if (piece.card.HasFeature(9))
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
            if (piece.card.HasFeature(6) && monster.rarity == 0)
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
