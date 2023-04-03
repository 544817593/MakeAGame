using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图格子基础信息
/// </summary>
public class EntityBoxGrid : MonoBehaviour
{
    [SerializeField] private TimeMultiplier timeMultiplier; // 时间流逝速度
    public (int, int) coordinate; // 格子在地图中的坐标

}
