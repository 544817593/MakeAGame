using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UprightRat", menuName = "ScriptableObjects/Pieces/Monsters/UprightRat")]
public class SOMonsterUprightRat : SOMonsterBase
{
    public GameObject anim;
    
    public override GameObject GetChildAnim()
    {
        return anim;
    }
}
