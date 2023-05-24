using System;
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
        [Obsolete("请使用IdToSO.FindCardSOByID()")]
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
            Sprite sp = Resources.Load<Sprite>($"Sprites/Cards/Rarity/Card_Rarity_{rarity}");
            return sp;
        }

        /// <summary>
        /// 时间流速enum转为实际乘倍数
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static float ToTimeMultiplierFloat(this TimeMultiplierEnum e)
        {
            switch (e)
            {
                case TimeMultiplierEnum.Normal:
                    return 1f;
                case TimeMultiplierEnum.Fast:
                    return 1.2f;
                case TimeMultiplierEnum.Superfast:
                    return 1.5f;
                case TimeMultiplierEnum.Slow:
                    return 0.8f;
                case TimeMultiplierEnum.Superslow:
                    return 0.5f;
                default:
                    return 1f;
            }
        }

        /// <summary>
        /// 世界坐标和以左下角为原点的ui坐标的转换
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Vector3 WorldToUIPos(Vector3 pos)
        {
            pos = Camera.main.WorldToScreenPoint(pos);
            pos -= new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            return pos;
        }

        public static Card GetCopy(this Card oldCard)
        {
            var newObj = oldCard.Clone();
            var newCard = newObj as Card;
            if (newCard == null)
            {
                Debug.LogError("card copy failed!");
                return null;
            }
            else
            {
                return newCard;
            }
        }
    }
}