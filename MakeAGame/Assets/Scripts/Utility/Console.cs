using System;
using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// 内部控制台（静态类）
    /// </summary>
    public static class Console
    {
        public const string ErrorOutput = "<color=\"red\">Unvalid command!</color>";
        
        // 所有命令
        private static string[] Commands = new string[]
        {
            "CrtScene", // 获取当前场景名称
            "GenMap",   // 生成一张全是普通格子的地图 arg1:行数 arg2:列数    // ex: GenMap 4 6
            "ChangeScene" // 转到战斗场景
        };
        
        /// <summary>
        /// 处理控制台输入
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <returns></returns>
        public static string Input(string input)
        {
            string output = null;
            List<String> args = new List<string>(input.Split(" "));
            if (args.Count == 0) return output;
            switch (args[0])
            {
                case "CrtScene":
                    output = CrtScene();    // 每个命令的处理对应一个方法
                    break;
                case "ChangeScene":
                    SceneManager.LoadScene(args[1]);
                    return output;
                case "GenMap":
                    output = GenerateMap(args.GetRange(1,args.Count - 1));
                    break;

                // todo 更多命令写在这里
                
                default:
                    output = ErrorOutput;
                    break;
            }

            return output;
        }

        private static string CrtScene()
        {
            Scene crtScene = SceneManager.GetActiveScene();
            return $"Current scene is: {crtScene.name}";
        }

        private static string GenerateMap(List<string> args)
        {
            if(args.Count < 2)
                return ErrorOutput;
            
            var mapSystem = GameEntry.Interface.GetSystem<IMapSystem>();
            mapSystem.Clear();

            SOMapData mapData = ScriptableObject.CreateInstance<SOMapData>();
            mapData.row = args[0].ToInt();
            mapData.col = args[1].ToInt();
            mapData.BuildMap();
            
            mapSystem.CreateMapBySO(mapData);
            
            // 镜头位置
            ChangeCameraTargetEvent setCameraCenterEvent = new ChangeCameraTargetEvent()
            {
                target = mapSystem.centerGrid.transform
            };
            GameEntry.Interface.SendEvent<ChangeCameraTargetEvent>(setCameraCenterEvent);

            return "map is recreated";
        }
    }
}
