using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items")]
public class SOItemBase : ScriptableObject
{
    public ItemTypeEnum itemType;
    public ItemVolumeEnum volume;
    public string itemName;
    public Sprite sprite;
    public int buyCost;
    protected PlayerManager playerManager;

}
