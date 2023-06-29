using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LengSpider", menuName = "ScriptableObjects/Pieces/Monsters/LengSpider")]
public class SOMonsterLengSpider : SOMonsterBase
{
    public GameObject anim;

    public override GameObject GetChildAnim()
    {
        return anim;
    }
}
