using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOFeature", menuName = "ScriptableObjects/Feature")]
    public class SOFeature: ScriptableObject
    {
        public int featureID;
        public string featureName;  // 特性名字
        public string featureDesc;  // 特性描述
        public string typeName;     // 特性对应的类名
        public Sprite icon;         // 图标名
    }
}