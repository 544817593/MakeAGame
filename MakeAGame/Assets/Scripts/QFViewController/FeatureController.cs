using Game;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureController : MonoBehaviour, IController
{
    public Action<PieceAttackStartEvent> OnPieceAttackFeatureCheck { get; private set; }
    public Action<PieceUnderAttackEvent> OnPieceUnderAttackFeatureCheck { get; private set; }
    public Action<PieceMoveReadyEvent> OnPieceMoveReadyFeatureCheck { get; private set; }
    public Action<PieceMoveFinishEvent> OnPieceMoveFinishFeatureCheck { get; private set; }

    void Start()
    {
        OnPieceAttackFeatureCheck += Dominant;
        OnPieceAttackFeatureCheck += Toxicologist;
        OnPieceAttackFeatureCheck += Bloodthirsty;
        OnPieceAttackFeatureCheck += AnimalKiller;
        OnPieceAttackFeatureCheck += Feline;
        OnPieceAttackFeatureCheck += Avian;
        OnPieceAttackFeatureCheck += Greedy;

        this.RegisterEvent<PieceAttackStartEvent>(OnPieceAttackFeatureCheck);
        this.RegisterEvent<PieceUnderAttackEvent>(OnPieceUnderAttackFeatureCheck);
        this.RegisterEvent<PieceMoveReadyEvent>(OnPieceMoveReadyFeatureCheck);
        this.RegisterEvent<PieceMoveFinishEvent>(OnPieceMoveFinishFeatureCheck);
    }

    private void Greedy(PieceAttackStartEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Avian(PieceAttackStartEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Feline(PieceAttackStartEvent obj)
    {
        throw new NotImplementedException();
    }

    private void AnimalKiller(PieceAttackStartEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Bloodthirsty(PieceAttackStartEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Toxicologist(PieceAttackStartEvent obj)
    {
        throw new NotImplementedException();
    }

    private void Dominant(PieceAttackStartEvent obj)
    {
        if (obj.isTargetMonster)
        {
            Monster monster = obj.target as Monster;
            if (monster.properties.Value.Contains(PropertyEnum.Greedy) && monster.rarity == 0)
                obj.baseDamage = obj.baseDamage + (int)0.25f * obj.baseDamage;
        }
        else
        {
            ViewPiece piece = obj.target as ViewPiece;
            if (piece.card.HasFeature(6) && piece.card.rarity == 0)
                obj.baseDamage = obj.baseDamage + (int)0.25f * obj.baseDamage;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}
