using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DarkYoung", menuName = "ScriptableObjects/Pieces/Monsters/DarkYoung")]
public class SOMonsterDarkYoung : SOMonsterBase
{
    public SOMonsterDarkYoung()
    {
        moveSpeed = int.MaxValue;
    }

    public Sprite starMarker; // 抵触额外属性标记图
}
