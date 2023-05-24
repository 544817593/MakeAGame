using Game;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FeatureController : MonoBehaviour, IController
{
    public Action<SpecialitiesAttackCheckEvent> OnPieceAttackFeatureCheck { get; private set; }
    public Action<PieceUnderAttackEvent> OnPieceUnderAttackFeatureCheck { get; private set; }
    public Action<PieceMoveReadyEvent> OnPieceMoveReadyFeatureCheck { get; private set; }
    public Action<PieceMoveFinishEvent> OnPieceMoveFinishFeatureCheck { get; private set; }

    void Start()
    {
        OnPieceAttackFeatureCheck += Dominant;
        //OnPieceAttackFeatureCheck += Toxicologist;
        //OnPieceAttackFeatureCheck += Bloodthirsty;
        //OnPieceAttackFeatureCheck += AnimalKiller;
        //OnPieceAttackFeatureCheck += Feline;
        //OnPieceAttackFeatureCheck += Avian;
        //OnPieceAttackFeatureCheck += Greedy;

        this.RegisterEvent<SpecialitiesAttackCheckEvent>(OnPieceAttackFeatureCheck);
        this.RegisterEvent<PieceUnderAttackEvent>(OnPieceUnderAttackFeatureCheck);
        this.RegisterEvent<PieceMoveReadyEvent>(OnPieceMoveReadyFeatureCheck);
        this.RegisterEvent<PieceMoveFinishEvent>(OnPieceMoveFinishFeatureCheck);

        // 测试用代码
        this.GetSystem<IPieceBattleSystem>().StartBattle(new ViewPieceBase(), new List<ViewPieceBase>());
    }

    private void Greedy(SpecialitiesAttackCheckEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Avian(SpecialitiesAttackCheckEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Feline(SpecialitiesAttackCheckEvent obj)
    {
        throw new NotImplementedException();
    }

    private void AnimalKiller(SpecialitiesAttackCheckEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Bloodthirsty(SpecialitiesAttackCheckEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Toxicologist(SpecialitiesAttackCheckEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Dominant(SpecialitiesAttackCheckEvent obj)
    {
        Debug.LogWarning("Dominant");
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            ViewPiece piece = obj.attacker as ViewPiece;
            if (piece.card.HasFeature(6) && monster.rarity == 0)
                obj.damage = obj.damage + (int)0.25f * obj.damage;
        }
        else
        {
            
            ViewPiece piece = obj.target as ViewPiece;
            Monster monster = obj.attacker as Monster;
            // test
            obj.damage = obj.damage + (int)0.25f * obj.damage;
            Debug.LogWarning("Damage changed");
            return;
            // ***
            if (monster.properties.Value.Contains(PropertyEnum.Dominant) && piece.card.rarity == 0)
            {
                Debug.LogWarning("Feature triggered");
                obj.damage = obj.damage + (int)0.25f * obj.damage;
            }
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}
