using System;
using UnityEngine;
using Game;
using System.Threading;
using QFramework;
using static System.Math;
using static UnityEngine.GraphicsBuffer;
using Sirenix.OdinInspector.Editor.StateUpdaters;

public abstract class BuffBase
{
    // 不同对象：格子、棋子……
    // 不同触发条件
    // 不同时间间隔

    public float duration; // buff持续时间

    public float leftTime; // buff剩余持续时间

    // 可触发次数等

    /// <summary>
    /// 进行buff是否能够生成的条件判断
    /// </summary>
    /// <returns>是否有生成buff的条件</returns>
    public abstract bool OnBuffCreate();

    /// <summary>
    /// buff开始时的效果，如扣血等
    /// </summary>
    public abstract void OnBuffStart();

    /// <summary>
    /// buff持续时间内每一帧调用，可判断是否出现了buff需要移除的条件（如目标被摧毁）
    /// </summary>
    public abstract void OnBuffRefresh();

    /// <summary>
    /// buff移除时的效果，如恢复之前扣除的血量
    /// </summary>
    public abstract void OnBuffRemove();
}

public abstract class BuffToPiece : BuffBase
{
    public ViewPieceBase target;
}

public abstract class BuffToGrid : BuffBase
{
    public BoxGrid target;
}

public class BuffTerrianFire : BuffToPiece
{
    // 每层buff掉最大生命值百分比
    private float damagePerLevel;
    // 当前buff层数
    private int curLevel;
    // 检查buff层数增加的计时器
    private float buffEnhanceTimer;
    // 每秒触发1次扣血
    private float damageTriggerTime;
    // 当前层buff的计时器
    private float curLevelTimer;
    public BuffTerrianFire(ViewPieceBase _piece)
    {
        target = _piece;
        duration = 5f;
        leftTime = 5f;
        damagePerLevel = 0.01f;
        curLevel = 1;
        buffEnhanceTimer = 0;
        damageTriggerTime = 0;
        curLevelTimer = 0;
    }

    

    public override bool OnBuffCreate()
    {
        // 如果已经有炽焰buff，不需要添加buff，由原有炽焰buff的refresh逻辑叠加层数
        if (target.listBuffs.Contains(BuffType.Terrian_Fire))
        {
            return false;
        }
        return target.PieceOnTerrianType(TerrainEnum.Fire);
    }
    public override void OnBuffStart()
    {
        Debug.Log("BuffTerrianFire: start");
        target.listBuffs.Add(BuffType.Terrian_Fire);
        target.takeDamage((int)(damagePerLevel * curLevel * target.maxHp), TerrainEnum.Fire);
    }

