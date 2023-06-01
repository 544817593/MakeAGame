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
            })
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            // 监听创建卡牌事件
            this.RegisterEvent<SpawnCardEvent>(data =>
            {
                OnSpawnCardEvent();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

        }

        /// <summary>
        /// 收到卡牌生成事件后处理
        /// </summary>
        private void OnSpawnCardEvent()
        {
            GameObject cardItem = (GameObject)Instantiate(Resources.Load("Prefabs/CardItem"));
            ViewBagCard viewCard = cardItem.AddComponent<ViewBagCard>();
            viewCard.card = new Card(1);
            this.GetSystem<ISpawnSystem>().SetLastSpawnedCard(cardItem);
        }

        /// <summary>
        /// 收到怪物生成事件后处理
        /// </summary>
        /// <param name="data"></param>
        private void OnSpawnMonsterEvent(SpawnMonsterEvent data)
        {
            var grid = this.GetSystem<IMapSystem>().Grids();
            Transform gridTransform = grid[data.row, data.col].transform;
            GameObject piece = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyPiece"));
            // 置于MonsterPosition父物体下，该物体比地图略高一些
            piece.transform.SetParent(GameObject.Find("MonsterPosition").transform);
            piece.transform.position = gridTransform.position;
            // piece.transform.Rotate(90, 0, 0);
            Monster monster = piece.GetComponent<Monster>();
            monster.data = Resources.Load<SOMonsterBase>("ScriptableObjects/Monsters/" + data.name);
            // monster.data = AssetDatabase.LoadAssetAtPath<SOMonsterBase>
            // ("Assets/Resources/ScriptableObjects/Monsters/" + data.name + ".asset");

            piece.transform.Find("Root/SpritePiece").GetComponent<SpriteRenderer>().sprite = monster.data.monsterSprite;
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