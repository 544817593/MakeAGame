using System;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 探索房初始化信息
    /// </summary>
    public class ExploreRoomInfo: MonoBehaviour, IController
    {
        public SOExploreMapData soExploreMap;
        private IExploreSystem exploreSys;
        private UIExplore ui;

        DirEnum[] dirs = new DirEnum[] {DirEnum.Top, DirEnum.Right, DirEnum.Down, DirEnum.Left};

        private void Start()
        {
            Debug.Log($"ExploreRoomInfo Start, so: {soExploreMap.name}");

            exploreSys = this.GetSystem<IExploreSystem>();
            exploreSys.CreateMapBySO(soExploreMap);

            ui = UIKit.OpenPanel<UIExplore>();
            var btns = ui.GetComponentsInChildren<Button>();
            btns[0].onClick.AddListener(() => OnMoveClick(dirs[0]));
            btns[1].onClick.AddListener(() => OnMoveClick(dirs[1]));
            btns[2].onClick.AddListener(() => OnMoveClick(dirs[2]));
            btns[3].onClick.AddListener(() => OnMoveClick(dirs[3]));
            // for (int i = 0; i < btns.Length; i++)    // 不知道为啥不太对劲
            // {
            //     int j = i;
            //     btns[i].onClick.AddListener(() => OnMoveClick(dirs[j]));
            // }
            
            var tmp = ui.transform.Find("Root/TmpLeftMove").GetComponent<TextMeshProUGUI>();
            tmp.text = exploreSys.leftMove.ToString();
        }

        private void OnMoveClick(DirEnum dir)
        {
            Debug.Log($"OnMoveClick {dir}");
            bool isMoveSuccess = exploreSys.MovePlayer(dir);
            if (isMoveSuccess)
            {
                var tmp = ui.transform.Find("Root/TmpLeftMove").GetComponent<TextMeshProUGUI>();
                tmp.text = exploreSys.leftMove.ToString();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}