using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// 内部控制台（静态类）
    /// </summary>
    public static class Console
    {
        // 所有命令
        private static string[] Commands = new string[]
        {
            "CrtScene", // 获取当前场景名称
            "GenerateMap",  // todo
            "Test",  // todo
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

                // todo 更多命令写在这里
                
                default:
                    output = "<color=\"red\">Unvalid command!</color>";
                    break;
            }

            return output;
        }

        private static string CrtScene()
        {
            Scene crtScene = SceneManager.GetActiveScene();
            return $"Current scene is: {crtScene.name}";
        }
    }
}