    public override void OnBuffRefresh()
    {
        Debug.Log("BuffTerrianFire: refresh");
        // 如果当前在火格子上，每5秒加1层buff，每加1层 持续时间和伤害都递增
        // 如果不在，剩余时间减少
        if (target != null && target.PieceOnTerrianType(TerrainEnum.Fire))
        {
            buffEnhanceTimer += Time.deltaTime;
            if(buffEnhanceTimer >= 5)
            {
                curLevel++;
                duration += 5f;
                leftTime = duration;
                buffEnhanceTimer = 0;
                curLevelTimer = 0;
            }
        }
        else
        {
            leftTime -= Time.deltaTime;
            curLevelTimer += Time.deltaTime;
            buffEnhanceTimer = 0;
            // 如果当前buff等级的剩余时间结束，buff降级
            if(curLevelTimer >= 5)
            {
                curLevel--;
                duration -= 5f;
                curLevelTimer = 0;
            }
            // 如果时间结束 移除buff
            if(target == null || leftTime <= 0)
            {
                GameManager.Instance.buffMan.RemoveBuff(this);
                return;
            }
        }
        // 每1秒结算1次伤害，如果当前帧触发buff增加/减少，就按触发后的结果算
        damageTriggerTime += Time.deltaTime;
        if(damageTriggerTime >= 1)
        {
            Debug.Log($"BuffTerrianFire伤害：{damagePerLevel * curLevel * target.maxHp}, {(int)(damagePerLevel * curLevel * target.maxHp)}");
            // 伤害为0-1之间的时候取1
            int dmg = ((int)(damagePerLevel * curLevel * target.maxHp)) == 0 ? 1 : (int)(damagePerLevel * curLevel * target.maxHp);
            target.takeDamage(dmg, TerrainEnum.Fire);
            damageTriggerTime = 0;
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffTerrianFire: remove");
        if (target != null)
        {
            target.listBuffs.Remove(BuffType.Terrian_Fire);
        }
    }
}

public class BuffTerrianPoison : BuffToPiece
{
    // 每层buff掉血量
    private int damagePerLevel;
    // 当前buff层数
    private int curLevel;
    // 检查buff层数增加的计时器
    private float buffEnhanceTimer;
    // 每秒触发1次扣血
    private float damageTriggerTime;
    // 当前层buff的计时器
    private float curLevelTimer;
    public BuffTerrianPoison(ViewPieceBase _piece)
    {
        target = _piece;
        duration = 5f;
        leftTime = 5f;
        damagePerLevel = 1;
        curLevel = 1;
        buffEnhanceTimer = 0;
        damageTriggerTime = 0;
        curLevelTimer = 0;
    }
    public override bool OnBuffCreate()
    {
        // 如果已经有毒沼buff，不需要添加buff，由原有毒沼buff的refresh逻辑叠加层数
        if (target.listBuffs.Contains(BuffType.Terrian_Poison))
        {
            return false;
        }
        return target.PieceOnTerrianType(TerrainEnum.Poison);
    }
    public override void OnBuffStart()
    {
        Debug.Log("BuffTerrianPoison: start");
        target.listBuffs.Add(BuffType.Terrian_Poison);
        target.takeDamage(damagePerLevel * curLevel, TerrainEnum.Poison);
    }

