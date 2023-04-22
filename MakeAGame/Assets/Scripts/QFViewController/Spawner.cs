using Game;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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


    void Start()
    {
        // 监听怪物生成事件
        this.RegisterEvent<SpawnMonsterEvent>(OnSpawnMonsterEvent);
    }


    /// <summary>
    /// 收到怪物生成事件后处理，还没写好！！！
    /// </summary>
    /// <param name="data"></param>
    private void OnSpawnMonsterEvent(SpawnMonsterEvent data)
    {
        var grid = this.GetSystem<MapSystem>().mGrids;
        Transform gridTransform = grid[data.row,data.col].transform;
        GameObject piece = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyPiece"));

        // 置于PiecePosition父物体下，该物体比地图略高一些
        piece.transform.SetParent(GameObject.Find("PiecePosition").transform);
        piece.transform.position = gridTransform.position;
        piece.transform.Rotate(90, 0, 0);
        piece.GetComponent<Monster>().data.Value = AssetDatabase.LoadAssetAtPath<SOMonsterBase>
            ("Assets/Resources/ScriptableObjects/" + data.name + ".asset");
        (int, int) temp = (data.row, data.col);
        piece.GetComponent<Monster>().leftTopGridPos = new BindableProperty<(int, int)>(temp);
        //enemyList.Add(piece.GetComponent<Monster>());


        //monster.InitWithData(MonsterPieceInfo.Instance.monsInfoList[id]);
    }
}
