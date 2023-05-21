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

        private Dictionary<PieceMoveDirection, Transform> dictBtnDirection =
            new Dictionary<PieceMoveDirection, Transform>();

        // private IPieceSystem pieceSystem;
        public PieceMoveDirection crtDirection = PieceMoveDirection.None;

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

            // pieceSystem = this.GetSystem<IPieceSystem>();
        }

        /// <summary>
        /// 只显示棋子可用的移动方向
        /// </summary>
        /// <param name="directions"></param>
        public void SetValidDirections(PieceMoveDirection[] directions)
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

        private void OnMouseHoverBtn(PieceMoveDirection dir)
        {
            Debug.Log($"mouse enter dir {dir}");
            crtDirection = dir;
            // this.SendCommand<ChangePieceDirectionCommand>(new ChangePieceDirectionCommand(dir));
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}