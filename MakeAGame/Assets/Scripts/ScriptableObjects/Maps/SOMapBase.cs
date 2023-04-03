using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图物体
/// </summary>
[CreateAssetMenu(fileName = "CombatMap", menuName = "ScriptableObjects/Map")]
public class SOMapBase : ScriptableObject
{
    public List<List<EntityBoxGrid>> grid2DList;  // 2D地图，每个储存一个格子
    public int rows;    // 地图总行数
    public int cols;    // 地图总列数


}
