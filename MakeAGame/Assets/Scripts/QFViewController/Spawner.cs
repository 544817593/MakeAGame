using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 用来实际生成关卡内的各种棋子
    /// </summary>
    public class Spawner : MonoBehaviour, IController
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
        /// <exception cref="NotImplementedException"></exception>
        private void OnSpawnCardEvent()
        {
            GameObject cardItem = (GameObject)Instantiate(Resources.Load("Prefabs/CardItem"));
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
            piece.transform.Rotate(90, 0, 0);
            Monster monster = piece.GetComponent<Monster>();
            monster.data = Resources.Load<SOMonsterBase>("ScriptableObjects/Monsters/" + data.name);

            //动画部分
            GameObject animGO = monster.data.GetChildAnim();
            GameObject mosterAnim = Instantiate(animGO);
            piece.GetComponent<MonsterMovement>().animator = mosterAnim.GetComponent<Animator>();
            mosterAnim.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            mosterAnim.transform.localPosition = new Vector3(0, 0, 0);
            mosterAnim.transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, 0));
            mosterAnim.transform.SetParent(piece.transform);
            
            // monster.data = AssetDatabase.LoadAssetAtPath<SOMonsterBase>
                // ("Assets/Resources/ScriptableObjects/Monsters/" + data.name + ".asset");
            piece.transform.Find("image").GetComponent<SpriteRenderer>().sprite = monster.data.monsterSprite;
            InitialiseMonsterValues(monster, data);
            grid[data.row, data.col].gridStatus.Value = GridStatusEnum.MonsterPiece;
            this.GetSystem<ISpawnSystem>().GetMonsterList().Add(piece.GetComponent<Monster>());
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
            monster.monsterId = somb.monsterId;
            monster.moveSpeed = new BindableProperty<float>(somb.moveSpeed);
            monster.hp = new BindableProperty<float>(somb.maxHp);
            monster.maxHp = new BindableProperty<float>(somb.maxHp);
            monster.atkSpeed = new BindableProperty<float>(somb.atkSpeed);
            monster.atkDmg = new BindableProperty<float>(somb.atkDmg);
            monster.defense = new BindableProperty<float>(somb.defense);
            monster.accuracy = new BindableProperty<float>(somb.accuracy);
            monster.atkRange = new BindableProperty<int>(somb.atkRange);
            monster.properties = new BindableProperty<List<PropertyEnum>>(somb.properties);
            monster.dirs = new BindableProperty<List<DirEnum>>(somb.dirs);
            monster.inCombat = new BindableProperty<bool>(false);
            monster.isAttacking = new BindableProperty<bool>(false);
            monster.isDying = new BindableProperty<bool>(false);
            (int, int) temp = (data.row, data.col);
            monster.leftTopGridPos = new BindableProperty<(int, int)>(temp);
            (int, int) temp2 = (data.row + somb.pieceSize.Item1 - 1, data.col + somb.pieceSize.Item2 - 1);
            monster.botRightGridPos = new BindableProperty<(int, int)>(temp2);
        }
    }
}