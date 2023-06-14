using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Game;
using System;
using System.Linq;

public class DeathController : MonoBehaviour, IController
{
    // 死面函数中要对卡牌的强化情况做检查：

    // 1.（可以造成伤害的死面）伤害增加
    // 2.（可以回血的死面）血量恢复增加
    // 3.（有持续时间效果的死面）效果持续时间增加
    // 4.（可以回血的死面）额外效果（目前仅有对全图随机3个敌人造成回复血量的同等伤害）
    // 其中1 2 4是有可能共存的，这种情况下对全图随机3个敌人造成回复血量的伤害会享受1的伤害增加

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
                B_ranked_staff(e);
                break;
            case 8:
                A_ranked_staff(e);
                break;
            case 9:
                Devil_bug(e);
                break;
            case 10:
                Owl(e);
                break;
            case 11:
                Quack_Frog(e);
                break;
            case 12:
                Failure(e);
                break;
            case 13:
                Laughing_Cat(e);
                break;
            case 14:
                Raven(e);
                break;
            case 15:
                Vine(e);
                break;
            case 16:
                Water_Wizard(e);
                break;
            //case 17:
            //    break;
            //case 18:
            //    break;
            //case 19:
            //    break;
            //case 21:
            //    break;
            //case 22:
            //    break;
            //case 23:
            //    break;
            //case 24:
            //    break;
            //case 25:
            //    break;
            //case 26:
            //    break;
            //case 27:
            //    break;
            //case 28:
            //    break;
            //case 29:
            //    break;
            //case 30:
            //    break;
            //case 31:
            //    break;
            //case 32:
            //    break;
            //case 33:
            //    break;
        }
    }
    /// <summary>
    /// 对格子上怪物造成伤害
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="e"></param>
    private void DamageMonster(int damage, DeathCardCheckEvent e)
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
                monster.takeDamage(damage);
            }
        }
    }

    /// <summary>
    /// 对地图上随机RandMonsterCount个怪物造成传入治疗量的伤害，这个伤害可以享受卡牌的伤害加成强化效果，
    /// 包含了怪物死亡检查MonsterDeathCheck
    /// </summary>
    /// <param name="e"></param>
    /// <param name="heal"></param>
    /// <param name="RandMonsterCount"></param>
    private void DamageRandMonster(DeathCardCheckEvent e, int heal, int RandMonsterCount)
    {
        int damage = heal;
        if (e.viewCard.card.deathEnhancement.damageIncrease > 0) // 检查卡牌的伤害加成强化效果
        {
            damage += e.viewCard.card.deathEnhancement.damageIncrease;
        }
        // 地图上随机RandMonsterCount个怪物造成等于回复量的伤害
        if (e.viewCard.card.deathEnhancement.extraDamageEffect)
        {
            // 地图上怪物总数不超过RandMonsterCount个直接扣血，超过RandMonsterCount个随机挑选
            if (pieceSystem.pieceEnemyList.Count <= RandMonsterCount)
            {
                foreach (Monster monster in pieceSystem.pieceEnemyList)
                {
                    monster.takeDamage(damage);
                }
            }
            else
            {
                // 洗牌算法，打乱列表后选择RandMonsterCount个数作为monster列表中的index
                // 创建一个包含0, 1, 2,..., pieceSystem.pieceEnemyList.Count - 1的list
                // 从头开始，每轮循环从[当前index, 总长度)的范围内随机选1个index, 将随机选择的index位置的值和当前位置的值互换，进行下一轮循环
                List<int> numbers = Enumerable.Range(0, pieceSystem.pieceEnemyList.Count - 1).ToList();
                for (int i = 0; i < numbers.Count; i++)
                {
                    int temp = numbers[i];
                    int randomIndex = UnityEngine.Random.Range(i, numbers.Count);
                    numbers[i] = numbers[randomIndex];
                    numbers[randomIndex] = temp;
                }
                for (int i = 0; i < RandMonsterCount; i++)
                {
                    Monster monster = pieceSystem.pieceEnemyList[numbers[i]];
                    monster.takeDamage(damage);
                }
            }
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
            grid.setSrFloor(Resources.Load<Sprite>("Sprites/Grids/terrain2")); // 更换障碍物资源
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
            Debug.LogError("Randolph_Carter传入格子数量错误");
            return;
        }
        foreach (BoxGrid grid in e.affectArea)
        {
            grid.timeMultiplier.Value = TimeMultiplierEnum.Superslow; // 最慢流速
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(e.viewCard.card); // 加入背包
            this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
        }
    }


    // 阿比盖尔  对当前位置以及其上下左右范围为1的格子造成10点伤害，获得一张阿比盖尔。
    // 传入选择的格子(传入最多5个格子，要在传入前选择好，如果上下左右包含了战斗区域外格子就不要传这些格子)，造成伤害，摧毁卡牌，背包加入卡牌。
    private void Abigal_Miller(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Abigal_Miller");
        if (e.affectArea.Length > 5 || e.affectArea.Length == 0)
        {
            Debug.LogError("Abigal_Miller传入格子数量错误");
            return;
        }
        DamageMonster(10, e);
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
            Debug.LogError("D_ranked_staff传入格子数量错误");
            return;
        }
        DamageMonster(15, e);
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 对3*3的格子造成20点伤害，25%的概率获得一张D级员工。
    // 传入选择的格子，造成伤害，摧毁卡牌，判定成功后添加一张D级员工到背包。
    private void C_ranked_staff(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: C_ranked_staff");
        if (e.affectArea.Length > 9 || e.affectArea.Length == 0)
        {
            Debug.LogError("C_ranked_staff传入格子数量错误");
            return;
        }
        DamageMonster(20, e);
        if (UnityEngine.Random.Range(1, 101) <= 25)
        {
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(new Card(5)); // D级员工加入背包，D级员工id为5
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 对4*4的格子造成25点伤害，20%的概率获得一张C级员工。
    private void B_ranked_staff(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: B_ranked_staff");
        if (e.affectArea.Length > 16 || e.affectArea.Length == 0)
        {
            Debug.LogError("B_ranked_staff传入格子数量错误");
            return;
        }
        DamageMonster(25, e);
        if (UnityEngine.Random.Range(1, 101) <= 20)
        {
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(new Card(6)); // C级员工加入背包
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
        
    }
    // 对4*4的格子造成40点伤害，15%的概率获得一张B级员工。
    private void A_ranked_staff(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: A_ranked_staff");
        if (e.affectArea.Length > 16 || e.affectArea.Length == 0)
        {
            Debug.LogError("A_ranked_staff传入格子数量错误");
            return;
        }
        DamageMonster(40, e);
        if (UnityEngine.Random.Range(1, 101) <= 15)
        {
            this.GetSystem<IInventorySystem>().SpawnBagCardInBag(new Card(7)); // B级员工加入背包
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 魔虫 在任意2*2的格子上分别召唤1只魔虫。
    private void Devil_bug(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Devil_bug");
        if (e.affectArea.Length > 4 || e.affectArea.Length == 0)
        {
            Debug.LogError("Devil_bug传入格子数量错误");
            return;
        }
        // 遍历传入的grid，召唤魔虫
        foreach (BoxGrid grid in e.affectArea)
        {
            if (grid.occupation != 0) {
                // TODO 生成魔虫 暂无实现函数
                //Game.Console.Input($"GenEnemy 魔虫 {grid.row} {grid.col}");
            }
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 猫头鹰 选择3*3的格子并将他们的时间流逝速率变为慢，持续30秒。
    // 传入选择的格子，流逝速率变为慢，开始协程计时，计时完毕回到原始值，摧毁卡牌。
    private void Owl(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Owl");
        if (e.affectArea.Length > 9 || e.affectArea.Length == 0)
        {
            Debug.LogError("Owl传入格子数量错误");
            return;
        }
        float duration = 30f;
        if(e.viewCard.card.deathEnhancement.statusTimeIncrease != 0)
        {
            duration += e.viewCard.card.deathEnhancement.statusTimeIncrease;
        }
        foreach (BoxGrid grid in e.affectArea)
        {
            GameManager.Instance.buffMan.AddBuff(new BuffChangeGridSpeed(grid, duration, TimeMultiplierEnum.Slow));
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 呱呱蛙 使一个战斗中的敌方单位攻速永久-0.3（最低0.1），如果该单位对声音敏感，则在该场战斗中额外-0.3（在此用buff处理）。
    // 传入战斗中的敌方单位所在的格子，检查是否对声音敏感，添加buff，摧毁卡牌。
    private void Quack_Frog(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Quack_Frog");
        if (e.affectArea.Length != 1)
        {
            Debug.LogError("Quack_Frog传入格子数量错误");
            return;
        }
        Monster monster = pieceSystem.getMonsterById(e.affectArea[0].occupation);
        // 格子上没有敌人
        if (monster == null) 
        {
            Debug.Log("Quack_Frog格子上没有怪物");
            return;
        }
        // 是否对声音敏感, 99999999代表时间无限, 是否在战斗中由BuffQuackFrog检查
        bool soundSensitive = monster.features.Value.Contains(FeatureEnum.SoundSensitive);
        GameManager.Instance.buffMan.AddBuff(new BuffQuackFrog(monster, 99999999, soundSensitive)); 
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }
    
    // 失败品 为一个友方单位恢复35点生命值。
    private void Failure(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Failure");
        if (e.affectArea.Length != 1)
        {
            Debug.LogError("Failure传入格子数量错误");
            return;
        }
        ViewPiece viewpiece = pieceSystem.getViewPieceById(e.affectArea[0].occupation);
        if(viewpiece == null)
        {
            Debug.Log("Failure格子上没有友军");
            return;
        }
        int addHp = 35;
        // 回复量强化检查
        if(e.viewCard.card.deathEnhancement.healthIncrease != 0)
        {
            addHp += e.viewCard.card.deathEnhancement.healthIncrease;
        }
        addHp = viewpiece.hp + addHp > viewpiece.maxHp ? viewpiece.maxHp - viewpiece.hp : addHp;
        viewpiece.hp.Value += addHp;

        // 地图上随机3个怪物造成等于回复量的伤害
        DamageRandMonster(e, addHp, 3);

        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }
    // 爱笑猫（Laughing Cat）使3*3格子内的敌人混乱15秒，混乱中的敌人将随机移动。
    // 传入选择的格子，寻找格子内敌人，施加混乱词条，开始计时，计时完毕删除词条，摧毁卡牌。
    private void Laughing_Cat(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Laughing_Cat");
        if (e.affectArea.Length != 1)
        {
            Debug.LogError("Laughing_Cat传入格子数量错误");
            return;
        }
        foreach(BoxGrid grid in e.affectArea)
        {
            Monster monster = pieceSystem.getMonsterById(grid.occupation);
            if (monster != null)
            {
                float duration = 15f;
                if(e.viewCard.card.deathEnhancement.statusTimeIncrease != 0)
                {
                    duration += e.viewCard.card.deathEnhancement.statusTimeIncrease;
                }
                GameManager.Instance.buffMan.AddBuff(new BuffConfusion(monster, duration));
            }
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 渡鸦（Raven）将4*4格子的时间流逝速率修正为正常，持续40秒。
    // 传入选择的格子，检查强化持续时间，更改时间速率，开始计时，计时完毕回到原始值，摧毁卡牌
    private void Raven(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Raven");
        if (e.affectArea.Length == 0 || e.affectArea.Length > 16)
        {
            Debug.LogError("Raven传入格子数量错误");
            return;
        }
        float duration = 40f;
        if (e.viewCard.card.deathEnhancement.statusTimeIncrease != 0)
        {
            duration += e.viewCard.card.deathEnhancement.statusTimeIncrease;
        }
        foreach (BoxGrid grid in e.affectArea)
        {
            GameManager.Instance.buffMan.AddBuff(new BuffChangeGridSpeed(grid, duration, TimeMultiplierEnum.Normal));
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    // 藤曼（Vine）使1*6格子内的敌人移动速度减半，持续3秒，随后这些格子时间流逝速率永久降低一个级别.
    // 检查是否强化持续时间，每个怪物移速减半，持续时间后，格子流速降低
    private void Vine(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Vine");
        if (e.affectArea.Length == 0 || e.affectArea.Length > 6)
        {
            Debug.LogError("Vine传入格子数量错误");
            return;
        }
        float duration = 3f;
        if (e.viewCard.card.deathEnhancement.statusTimeIncrease != 0)
        {
            duration += e.viewCard.card.deathEnhancement.statusTimeIncrease;
        }
        foreach (BoxGrid grid in e.affectArea)
        {
            Monster monster = pieceSystem.getMonsterById(grid.occupation);
            if (monster != null)
            {
                GameManager.Instance.buffMan.AddBuff(new BuffVine(monster, duration)); // 移速减半
            }
            GameManager.Instance.buffMan.AddBuff(new BuffVine2(grid, 99999999)); // 格子降速，持续时间无限
        }
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }
    // 水精（Water Wizard）为3*3格子内的友方单位回复20点生命值，如果该范围内有水，则回复25点。
    // 传入选择的格子，寻找友方单位，判断是否有水，检查是否强化回血，回血，检查是否强化额外效果，摧毁卡牌。
    private void Water_Wizard(DeathCardCheckEvent e)
    {
        Debug.Log("enter death func: Water_Wizard");
        if (e.affectArea.Length == 0 || e.affectArea.Length > 9)
        {
            Debug.LogError("Water_Wizard传入格子数量错误");
            return;
        }
        int addHp = 20;
        foreach (BoxGrid grid in e.affectArea)
        {
            if(grid.terrain.Value == (int)TerrainEnum.Water)
            {
                addHp = 25;
                break;
            }
        }
        if(e.viewCard.card.deathEnhancement.healthIncrease != 0)
        {
            addHp += e.viewCard.card.deathEnhancement.healthIncrease;
        }
        foreach (BoxGrid grid in e.affectArea)
        {
            ViewPiece viewpiece = pieceSystem.getViewPieceById(grid.occupation);
            if(viewpiece.maxHp - viewpiece.hp <= addHp)
            {
                addHp = viewpiece.maxHp - viewpiece.hp;
            }
            viewpiece.hp.Value += addHp;
        }
        // 对地图上随机3个怪物造成等于治疗量的伤害
        DamageRandMonster(e, addHp, 3);
        this.GetSystem<IHandCardSystem>().SubCard(e.viewCard); // 摧毁卡牌
    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}
