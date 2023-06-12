using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoonBeast", menuName = "ScriptableObjects/Pieces/Monsters/MoonBeast")]
public class SOMonsterMoonBeast : SOMonsterBase
{
    public GameObject anim;

    public override GameObject GetChildAnim()
    {
        return anim;
    }
}
