using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QFramework;
using Unity.VisualScripting;
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
        /// 向左走的方向
        /// </summary>
        public static List<DirEnum> leftDirs = new List<DirEnum>()
            { DirEnum.Topleft, DirEnum.Left, DirEnum.Downleft};

        /// <summary>
        /// 向右走的方向
        /// </summary>
        public static List<DirEnum> rightDirs = new List<DirEnum>()
            { DirEnum.Topright, DirEnum.Right, DirEnum.Downright };

        public static Dictionary<BuffType, string> buffToName = new Dictionary<BuffType, string>() 
        {
            { BuffType.Confusion, "混乱"},
            { BuffType.Poison, "中毒"},
            { BuffType.Drowning, "溺水"},
            { BuffType.Terrian_Poison, "毒沼"},
            { BuffType.Terrian_Fire, "炽焰"},
            { BuffType.Terrian_Water, "水潭"},
        };

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
        public static Sprite GetRaritySprite(RarityEnum rarity)
        {
            Sprite sp = Resources.Load<Sprite>($"Sprites/Cards/Rarity/Card_Rarity_{(int)rarity+1}");
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
        public static Vector3 ScreenToUIPos(Vector3 pos)
        {
            // pos = Camera.main.WorldToScreenPoint(pos);
            // pos -= new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            // return pos;

            Debug.Log($"start pos: {pos}");
            
            if (uiSize == Vector2.zero)
            {
                var uiRootCanvas = UIKit.Root.Canvas;
                uiSize = uiRootCanvas.GetComponent<RectTransform>().sizeDelta;//得到画布的尺寸
            }
            
            pos -= new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            pos.x = pos.x * (uiSize.x / Screen.width);
            pos.y = pos.y * (uiSize.y / Screen.height);
            
            Debug.Log($"return pos: {pos}");
            return pos;
        }

        private static Vector2 uiSize = Vector2.zero;
        

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

        public static string GetFileWithTail(string resFolder, string tail, string format)
        {
            string iconFileName = String.Empty;
            string fullFolderPath = Application.dataPath + "/Resources/" + resFolder;
            var iconFiles = new DirectoryInfo(fullFolderPath).GetFiles();
            iconFileName = iconFiles.ToList().Find(info => info.Name.EndsWith($"{tail}.{format}"))?.Name;

            if (string.IsNullOrEmpty(iconFileName))
                return null;
            else
            {
                iconFileName = iconFileName.Split(".")[0];
            }
            return iconFileName;
        }

        private const string RelicIconResFolder = "Sprites/Relics";
        public static Sprite GetRelicSpriteByID(int id)
        {
            string spriteName = Extensions.GetFileWithTail(RelicIconResFolder, $"{id}", "png");
            if (string.IsNullOrEmpty(spriteName))
            {
                return null;
            }
            return Resources.Load<Sprite>($@"{RelicIconResFolder}/{spriteName}");
        }

        public static string GetDeathFuncTypeName(int charaID)
        {
            return IdToSO.FindCardSOByID(charaID).deathFuncName;
        }

        public static RelicBase GetRelicBySO(SORelic so)
        {
            switch (so.relicID)
            {
                case 1: return new Relic1(so);
                case 3: return new Relic3(so);
                case 4: return new Relic4(so);
                case 5: return new Relic5(so);
                case 6: return new Relic6(so);
                case 7: return new Relic7(so);
                case 8: return new Relic8(so);
                case 9: return new Relic9(so);
                case 10: return new Relic10(so);
                default: return null;
            }
        }

        /// <summary>
        /// 返还以x,y为基础的射程内所有坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="range"></param>
        /// <returns>List of coordinates that are in range of target</returns>
        public static List<(int, int)> GetCoordinatesInRange(int x, int y, int range)
        {
            List<(int, int)> gridsInRange = new List<(int, int)>();
            for (int xCord = x - range; xCord < x + range + 1; xCord++)
            {
                for (int yCord = y - range; yCord < y + range + 1; yCord++)
                {
                    if (MathF.Abs(yCord - y) + MathF.Abs(xCord - x) <= range)
                    {
                        gridsInRange.Add((xCord, yCord));
                    }                      
                }               
            }
            return gridsInRange;
        }

        /// <summary>
        /// 在初始格子一定范围内有没有一个为特定类型的的格子
        /// </summary>
        /// <param name="grid">初始格子</param>
        /// <param name="range">范围</param>
        /// <param name="gridStatus">目标格子种类</param>
        /// <returns></returns>
        public static bool HasGridTypeInRange(BoxGrid grid, int range, TerrainEnum gridStatus)
        {
            List<(int, int)> gridCoordinates = GetCoordinatesInRange(grid.row, grid.col, range);
            BoxGrid[,] map = GameEntry.Interface.GetSystem<IMapSystem>().Grids();
            foreach((int,int) coordinate in gridCoordinates)
            {
                // 格子出界直接跳过
                if (coordinate.Item1 < 0 || coordinate.Item2 < 0 || coordinate.Item1 > map.GetLength(0) || coordinate.Item2 > map.GetLength(1)) continue;
                if (map[coordinate.Item1, coordinate.Item2].terrain == (int)gridStatus) return true;
            }
            return false;
        }
    }
}