using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ghast", menuName = "ScriptableObjects/Pieces/Monsters/Ghast")]
public class SOMonsterGhast : SOMonsterBase
{
    public GameObject anim;

    public override GameObject GetChildAnim()
    {
        return anim;
    }
}