    public override void OnBuffRefresh()
    {
        Debug.Log("BuffTerrianPoison: refresh");
        // 如果当前在毒沼格子上，每5秒加1层buff，每加1层 持续时间和伤害都递增
        // 如果不在，剩余时间减少
        if (target != null && target.PieceOnTerrianType(TerrainEnum.Poison))
        {
            buffEnhanceTimer += Time.deltaTime;
            if (buffEnhanceTimer >= 5)
            {
                curLevel++;
                duration += 5f;
                leftTime = duration;
                buffEnhanceTimer = 0;
                curLevelTimer = 0;
            }
        }
        else
        {
            leftTime -= Time.deltaTime;
            curLevelTimer += Time.deltaTime;
            buffEnhanceTimer = 0;
            // 如果当前buff等级的剩余时间结束，buff降级
            if (curLevelTimer >= 5)
            {
                curLevel--;
                duration -= 5f;
                curLevelTimer = 0;
            }
            // 如果时间结束 移除buff
            if (target == null || leftTime <= 0)
            {
                GameManager.Instance.buffMan.RemoveBuff(this);
                return;
            }
        }
        // 每1秒结算1次伤害，如果当前帧触发buff增加/减少，就按触发后的结果算
        damageTriggerTime += Time.deltaTime;
        if (damageTriggerTime >= 1)
        {
            Debug.Log($"BuffTerrianPoison伤害：{damagePerLevel * curLevel}");
            target.takeDamage(damagePerLevel * curLevel, TerrainEnum.Poison);
            damageTriggerTime = 0;
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffTerrianPoison: remove");
        if (target != null)
        {
            target.listBuffs.Remove(BuffType.Terrian_Poison);
        }
    }
}

public class BuffTerrianWater : BuffToPiece
{
    private float moveSpeedDown;
    public BuffTerrianWater(ViewPieceBase _piece)
    {
        target = _piece;
        moveSpeedDown = 0.5f;
    }
    public override bool OnBuffCreate()
    {
        // 如果在水潭上并且已经有水潭buff就减移速，如果移速小于10就加溺亡buff
        // 如果在水潭上没有水潭buff，return true去添加水潭buff
        if(target != null && target.PieceOnTerrianType(TerrainEnum.Water))
        {   
            if (target.listBuffs.Contains(BuffType.Terrian_Water))
            {
                target.moveSpeed.Value -= moveSpeedDown;
                if(target.moveSpeed.Value < 10)
                {
                    GameManager.Instance.buffMan.AddBuff(new BuffDrowning(target));
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    public override void OnBuffStart()
    {
        Debug.Log("BuffTerrianWater OnBuffStart: Add Terrian_Water");
        target.listBuffs.Add(BuffType.Terrian_Water);
    }

    public override void OnBuffRefresh()
    {
        Debug.Log("BuffTerrianWater refresh");
        // 离开水潭就移除水潭buff
        if (target != null && !target.PieceOnTerrianType(TerrainEnum.Water)) 
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffTerrianWater: remove");
        if (target != null)
        {
            target.listBuffs.Remove(BuffType.Terrian_Water);
        }
    }
}
/// <summary>
/// 溺亡Buff
/// </summary>
public class BuffDrowning : BuffToPiece
{
    // 最大生命值百分比的伤害
    private float damagePercent;
    private float timer;
    public BuffDrowning(ViewPieceBase _piece)
    {
        target = _piece;
        damagePercent = 0.1f;
        timer = 0;
        leftTime = 99999999;
        duration = 99999999;
    }
    // 移速小于10并且在水潭上就获得溺亡buff
    public override bool OnBuffCreate()
    {
        // 已携带溺亡就不叠加
        if (target == null || target.listBuffs.Contains(BuffType.Drowning))
        {
            return false;
        }
        else if (target != null && target.moveSpeed < 10 && target.PieceOnTerrianType(TerrainEnum.Water))
        {
            return true;
        }   
        else
        {
            return false;
        }
    }
    public override void OnBuffStart()
    {
        target.takeDamage((int) (damagePercent * target.maxHp), TerrainEnum.Water);
    }

    public override void OnBuffRefresh()
    {
        Debug.Log("BuffDrowning: refresh");
        if(target == null)
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
        timer += Time.deltaTime;
        // 每3秒造成1次伤害
        if(timer >= 3) 
        {
            target.takeDamage((int)(damagePercent * target.maxHp), TerrainEnum.Water);
            timer = 0;
        }
    }

    public override void OnBuffRemove()
    {
        // 溺亡buff无法移除
        Debug.Log("BuffDrowning: remove");
    }
}


/// <summary>
/// 若buff目标处于战斗中，atkSpeed减0.3，持续至战斗结束（包括目标死亡的情况）
/// </summary>
public class BuffQuackFrog : BuffToPiece
{
    private float atkSpdDiff;
    Monster monster;
    bool soundSensitive;
    public BuffQuackFrog(Monster _piece, float _dur, bool _soundSensitive)
    {
        target = _piece;
        duration = _dur;
        monster = _piece;
        soundSensitive = _soundSensitive;
    }

    public override bool OnBuffCreate()
    {
        // 只对处于战斗中的单位附加buff
        if (!target.inCombat)
        {
            return false;
        }

        return true;
    }

    public override void OnBuffStart()
    {
        Debug.Log("BuffFrog: start");
        if (soundSensitive)
        {
            atkSpdDiff = monster.atkSpeed > 0.6f ? 0.6f : monster.atkSpeed;
            monster.atkSpeed.Value -= atkSpdDiff;
        }
        else
        {
            atkSpdDiff = monster.atkSpeed > 0.3f ? 0.3f : monster.atkSpeed;
            monster.atkSpeed.Value -= atkSpdDiff;
        }
        
    }

    public override void OnBuffRefresh()
    {
        if (target == null || !target.inCombat)
        {
            Debug.Log("target died or left fight, to remove buff");

            // 移除buff
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffFrog: remove");

        if (target != null)
        {
            monster.atkSpeed.Value += atkSpdDiff;
        }
    }
}

/// <summary>
/// 改变格子时间流速（特殊格子可能不受影响）
/// </summary>
public class BuffChangeGridSpeed : BuffToGrid
{
    private TimeMultiplierEnum timeBefore;
    private TimeMultiplierEnum timeAfter;

    public BuffChangeGridSpeed(BoxGrid _grid, float _dur, TimeMultiplierEnum destTimeMulti)
    {
        target = _grid;
        duration = _dur;
        leftTime = duration;
        timeAfter = destTimeMulti;
    }

    public override bool OnBuffCreate()
    {
        return true;
    }

    public override void OnBuffStart()
    {
        Debug.Log("BuffChangeGridSpeed: start");
        timeBefore = target.timeMultiplier;
        target.timeMultiplier.Value = timeAfter;
    }

    public override void OnBuffRefresh()
    {
        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffChangeGridSpeed remove");

        if (target != null)
        {
            target.timeMultiplier.Value = timeBefore;
        }
    }
}

/// <summary>
/// 混乱，移动方向随机
/// </summary>
public class BuffConfusion : BuffToPiece
{

    public BuffConfusion(ViewPieceBase _piece, float _dur)
    {
        target = _piece;
        duration = _dur;
        leftTime = duration;
    }

    public override bool OnBuffCreate()
    {
        // 心理医生
        if (target.features.Value.Contains(FeatureEnum.Psychologist)) return false;

        // 先查找该棋子是否已有该类型buff，若有，不再挂新的，而是叠加时间
        if (target.listBuffs.Contains(BuffType.Confusion))
        {
            foreach (var buff in GameManager.Instance.buffMan.listBuffs)
            {
                if (buff is BuffConfusion)
                {
                    var pieceBuff = buff as BuffConfusion;
                    if (pieceBuff.target == target)
                    {
                        Debug.Log($"BuffConfusion: already exist, add leftTime {pieceBuff.leftTime} + {leftTime}");
                        pieceBuff.duration += duration;
                        pieceBuff.leftTime += leftTime;
                        return false;
                    }
                }
            }
        }
        
        return true;
    }

    public override void OnBuffStart()
    {
        Debug.Log("BuffConfusion: start");
        target.listBuffs.Add(BuffType.Confusion);
    }

    public override void OnBuffRefresh()
    {
        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffConfusion remove");

        if (target != null)
        {
            target.listBuffs.Remove(BuffType.Confusion);
        }
    }
}

/// <summary>
/// 藤蔓buff，moveSpeed减半，持续3s，
/// </summary>
public class BuffVine : BuffToPiece
{
    private float moveSpeedDiff;
    private Monster monster;

    public BuffVine(ViewPieceBase _piece, float _dur)
    {
        target = _piece;
        duration = _dur;
        leftTime = duration;
        monster = _piece as Monster;
    }

    public override bool OnBuffCreate() { return true; }

    public override void OnBuffStart()
    {
        Debug.Log("BuffVein1: start");
        moveSpeedDiff = monster.moveSpeed / 2;
        monster.moveSpeed.Value -= moveSpeedDiff;
    }

    public override void OnBuffRefresh()
    {
        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffVein1 remove");
        if(target != null)
            monster.atkSpeed.Value += moveSpeedDiff;
    }
}

/// <summary>
/// 等待三秒BuffVine结束后对格子时间流速降级
/// </summary>
public class BuffVine2 : BuffToGrid
{
    public BuffVine2(BoxGrid _grid, float _dur)
    {
        target = _grid;
        duration = _dur;
        leftTime = duration;
    }

    public override bool OnBuffCreate()
    {
        return true;
    }

    public override void OnBuffStart()
    {
        Debug.Log("BuffVein2GridSpeed: start");
    }

    public override void OnBuffRefresh()
    {
        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffVein2 remove");

        if (target != null)
        {
            target.LevelDownTimeMultiplier();
        }
    }
}

/// <summary>
/// 地震作弊技怪物移动速度归零效果
/// </summary>
public class BuffEarthquake : BuffToPiece
{
    private float moveSpdDiff;
    private Monster monster;

    public BuffEarthquake(ViewPieceBase _piece, float _dur)
    {
        target = _piece;
        duration = _dur;
        leftTime = duration;
        monster = _piece as Monster;
    }

    public override bool OnBuffCreate() { return true; }

    public override void OnBuffStart()
    {
        moveSpdDiff = monster.moveSpeed;
        monster.moveSpeed.Value = 999;
        
    }

    public override void OnBuffRefresh()
    {
        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        if (target != null)
            monster.moveSpeed.Value = moveSpdDiff;
    }
}

/// <summary>
/// 中毒
/// </summary>
public class DebuffPoison : BuffToPiece
{
    private float activatedTime; // 已生效时间，每到1产生伤害并重置
    private Monster monster;
    private ViewPiece viewPiece;

    public DebuffPoison(ViewPieceBase _piece, float _dur)
    {
        target = _piece;
        duration = _dur;
        leftTime = duration;
        activatedTime = 0;
    }

    public override bool OnBuffCreate() {

        // 心理医生
        if (target.features.Value.Contains(FeatureEnum.Psychologist)) return false;

        // 先查找该棋子是否已有该类型buff，若有，不再挂新的，而是叠加时间
        if (target.listBuffs.Contains(BuffType.Poison))
        {
            foreach (var buff in GameManager.Instance.buffMan.listBuffs)
            {
                if (buff is DebuffPoison)
                {
                    var pieceBuff = buff as DebuffPoison;
                    if (pieceBuff.target == target)
                    {
                        Debug.Log($"DebuffPoison: already exist, add leftTime {pieceBuff.leftTime} + {leftTime}");
                        pieceBuff.duration += duration;
                        pieceBuff.leftTime += leftTime;
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public override void OnBuffStart()
    {
        target.listBuffs.Add(BuffType.Poison);
        if (monster != null) monster.hp.Value = (int) (0.9f * monster.hp);
        if (viewPiece != null) viewPiece.hp.Value = (int)(0.9f * viewPiece.hp);
    }

    public override void OnBuffRefresh()
    {
        leftTime -= Time.deltaTime;
        activatedTime += Time.deltaTime;
        if (activatedTime >= 1)
        {
            if (monster != null) monster.hp.Value = (int)(0.9f * monster.hp);
            if (viewPiece != null) viewPiece.hp.Value = (int)(0.9f * viewPiece.hp);
            activatedTime = 0f;
        }
        if (leftTime <= 0)
        {
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        if (target != null)
        {
            target.listBuffs.Remove(BuffType.Poison);
        }
    }
}

/// <summary>
/// 标记一个友军，每当他受到伤害时，获得2金币
/// </summary>
public class BuffLightNavyQuillPen : BuffToPiece, ICanRegisterEvent
{
    public BuffLightNavyQuillPen(ViewPiece _piece)
    {
        target = _piece;
    }

    public override bool OnBuffCreate()
    {
        return true;
    }

    public override void OnBuffStart()
    {
        Debug.Log("BuffLightNavyQuillPen: start");

        GameManager.Instance.buffMan.SetBuffIcon((ViewPiece) target, "使用型道具-52");

        this.RegisterEvent<PieceHitFinishEvent>(e => 
        {
            if (e.piece == target)
            { 
                PlayerManager.Instance.player.AddGold(2); 
            }
        }
        );

    }

    public override void OnBuffRefresh()
    {
        if (target == null)
        {
            Debug.Log("target died, to remove buff");

            // 移除buff
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffLightNavyQuillPen: remove");

    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}

/// <summary>
/// 标记一个敌人，友方对其进行攻击时吸血10%，无法标记BOSS
/// </summary>
public class BuffNavyQuillPen : BuffToPiece, ICanRegisterEvent
{
    public BuffNavyQuillPen(Monster _piece)
    {
        target = _piece;
    }

    public override bool OnBuffCreate()
    {
        return true;
    }

    public override void OnBuffStart()
    {
        Debug.Log("BuffNavyQuillPen: start");

        GameManager.Instance.buffMan.SetBuffIcon((Monster)target, "使用型道具-51");

        this.RegisterEvent<PieceHitFinishEvent>(e =>
        {
            if (e.piece == target && e.attacker != null)
            {
                e.attacker.hp.Value += (int)(e.damage * 0.1f);
                if (e.attacker.hp > e.attacker.maxHp) e.attacker.hp = e.attacker.maxHp;
            }
        }
        );

    }

    public override void OnBuffRefresh()
    {
        if (target == null)
        {
            Debug.Log("target died, to remove buff");

            // 移除buff
            GameManager.Instance.buffMan.RemoveBuff(this);
        }
    }

    public override void OnBuffRemove()
    {
        Debug.Log("BuffNavyQuillPen: remove");

    }

    public IArchitecture GetArchitecture()
    {
        return GameEntry.Interface;
    }
}



public enum BuffType
{
    Confusion,
    Poison,
    Terrian_Fire,
    Terrian_Water,
    Terrian_Poison,
    Drowning
}
