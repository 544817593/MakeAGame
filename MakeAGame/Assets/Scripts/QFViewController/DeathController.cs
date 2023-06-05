using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Game;
using System;
using Unity.VisualScripting;
using UnityEditor;
using static Game.Card;
using static DeathController;

public class DeathController : MonoBehaviour, IController
{
    private static DeathController _instance;
    public static DeathController Instance { get { return _instance; } }

    /// <summary>
    /// 死面函数触发所需要的参数
    /// </summary>
    public struct DeathCardCheckEvent
    {
        public ViewCard viewCard; // 使用的卡牌
        public BoxGrid[] affectArea; // 所有生效的格子都需要传入
    }

    public Action<DeathCardCheckEvent> OnDeathCardCheck { get; private set; }
    public IPieceSystem pieceSystem;

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
        pieceSystem = this.GetSystem<IPieceSystem>();
        OnDeathCardCheck += MatchDeathCard;
    }

    /// <summary>
    /// 根据所使用的死面卡牌id，来决定调用哪个死面函数
    /// </summary>
    /// <param name="e"></param>
    private void MatchDeathCard(DeathCardCheckEvent e)
    {
        switch (e.viewCard.card.charaID)
        {
            case 2:
                Francis_Wayland_Thurston(e);
                break;
            case 3:
                Randolph_Carter(e);
                break;
            case 4:
                Abigal_Miller(e);
                break;
            case 5:
                D_ranked_staff(e);
                break;
            case 6:
                C_ranked_staff(e);
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            case 15:
                break;
            case 16:
                break;
            case 17:
                break;
            case 18:
                break;
            case 19:
                break;
            case 21:
                break;
            case 22:
                break;
            case 23:
                break;
            case 24:
                break;
            case 25:
                break;
            case 26:
                break;
            case 27:
                break;
            case 28:
                break;
            case 29:
                break;
            case 30:
                break;
            case 31:
                break;
            case 32:
                break;
            case 33:
                break;
        }
    }
    // 在当前位置放置一个不可摧毁的障碍物，获得一张弗朗西斯。
    // 传入选择的格子，摧毁卡牌，改变格子，生成美术障碍物，背包加入卡牌
    private void Francis_Wayland_Thurston(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Francis_Wayland_Thurston");
        if(e.affectArea.Length != 1)
        {
            Debug.LogError("Francis_Wayland_Thurston传入格子数量错误");
            return;
        }
        foreach(BoxGrid grid in e.affectArea)
        {
            grid.terrain.Value = (int) TerrainEnum.Wall; // 格子类型
            grid.setSrFloor(Resources.Load<Sprite>("Sprites/Grids/地砖")); // 更换障碍物资源
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(e.viewCard.card); // 加入背包
            this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
        }
    }

    // 伦道夫·卡特 当前位置的时间流逝速率永久降为最慢，同时获得一张伦道夫。
    private void Randolph_Carter(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Randolph_Carter");
        if (e.affectArea.Length != 1)
        {
            Debug.LogError("Francis_Wayland_Thurston传入格子数量错误");
            return;
        }
        foreach (BoxGrid grid in e.affectArea)
        {
            grid.timeMultiplier.Value = TimeMultiplierEnum.Superslow; // 最慢流速
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(e.viewCard.card); // 加入背包
            this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
        }
    }
    private void damageMonster(int damage, DeathCardCheckEvent e)
    {
        // 如果经过强化，则伤害加上强化的值
        if (e.viewCard.card.deathEnhancement.damageIncrease > 0)
        {
            damage += e.viewCard.card.deathEnhancement.damageIncrease;
        }
        // 遍历传入的grid，对所有monster造成伤害
        foreach (BoxGrid grid in e.affectArea)
        {
            if (pieceSystem.IsPieceMonster(grid.occupation))
            {
                Monster monster = pieceSystem.getMonsterById(grid.occupation);
                monster.hp.Value -= damage;
            }
        }
    }
    // 阿比盖尔  对当前位置以及其上下左右范围为1的格子造成1点伤害，获得一张阿比盖尔。
    // 传入选择的格子(传入最多5个格子，要在传入前选择好，如果上下左右包含了战斗区域外格子就不要传这些格子)，造成伤害，摧毁卡牌，背包加入卡牌。
    private void Abigal_Miller(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Abigal_Miller");
        if (e.affectArea.Length > 5 || e.affectArea.Length == 0)
        {
            Debug.LogError("Francis_Wayland_Thurston传入格子数量错误");
            return;
        }
        damageMonster(1, e);
        this.GetSystem<IInventorySystem>().SpawnBagCardInBag(e.viewCard.card); // 加入背包
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }
    // D级员工（D-ranked staff）对3*3的格子造成15点伤害。
    // 传入选择的格子，造成伤害，摧毁卡牌。
    private void D_ranked_staff(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: D_ranked_staff");
        if (e.affectArea.Length > 9 || e.affectArea.Length == 0)
        {
            Debug.LogError("Francis_Wayland_Thurston传入格子数量错误");
            return;
        }
        damageMonster(15, e);
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 对3*3的格子造成20点伤害，25%的概率获得一张D级员工。
    // 传入选择的格子，造成伤害，摧毁卡牌，判定成功后添加一张D级员工到背包。
    private void C_ranked_staff(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: C_ranked_staff");
        if (e.affectArea.Length > 9 || e.affectArea.Length == 0)
        {
            Debug.LogError("Francis_Wayland_Thurston传入格子数量错误");
            return;
        }
        damageMonster(20, e);
        if (UnityEngine.Random.Range(1, 101) <= 25)
        {
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(new Card(5)); // D级员工加入背包，D级员工id为5
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }



    // 伤害增加
    // +血
    // 效果持续时间增加
    // 额外效果



    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}
