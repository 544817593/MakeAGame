using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SOFeature", menuName = "ScriptableObjects/Feature")]
    public class SOFeature: ScriptableObject
    {
        public int featureID;
        public string featureName;
        public string featureDesc;
    }
}