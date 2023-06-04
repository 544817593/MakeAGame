using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Game;
using System;
using Unity.VisualScripting;

public class DeathController : MonoBehaviour, IController
{
    private static DeathController _instance;
    public static DeathController Instance { get { return _instance; } }

    public Action<DeathCardCheckEvent> OnDeathCardCheck { get; private set; }

    public IArchitecture GetArchitecture()
    {
        throw new System.NotImplementedException();
    }

    private BoxGrid[,] boxGrid;

    private void Awake()
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

    private void Start()
    {
        boxGrid = this.GetSystem<IMapSystem>().Grids();
        OnDeathCardCheck += CheckDeathCard;

    }

    /// <summary>
    /// 根据所使用的死面卡牌，来决定调用哪个死面函数
    /// </summary>
    /// <param name="e"></param>
    private void CheckDeathCard(DeathCardCheckEvent e)
    {
        switch (e.viewCard.card.charaID)
        {

        }
    }

    private void DRankedStaff(BoxGrid[,] boxGrids, ViewCard card)
    {
        
        foreach (BoxGrid grid in boxGrids)
        {

        }

    }

    private void CRankedStaff()
    {

    }



    // 伤害增加
    // +血
    // 效果持续时间增加
    // 额外效果

    /// <summary>
    /// 死面函数触发所需要的参数
    /// </summary>
    public struct DeathCardCheckEvent
    {
        public ViewCard viewCard;
        public int[,] affectArea;

    }


}
