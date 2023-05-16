using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items")]
public class SOItemBase : ScriptableObject
{
    public ItemTypeEnum itemType; // 物品类型
    public ItemVolumeEnum volume; // 物品容量
    public string itemName; // 物品名字
    public Sprite sprite; // 物品图标
    public int buyCost; // 物品购买价格
    public int sellPrice; // 物品出售价格
    public string description; // 物品描述
    public int rarity; // 物品稀有度
    public int enhanceLevel; // 如果物品类型是强化物品，这个存储可强化的卡牌等级
    protected PlayerManager playerManager;

}
