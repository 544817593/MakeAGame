using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "HoundsOfTindalos", menuName = "ScriptableObjects/Pieces/Monsters/HoundsOfTindalos")]
public class SOMonsterHoundsOfTindalos : SOMonsterBase
{
    public GameObject anim;

    public override GameObject GetChildAnim()
    {
        return anim;
    }
}
