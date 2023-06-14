using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 用来实际生成各种卡牌/棋子
    /// </summary>
    public class Spawner : MonoBehaviour, IController, ICanSendEvent
    {
        // 战斗场景中持续抽卡的协程
        private Coroutine drawCardCoroutine;

        /// <summary>
        /// 获取Architecture 每个IController都要写
        /// </summary>
        /// <returns></returns>
        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }


        void Awake()
        {
            // 监听怪物生成事件
            this.RegisterEvent<SpawnMonsterEvent>(OnSpawnMonsterEvent)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            // 监听怪物持续生成事件
            this.RegisterEvent<ConstantSpawnMonsterEvent>(data => 
            { 
                StartCoroutine(OnConstantSpawnMonsterEvent(data)); 
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 监听创建卡牌事件
            this.RegisterEvent<SpawnCardEvent>(data =>
            {
                OnSpawnCardEvent(data);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 监听持续抽取卡牌事件
            this.RegisterEvent<RefillHandCardEvent>(data =>
            {
                drawCardCoroutine = StartCoroutine(OnHandCardRefillEvent(data));
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 监听亡灵生成事件
            this.RegisterEvent<SpawnUndeadEvent>(data =>
            {
                SpawnUndead(data.undeadSpawnPositionX, data.undeadSpawnPositionY);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // 监听战斗胜利/失败事件
            this.RegisterEvent<CombatDefeatEvent>((data) => 
            {
                StopCoroutine(drawCardCoroutine);
            }
            ).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<CombatVictoryEvent>((data) =>
            {
                StopCoroutine(drawCardCoroutine);
            }
            ).UnRegisterWhenGameObjectDestroyed(gameObject);

        }

        public void SpawnUndead(int x, int y)
        {
            // 棋子实例化，挂载组件，部分初始化
            GameObject pieceGO = this.GetSystem<IPieceGeneratorSystem>().CreatePieceFriend();
            var viewPiece = pieceGO.AddComponent<ViewPiece>();
            // 接收数据，初始化显示
            viewPiece.SetDataWithCard(new Card(0));
            BoxGrid grid = this.GetSystem<IMapSystem>().Grids()[x, y];
            List<BoxGrid> grids = new List<BoxGrid>();
            grids.Add(grid);
            viewPiece.SetGrids(grids);
            viewPiece.InitState();

            this.GetSystem<IPieceSystem>().pieceFriendList.Add(viewPiece);
            this.GetSystem<IPieceBattleSystem>().CheckAllPieceAtkRange();

        }

        /// <summary>
        /// 收到持续抽卡事件后开始持续抽卡
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerator OnHandCardRefillEvent(RefillHandCardEvent data)
        {
            IInventorySystem inventorySystem = this.GetSystem<IInventorySystem>();
            IHandCardSystem handCardSystem = this.GetSystem<IHandCardSystem>();
            while(GameManager.Instance.gameSceneMan.GetCurrentSceneName() == "Combat")
            {
                if (inventorySystem.GetBagCardList().Count != 0 &&
                    handCardSystem.handCardList.Value.Count < handCardSystem.maxCardCount)
                {
                    Card card = inventorySystem.DrawCard();
                    this.SendCommand<AddHandCardCommand>(new AddHandCardCommand(card));
                }
                yield return new WaitForSeconds(data.drawCardCooldown);
            }
        }

        /// <summary>
        /// 收到卡牌生成事件后处理
        /// </summary>
        private void OnSpawnCardEvent(SpawnCardEvent data)
        {
            GameObject cardItem = (GameObject)Instantiate(Resources.Load("Prefabs/CardItem"));
            ViewBagCard viewCard = cardItem.AddComponent<ViewBagCard>();
            viewCard.card = new Card(data.cardId);
            this.GetSystem<ISpawnSystem>().SetLastSpawnedCard(cardItem);
        }

        /// <summary>
        /// 收到怪物生成事件后处理
        /// </summary>
        /// <param name="data"></param>
        private void OnSpawnMonsterEvent(SpawnMonsterEvent data)
        {
            // 拦下不合法怪物名字
            var so = Resources.Load<SOMonsterBase>("ScriptableObjects/Monsters/" + data.name);
            if (so == null)
            {
                Debug.LogError($"no monster so: {data.name}");
                return;
            }
            
            var grid = this.GetSystem<IMapSystem>().Grids();
            Transform gridTransform = grid[data.row, data.col].transform;
            GameObject piece = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyPiece"));
            // 置于MonsterPosition父物体下，该物体比地图略高一些
            piece.transform.SetParent(GameObject.Find("MonsterPosition").transform);
            piece.transform.position = gridTransform.position;
            Monster monster = piece.GetComponent<Monster>();
            monster.data = so;

            monster.touchArea = monster.transform.Find("Root/SpritePiece").gameObject;
            monster.touchArea.GetComponent<BoxCollider2D>().size = monster.data.monsterSprite.bounds.size;
            monster.collider2d = monster.transform.Find("Root/SpritePiece").GetComponent<BoxCollider2D>();
            
            //动画部分
            GameObject animGO = monster.data.GetChildAnim();
            if (animGO != null)
            {
                GameObject monsterAnim = Instantiate(animGO);
                piece.GetComponent<Monster>().animator = monsterAnim.GetComponent<Animator>();
                monsterAnim.transform.SetParent(piece.transform);
                monsterAnim.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                monsterAnim.transform.localPosition = new Vector3(0, 0.25f, -0.25f); // 确保不会被棋盘遮住
                monsterAnim.transform.localRotation = animGO.transform.localRotation;
                monster.isFacingRight = false;

                Destroy(piece.transform.Find("Root/SpritePiece").GetComponent<SpriteRenderer>()); // 如果有动画预设体，删除图片，暂时先这样写，直到所有棋子都有动画
            }
            else
            {
                piece.transform.Find("Root/SpritePiece").GetComponent<SpriteRenderer>().sprite = monster.data.monsterSprite;
            }

            InitialiseMonsterValues(monster, data);
            grid[data.row, data.col].gridStatus.Value = GridStatusEnum.MonsterPiece;
            this.GetSystem<ISpawnSystem>().GetMonsterList().Add(piece.GetComponent<Monster>());
            
            // 在棋子系统中登记 // todo 目前假设怪物都占据一个格子
            List<BoxGrid> crtGrids = new List<BoxGrid>();
            crtGrids.Add(grid[data.row, data.col]);
            this.GetSystem<IPieceSystem>().AddPieceEnemy(monster, crtGrids);

            // 发送棋子生成后触发的特性event
            SpecialitiesSpawnCheckEvent e = new SpecialitiesSpawnCheckEvent { piece = monster };
            this.SendEvent(e);
        }

        /// <summary>
        /// 收到怪物持续生成事件后处理
        /// </summary>
        /// <param name="data"></param>
        private IEnumerator OnConstantSpawnMonsterEvent(ConstantSpawnMonsterEvent data)
        {
            int x = data.spawnPoint.x;
            int y = data.spawnPoint.y;
            ISpawnSystem spawnSystem = this.GetSystem<ISpawnSystem>();
            var grid = this.GetSystem<IMapSystem>().Grids()[x, y];
            float startTime = Time.time;
            while (Time.time - startTime <= data.duration)
            {
                int rng = UnityEngine.Random.Range(1, 101);
                if (grid.IsEmpty() && rng < data.spawnProbability)
                {
                    var spawnMonsterEvent = new SpawnMonsterEvent
                    { col = y, row = x, name = data.name, pieceId = spawnSystem.GetPieceIdCounter() };
                    spawnSystem.IncrementPieceIdCounter();
                    OnSpawnMonsterEvent(spawnMonsterEvent);
                }
                yield return new WaitForSeconds(data.cooldown);
            }
        }

        /// <summary>
        /// 将对应怪物SO的数据读取到怪物实例中
        /// </summary>
        /// <param name="monster">怪物实例下的Monster script</param>
        private void InitialiseMonsterValues(Monster monster, SpawnMonsterEvent data)
        {
            SOMonsterBase somb = monster.data;
            monster.pieceId = data.pieceId;           
            monster.rarity = somb.rarity;
            monster.generalId = somb.monsterId;
            monster.moveSpeed = new BindableProperty<float>(somb.moveSpeed);
            monster.hp = new BindableProperty<int>(somb.maxHp);
            monster.maxHp = new BindableProperty<int>(somb.maxHp);
            monster.atkSpeed = new BindableProperty<float>(somb.atkSpeed);
            monster.atkDmg = new BindableProperty<float>(somb.atkDmg);
            monster.defense = new BindableProperty<float>(somb.defense);
            monster.accuracy = new BindableProperty<float>(somb.accuracy);
            monster.atkRange = new BindableProperty<int>(somb.atkRange);
            monster.features = new BindableProperty<List<FeatureEnum>>(somb.properties);
            monster.dirs = new BindableProperty<List<DirEnum>>(somb.dirs);
            monster.inCombat = false;
            monster.isAttacking = new BindableProperty<bool>(false);
            monster.isDying = new BindableProperty<bool>(false);

            // 也许可以删除，用viewpiecebase的pieceGrids
            (int, int) temp = (data.row, data.col);
            monster.leftTopGridPos = new BindableProperty<(int, int)>(temp);
            (int, int) temp2 = (data.row + somb.height - 1, data.col + somb.width - 1);
            monster.botRightGridPos = new BindableProperty<(int, int)>(temp2);
        }
    }
}