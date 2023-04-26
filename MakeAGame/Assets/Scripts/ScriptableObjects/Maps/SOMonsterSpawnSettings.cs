using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpawnMap", menuName ="ScriptableObjects/MonsterSpawnData")]
public class SOMonsterSpawnSettings : ScriptableObject
{
    public List<Vector2Int> spawnPoints;
    public List<int> spawnProbability;
    public List<int> spawnDuration;
    public List<int> spawnCooldown;
    public List<string> monsterName;

}
