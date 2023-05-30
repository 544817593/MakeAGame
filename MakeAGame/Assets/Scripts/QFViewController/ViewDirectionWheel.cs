using System.Collections.Generic;
using QFramework;
using Sirenix.Utilities;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace Game
{
    public class ViewDirectionWheel: IController
    {
        public GameObject gameObject { get; }

        private Dictionary<DirEnum, Transform> dictBtnDirection = new Dictionary<DirEnum, Transform>();
        
        public DirEnum crtDirection = DirEnum.None;

        public ViewDirectionWheel(GameObject go)
        {
            gameObject = go;

            Transform root = gameObject.transform.Find("Root");
            for (int i = 0; i < root.childCount; i++)
            {
                var trans = root.GetChild(i);
                var indicator = trans.GetComponent<DirectionIndicator>();
                indicator.InitUIHelper(() => OnMouseHoverBtn(indicator.direction));
                var dir = indicator.direction;
                dictBtnDirection.Add(dir, trans);
            }
        }

        /// <summary>
        /// 只显示棋子可用的移动方向
        /// </summary>
        /// <param name="directions"></param>
        public void SetValidDirections(DirEnum[] directions)
        {
            var listDir = Enumerable.ToList(directions);
            foreach (var kvp in dictBtnDirection)
            {
                if (listDir.Contains(kvp.Key))
                {
                    kvp.Value.gameObject.SetActive(true);
                }
                else
                {
                    kvp.Value.gameObject.SetActive(false);
                }
            }
        }

        private void OnMouseHoverBtn(DirEnum dir)
        {
            crtDirection = dir;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}