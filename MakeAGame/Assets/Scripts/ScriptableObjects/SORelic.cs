using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SORelic", menuName = "ScriptableObjects/Relic")]
    /// <summary>
    /// 遗物信息
    /// </summary>
    public class SORelic: ScriptableObject
    {
        public int relicID; // id
        public string relicName;    // 名字
        public string desc; // 设定描述
        public string effectDesc;   // 效果描述
        public RarityEnum rarity;   // 品质 稀有度

        public List<int> toCancelRelics = new List<int>();  // 本遗物会取消哪些遗物的效果
        public List<int> beCanceledRelics = new List<int>();    // 本遗物会被哪些遗物取消效果
        public int effectPriority;  // 与其他遗物同时作用于同一数值时，两者的先后顺序

        public List<float> effectParams;    // 效果参数，每个遗物的每个参数意义不同，见表备注
    }
}