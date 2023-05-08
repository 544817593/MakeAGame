using UnityEngine;

namespace Game
{
    /// <summary>
    /// 一些全局静态方法
    /// </summary>
    public static class Extensions
    {
        public const string DefaultCharacterInfoPath = "ScriptableObjects/Characters/FrancisWaylandThurston";    // 默认角色信息资源
        
        /// <summary>
        /// 获取角色信息so
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="canRetNull">是否可以返回空，若否，找不到对应资源时返回默认资源</param>
        /// <returns></returns>
        public static SOCharacterInfo GetCharacterInfo(int id, bool canRetNull = true)
        {
            var so = Resources.Load<SOCharacterInfo>($"ScriptableObjects/Characters/SOCharacterInfo_{id}");
            if (so != null) 
                return so;
            else
            {
                if (canRetNull) 
                    return null;
                else
                {
                    Debug.LogError($"can't find CharacterInfo ID: {id}, return default");
                    var defaultSO = Resources.Load<SOCharacterInfo>(DefaultCharacterInfoPath);
                    return defaultSO;
                }
            }
        }

        /// <summary>
        /// 获取稀有度宝石图片
        /// </summary>
        /// <param name="rarity"></param>
        /// <returns></returns>
        public static Sprite GetRaritySprite(int rarity)
        {
            Sprite sp = Resources.Load<Sprite>($"Sprites/Card/Card_Rarity_{rarity}");
            return sp;
        }
    }
}