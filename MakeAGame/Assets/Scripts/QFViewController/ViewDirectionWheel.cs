using System.Collections.Generic;
using QFramework;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Enumerable = System.Linq.Enumerable;

namespace Game
{
    public class ViewDirectionWheel: IController
    {
        public static Dictionary<DirEnum, string> NotDirectionDict = new Dictionary<DirEnum, string>() 
        {
            {DirEnum.Top, "Sprites/Direction/NotDirectionUp"},
            {DirEnum.Down, "Sprites/Direction/NotDirectionDown"},
            {DirEnum.Left, "Sprites/Direction/NotDirectionLeft"},
            {DirEnum.Right, "Sprites/Direction/NotDirectionRight"},
        };
        public static Dictionary<DirEnum, string> MoveDirectionDict = new Dictionary<DirEnum, string>()
        {
            {DirEnum.Top, "Sprites/Direction/MoveDirectionUp"},
            {DirEnum.Down, "Sprites/Direction/MoveDirectionDown"},
            {DirEnum.Left, "Sprites/Direction/MoveDirectionLeft"},
            {DirEnum.Right, "Sprites/Direction/MoveDirectionRight"},
        };
        public static Dictionary<DirEnum, string> ClickDirectionDict = new Dictionary<DirEnum, string>()
        {
            {DirEnum.Top, "Sprites/Direction/ClickDirectionUp"},
            {DirEnum.Down, "Sprites/Direction/ClickDirectionDown"},
            {DirEnum.Left, "Sprites/Direction/ClickDirectionLeft"},
            {DirEnum.Right, "Sprites/Direction/ClickDirectionRight"},
        };
        public static Dictionary<DirEnum, string> CurDirectionDict = new Dictionary<DirEnum, string>()
        {
            {DirEnum.Top, "Sprites/Direction/Top"},
            {DirEnum.Down, "Sprites/Direction/Down"},
            {DirEnum.Left, "Sprites/Direction/Left"},
            {DirEnum.Right, "Sprites/Direction/Right"},
        };


        public GameObject gameObject { get; }

        private Dictionary<DirEnum, Transform> dictBtnDirection = new Dictionary<DirEnum, Transform>();
        
        public DirEnum crtDirection = DirEnum.None;

        public Dictionary<GameObject, Sprite> directionSpriteDict = new Dictionary<GameObject, Sprite>();

        public ViewDirectionWheel(GameObject go)
        {
            gameObject = go;

            Transform root = gameObject.transform.Find("Root");
            for (int i = 0; i < root.childCount; i++)
            {
                var trans = root.GetChild(i);
                var indicator = trans.GetComponent<DirectionIndicator>();
                indicator.InitUIHelper(() => OnMouseHoverBtn(indicator.direction));
                indicator.UIPointerExit(() => onMouseExitBtn(trans.gameObject));
                var dir = indicator.direction;
                dictBtnDirection.Add(dir, trans);
            }
        }

        /// <summary>
        /// 只显示棋子可用的移动方向
        /// </summary>
        /// <param name="directions"></param>
        public void SetValidDirections(List<DirEnum> directions)
        {
            var listDir = Enumerable.ToList(directions);
            foreach (var kvp in dictBtnDirection)
            {
                kvp.Value.gameObject.SetActive(true);
                // 挂对应图片资源
                if (listDir.Contains(kvp.Key))
                {
                    kvp.Value.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(MoveDirectionDict[kvp.Key]);
                    directionSpriteDict[kvp.Value.gameObject] = Resources.Load<Sprite>(MoveDirectionDict[kvp.Key]);
                }
                else
                {
                    kvp.Value.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(NotDirectionDict[kvp.Key]);
                    directionSpriteDict[kvp.Value.gameObject] = Resources.Load<Sprite>(NotDirectionDict[kvp.Key]);
                }
            }
        }

        private void OnMouseHoverBtn(DirEnum dir)
        {
            crtDirection = dir;
            foreach (var kvp in dictBtnDirection)
            {
                if (dir == kvp.Key)
                {
                    kvp.Value.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ClickDirectionDict[kvp.Key]);
                }
            }
        }
        private void onMouseExitBtn(GameObject directionObj)
        {
            crtDirection = DirEnum.None;
            directionObj.GetComponent<Image>().sprite = directionSpriteDict[directionObj];
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}