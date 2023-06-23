using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DamageNumbersPro;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game
{
    /// <summary>
    /// 棋子基类，理想情况下，敌人和友方都将继承此类
    /// </summary>
    public partial class ViewPieceBase: MonoBehaviour, IController, ICanSendEvent
    {
        // 怪物受到棋子伤害的数字弹出样式资源，棋子受到怪物伤害的数字弹出样式资源
        protected DamageNumberMesh MonsterDamageNumber;
        protected DamageNumberMesh TerrianFireDamageNumber;
        protected DamageNumberMesh TerrianPoisonDamageNumber;
        protected DamageNumberMesh TerrianWaterDamageNumber;
        protected DamageNumberMesh MagicDamageNumber;
        // protected Transform healthBar;

        protected IMapSystem mapSystem;
        protected IMovementSystem movementSystem;
        protected SOCharacterInfo so;
        protected PieceStateEnum stateFlag = PieceStateEnum.Moving;
        protected PieceState state = new PieceStateIdle(null);

        public DirEnum direction = DirEnum.None;
        public bool inCombat; // 是否在战斗中(挨打或者攻击)
        public List<BuffType> listBuffs;    // 目前身上起效的buff

        public Action<PieceMoveReadyEvent> OnPieceMoveReady;
        public Action<PieceMoveFinishEvent> OnPieceMoveFinish;
        public Action<PieceAttackStartEvent> OnPieceAttackStart;
        public Action<PieceAttackEndEvent> OnPieceAttackEnd;
        public Action<PieceUnderAttackEvent> OnPieceUnderAttack;

        public Animator pieceAnimator; // 动画组件
        protected Coroutine movementCoroutine; // 移动协程
        public bool isFacingRight = true; // 棋子目前预设体是否朝向右侧

        public bool timeStop; // 如果在异化技能中时间暂停

        public List<BoxGrid> pieceGrids { get; protected set; } = new List<BoxGrid>();
        // 经过所有占地格子计算出来的时间流速
        public float crtTimeMultiplier
        {
            get
            {
                // 如果在异化技能中时间暂停
                if(timeStop) 
                {
                    return 0.00000001f;
                }
                float val = 0f;
                foreach (var grid in pieceGrids)
                {
                    val += Extensions.ToTimeMultiplierFloat(grid.timeMultiplier);
                }
                val /= pieceGrids.Count;
                return val;
            }
            
        }

        #region 棋子数据
        public RarityEnum rarity; // 稀有度 0 白 -- 4 橙
        public int generalId; // 基础ID，辨认品种
        public int pieceId; // 棋子ID，每个棋子独一份
        public (int, int) pieceSize; // 棋子尺寸

        public BindableProperty<float> moveSpeed; // 移动速度
        public BindableProperty<int> hp; // 当前生命值
        public BindableProperty<int> maxHp; // 最大生命值
        public BindableProperty<float> atkSpeed; // 攻速
        public BindableProperty<float> atkDmg; // 攻击力
        public BindableProperty<float> defense; // 防御力
        public BindableProperty<float> accuracy; // 命中率
        public BindableProperty<int> atkRange; // 射程
        public BindableProperty<List<FeatureEnum>> features; // 特性
        public BindableProperty<List<DirEnum>> dirs; // 可移动方向
        public BindableProperty<bool> isAttacking; // 是否在发起攻击
        public BindableProperty<bool> isDying; // 是否正在死亡中                         
        #endregion

        // public List<BoxGrid> attadkRangeGrids = new List<BoxGrid>(); // todo

        protected virtual void Start()
        {
            InitBind();
            
            mapSystem = this.GetSystem<IMapSystem>();
            movementSystem = this.GetSystem<IMovementSystem>();
            
            // 棋子自身也会监听棋子（包括自己）
            OnPieceMoveReady += OnMoveReadyEvent;
            OnPieceMoveFinish += OnMoveFinishEvent;
            OnPieceAttackStart += OnAttackStartEvent;
            OnPieceAttackEnd += OnAttackEndEvent;
            OnPieceUnderAttack += OnUnderAttackEvent;
            
            hp.Register(e => OnCurrHpChanged(e));
            MonsterDamageNumber = Resources.Load("Prefabs/Damage Number Prefab/Monster Damage").GetComponent<DamageNumberMesh>();
            TerrianFireDamageNumber = Resources.Load("Prefabs/Damage Number Prefab/Terrian Fire Damage").GetComponent<DamageNumberMesh>();
            TerrianPoisonDamageNumber = Resources.Load("Prefabs/Damage Number Prefab/Terrian Poison Damage").GetComponent<DamageNumberMesh>();
            TerrianWaterDamageNumber = Resources.Load("Prefabs/Damage Number Prefab/Terrian Water Damage").GetComponent<DamageNumberMesh>();
            MagicDamageNumber = Resources.Load("Prefabs/Damage Number Prefab/Magic Damage").GetComponent<DamageNumberMesh>();

            timeStop = false;
        }

        /// <summary>
        /// 当前生命值被改变
        /// </summary>
        /// <param name="e">新生命值</param>
        protected void OnCurrHpChanged(int e)
        {
            healthBar.SetBarFillAmount((float)e / maxHp);
        }

        public virtual void SetGrids(List<BoxGrid> grids)
        {
            foreach (var grid in grids)
            {
                pieceGrids.Add(grid);
            }
            transform.position = GetGridsCenterPos();
        }

        /// <summary>
        /// 棋子生成后，设置初始状态
        /// </summary>
        public virtual void InitState()
        {

        }

        protected void Update()
        {
            state.Update();
        }

        protected void ChangeStateTo(PieceState newState)
        {
            Debug.Log($"change state from: {state.stateEnum} to: {newState.stateEnum}");
            state.ExitState();
            state = newState;
            state.EnterState();
            stateFlag = state.stateEnum;
        }

        /// <summary>
        /// 棋子所占格子内是否包含了传入的格子类型
        /// </summary>
        /// <param name="terrianType"></param>
        /// <returns></returns>
        public bool PieceOnTerrianType(TerrainEnum terrianType)
        {
            foreach (BoxGrid grid in pieceGrids)
            {
                if (grid.terrain.Value == (int)terrianType)
                {
                    return true;
                }
            }
            return false;
        }

        public PieceStateEnum GetPieceState()
        {
            return stateFlag;
        }

        public PieceEnemyMovingState GetEnemyMovingState()
        {
            return state as PieceEnemyMovingState;
        }

        public PieceFriendMovingState GetFriendMovingState()
        {
            return state as PieceFriendMovingState;
        }

        protected Vector3 GetGridsCenterPos()
        {
            if (pieceGrids.Count == 0) return transform.position;
            Vector3 centerPos = Vector3.zero;
            foreach (var grid in pieceGrids)
            {
                centerPos += grid.transform.position;
                
            }
            centerPos /= pieceGrids.Count;
            return centerPos;
        }
        
        protected virtual bool CheckIfOneGridCanMove(BoxGrid grid)
        {
            // 通用判断
            // 1.是否格子已被占用，且不是自己当前占用的格子
            if (!pieceGrids.Contains(grid) && !mapSystem.GridCanMoveTo(grid))
                return false;

            // 某些判断...
            // 海洋恐惧症
            if (!FeatureController.instance.Hydrophobia(this, grid)) return false;
            
            return true;
        }

        /// <summary>
        /// 移动时执行动画的协程
        /// </summary>
        /// <param name="newGridTransPos">新位置</param>
        /// <param name="duration">持续时间</param>
        /// <returns></returns>
        protected IEnumerator MoveToTarget(Vector3 newGridTransPos, float duration = 0.5f)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                transform.position = Vector3.Lerp(startPosition, newGridTransPos, t);
                yield return null;
            }

            if (pieceAnimator != null)
            {
                pieceAnimator.SetBool("isMove", false);
            }

            movementCoroutine = null;
            OnMoveFinish();
        }

        protected void OnMoveFinish()
        {
            // 发送结束移动事件
            GetArchitecture().SendEvent<PieceMoveFinishEvent>(new PieceMoveFinishEvent() { viewPieceBase = this });
        }

        // 发起攻击
        public virtual void Attack()
        {
            
        }

        // 受到攻击，返回是否死亡
        public virtual bool Hit(int damage, ViewPieceBase attacker)
        {
            // 收到攻击数据...
            // 进行各种效果计算...
            // 获取实际伤害
            return false;
        }

        // 受到棋子以外的东西的伤害，地块，死面之类的，包含掉血后的死亡检查
        public void TakeDamage(int damage)
        { 
            hp.Value -= damage;
            MagicDamageNumber.Spawn(this.Position(), damage);
            if (hp <= 0)
            {
                this.GetSystem<IPieceBattleSystem>().EndBattle(this); // 不确定是不是需要
                // 再从棋子系统中注销
                this.GetSystem<IPieceSystem>().RemovePiece(this);
                // 最后处理自身的死亡
                Die();
            }
        }
        // 重载，被地块buff调用，弹出不同资源的伤害数字
        public void TakeDamage(int damage, TerrainEnum terrian)
        {
            hp.Value -= damage;
            if (terrian == TerrainEnum.Fire)
            {
                TerrianFireDamageNumber.Spawn(this.Position(), damage);
            }else if(terrian == TerrainEnum.Poison)
            {
                TerrianPoisonDamageNumber.Spawn(this.Position(), damage);
            }else if(terrian == TerrainEnum.Water)
            {
                TerrianWaterDamageNumber.Spawn(this.Position(), damage);
            }
            
            if (hp <= 0)
            {
                this.GetSystem<IPieceBattleSystem>().EndBattle(this); // 不确定是不是需要
                // 再从棋子系统中注销
                this.GetSystem<IPieceSystem>().RemovePiece(this);
                // 最后处理自身的死亡
                Die();
            }
        }
        public virtual void Die()
        {
            state = new PieceStateIdle(this);
            foreach (var grid in pieceGrids)
            {
                grid.occupation = 0;
                grid.gridStatus.Value = GridStatusEnum.Unoccupied;
            }
            pieceGrids.Clear();
            
            Destroy(gameObject);
        }

        /// <summary>
        /// 更改方向后可能需要旋转棋子预设体的朝向
        /// </summary>
        /// <param name="newDir">新方向</param>
        /// <returns></returns>
        public void PieceFlip(DirEnum newDir)
        {
            if (isFacingRight && Extensions.leftDirs.Contains(newDir) ||
                !isFacingRight && Extensions.rightDirs.Contains(newDir))
            {
                Vector3 currentEulerAngles = pieceAnimator.transform.eulerAngles;
                Vector3 newEulerAngles = new Vector3(
                    currentEulerAngles.x,
                    (currentEulerAngles.y + 180) % 360,
                    currentEulerAngles.z
                );
                pieceAnimator.transform.rotation = Quaternion.Euler(newEulerAngles);
                isFacingRight = !isFacingRight;
            }

        }

        /// <summary>
        /// 攻击时可能需要旋转棋子预设体的朝向
        /// </summary>
        /// <param name="defender">挨打棋子</param>
        public void PieceFlip(ViewPieceBase defender)
        {
            if ((isFacingRight && defender.pieceGrids[0].col < pieceGrids[0].col) ||
                (!isFacingRight && defender.pieceGrids[0].col > pieceGrids[0].col))
            {
                Vector3 currentEulerAngles = pieceAnimator.transform.eulerAngles;
                Vector3 newEulerAngles = new Vector3(
                    currentEulerAngles.x,
                    (currentEulerAngles.y + 180) % 360,
                    currentEulerAngles.z
                );
                pieceAnimator.transform.rotation = Quaternion.Euler(newEulerAngles);
                isFacingRight = !isFacingRight;
            }
        }

        protected virtual void OnMoveReadyEvent(PieceMoveReadyEvent e)
        {

        }
        
        protected virtual void OnMoveFinishEvent(PieceMoveFinishEvent e)
        {
            if (PieceOnTerrianType(TerrainEnum.Fire))
            {
                GameManager.Instance.buffMan.AddBuff(new BuffTerrianFire(this));
            }else if (PieceOnTerrianType(TerrainEnum.Poison))
            {
                GameManager.Instance.buffMan.AddBuff(new BuffTerrianPoison(this));
            }
        }

        protected virtual void OnAttackStartEvent(PieceAttackStartEvent e)
        {

        }

        protected virtual void OnAttackEndEvent(PieceAttackEndEvent e)
        {
            
        }

        protected virtual void OnUnderAttackEvent(PieceUnderAttackEvent e)
        {

        }

        public bool IsAttacking()
        {
            return stateFlag == PieceStateEnum.Attacking;
        }

        public void SetTouchAreaEnable(bool isEnable)
        {
            
        }

        /// <summary>
        /// 以在挨打方头上播放标记动画的形式展示攻击
        /// </summary>
        /// <param name="animationInstance"></param>
        /// <returns></returns>
        protected IEnumerator PlayAttackAnimByMarking(GameObject animationInstance)
        {
            yield return new WaitForSeconds(0.2f);
            animationInstance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            animationInstance.transform.localPosition = new Vector3(-0.5f, 2, 0);

            // 等待动画播放完毕
            yield return new WaitForSeconds(animationInstance.GetComponentInChildren<Animator>()
                .runtimeAnimatorController.animationClips[0].length);

            // 销毁实例化的预制体
            GameObject.Destroy(animationInstance);
        }

        /// <summary>
        /// 以播放攻击方自身攻击动画的形式展示攻击
        /// </summary>
        /// <param name="animationInstance"></param>
        /// <returns></returns>
        protected IEnumerator PlayAttackAnimByAction(ViewPieceBase attacker)
        {
            attacker.pieceAnimator?.SetBool("isAttack", true);
            yield return new WaitForSeconds(attacker.pieceAnimator.GetCurrentAnimatorStateInfo(0).length);
            attacker.pieceAnimator?.SetBool("isAttack", false);
            yield return null;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}