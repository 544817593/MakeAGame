using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StarVampire", menuName = "ScriptableObjects/Pieces/Monsters/StarVampire")]
public class SOMonsterStarVampire : SOMonsterBase
{
    public GameObject anim;

    public override GameObject GetChildAnim()
    {
        return anim;
    }
}
