using System;
using UnityEngine;
using Game;
using System.Threading;

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

/// <summary>
/// 若buff目标处于战斗中，atkSpeed减0.3，持续至战斗结束（包括目标死亡的情况）
/// </summary>
public class BuffQuackFrog : BuffToPiece
{
    private float atkSpdDiff;
    Monster monster;

    public BuffQuackFrog(ViewPieceBase _piece, float _dur)
    {
        target = _piece;
        duration = _dur;
        monster = _piece as Monster;
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
        atkSpdDiff = monster.atkSpeed > 0.3f ? 0.3f : monster.atkSpeed;
        monster.atkSpeed.Value -= atkSpdDiff;
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
    private float atkSpdDiff;

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
/// AtkSpeed减半，持续3s
/// </summary>
public class BuffVine : BuffToPiece
{
    private float atkSpdDiff;
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
        atkSpdDiff = monster.atkSpeed / 2;
        monster.atkSpeed.Value -= atkSpdDiff;
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
            monster.atkSpeed.Value += atkSpdDiff;
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
       
    }
}

public enum BuffType
{
    Confusion,
    Poison
}
