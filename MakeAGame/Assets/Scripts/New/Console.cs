using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public static class Console
    {
        private static string[] Commands = new string[]
        {
            "CrtScene", "GenerateMap", "Test"
        };
        
        public static string Input(string input)
        {
            string output = null;
            List<String> args = new List<string>(input.Split(" "));
            if (args.Count == 0) return output;
            switch (args[0])
            {
                case "CrtScene":
                    output = CrtScene();
                    break;
                
                // case more
                
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
