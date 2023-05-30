using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cfg;
using cfg.GameData;
using SimpleJSON;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Game.LubanDataLoad
{
    /*
     * =====使用Luban转配置文件（excel->json->so）=====
     * 1.在LubanTool-GameConfig-Datas文件夹中编辑excel
     * 2.（mac）运行LubanTool文件夹中的gen_code_json.sh 对windows暂时不支持
     * 3.成功后，运行该脚本
     */
    
    /// <summary>
    /// excel文件配好后，先通过Luban生成json文件，再运行该脚本导入成ScriptableObject（会覆盖已有文件）
    /// </summary>
    public class LubanDataLoader: MonoBehaviour
    {
        // 存放SOFeature的文件夹的完整路径
        public string fullSOFolderFeature = "Assets/Resources/ScriptableObjects/LubanFeature";
        public bool forceOverrideFeature = true;   // 是否强行覆盖已有的so；若否，只生成新加的so
        public bool loadFeature;
        
        // 存放SOCharacterInfo的文件夹的完整路径
        public string fullSOFolderChara = "Assets/Resources/ScriptableObjects/LubanCharacter";
        public bool forceOverrideChara = true;   // 是否强行覆盖已有的so；若否，只生成新加的so
        public bool loadCharacter;
        
        // Feature图片资源路径
        public string FeatureIconResFolder = "Sprites/Cards/Feature";
        
        private void Start()
        {
            StartLoad();
        }

        void StartLoad()
        {
            var table = new Tables(Loader);
            var features = table.TbFeature.DataList;
            LoadFeature(features);
            // var characters = table.TbCharacter.DataList;
            // LoadCharacter(characters);
        }

        void LoadFeature(List<Feature> datas)
        {
            Debug.Log(string.Format("<color=green>{0}</color>", $"start load feature, data count: {datas.Count}"));
            string resFolderFeature = fullSOFolderFeature.Substring("Assets/".Length);
            int createCount = 0;
            
            foreach (var data in datas)
            {
                if (forceOverrideFeature)
                {
                    var tmpSO = Resources.Load<SOFeature>(
                        $@"{resFolderFeature}/feature_so_{data.FeatureName}_{data.FeatureID}");
                    if (tmpSO != null)
                    {
                        Debug.Log(string.Format("<color=yellow>{0}</color>",
                            $"SOFeature already existed, continue - feature_so_{data.FeatureName}_{data.FeatureID}"));
                        continue;
                    }
                }
                
                SOFeature so = ScriptableObject.CreateInstance<SOFeature>();
                ConvertFeatureJsonToSO(so, data);

                AssetDatabase.CreateAsset(so, $@"{fullSOFolderFeature}/feature_so_{data.FeatureName}_{data.FeatureID}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"create feature_so_{data.FeatureName}_{data.FeatureID}.asset");
                createCount++;
            }
            
            Debug.Log(string.Format("<color=green>{0}</color>", $"finish load feature, create count: {createCount}"));
        }

        void ConvertFeatureJsonToSO(SOFeature so, Feature json)
        {
            string iconFileName = String.Empty;
            string fullFolderPath = Application.dataPath + "/Resources/" + FeatureIconResFolder;
            var iconFiles = new DirectoryInfo(fullFolderPath).GetFiles();
            iconFileName = iconFiles.ToList().Find(info => info.Name.EndsWith($"_{json.FeatureID}.png"))?.Name;

            so.featureID = json.FeatureID;
            so.featureName = json.FeatureName;
            so.featureDesc = json.Desc;
            so.typeName = json.TypeName;

            if(string.IsNullOrEmpty(iconFileName))
            {
                Debug.LogError($"feature {json.FeatureName} icon sprite null");
                return;
            }
            so.icon = Resources.Load<Sprite>($@"{FeatureIconResFolder}/{iconFileName}");
        }

        void LoadCharacter(List<Character> datas)
        {
            Debug.Log(string.Format("<color=green>{0}</color>", $"start load character, data count: {datas.Count}"));
            string resFolderChara = fullSOFolderChara.Substring("Assets/".Length);
            int createCount = 0;
            
            foreach (var data in datas)
            {
                if (forceOverrideChara)
                {
                    var tmpSO = Resources.Load<SOFeature>(
                        $@"{resFolderChara}/character_so_{data.CharacterName}_{data.CharacterID}");
                    if (tmpSO != null)
                    {
                        Debug.Log(string.Format("<color=yellow>{0}</color>",
                            $"SOCharacterInfo already existed, continue - character_so_{data.CharacterName}_{data.CharacterID}"));
                        continue;
                    }
                }
                
                SOCharacterInfo so = ScriptableObject.CreateInstance<SOCharacterInfo>();
                // todo luban character
                
                

                AssetDatabase.CreateAsset(so, $@"{fullSOFolderChara}/character_so_{data.CharacterName}_{data.CharacterID}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"create character_so_{data.CharacterName}_{data.CharacterID}.asset");
                createCount++;
            }
            
            Debug.Log(string.Format("<color=green>{0}</color>", $"finish load character, create count: {createCount}"));
        }
        
        void ConvertCharacterJsonToSO(SOFeature so, Character json)
        {
            
            // todo Luban icon
            
            
        }

        // 载入json文件
        private JSONNode Loader(string fileName)
        {
            return JSON.Parse(File.ReadAllText(Application.dataPath + "/Resources/Json/" + fileName + ".json"));
        }
    }
}