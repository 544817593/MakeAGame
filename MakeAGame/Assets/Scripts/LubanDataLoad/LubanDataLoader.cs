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
        // 卡面图片资源路径
        public string CardIconResFolder = "Sprites/Cards/Character";
        // 棋子图片资源路径
        public string PieceIconResFolder = "Sprites/Piece";
        
        private void Start()
        {
            StartLoad();
        }

        void StartLoad()
        {
            var table = new Tables(Loader);
            Debug.Log($"start load so, loadFeature: {loadFeature}, loadCharacter: {loadCharacter}");

            if (loadFeature)
            {
                var features = table.TbFeature.DataList;
                LoadFeature(features);   
            }
            
            AssetDatabase.Refresh();

            if (loadCharacter)
            {
                var characters = table.TbCharacter.DataList;
                LoadCharacter(characters);   
            }
            
            AssetDatabase.Refresh();
        }

        void LoadFeature(List<Feature> datas)
        {
            Debug.Log(string.Format("<color=green>{0}</color>", $"start load feature, data count: {datas.Count}"));
            string resFolderFeature = fullSOFolderFeature.Substring("Assets/Resources/".Length);
            int createCount = 0;
            
            foreach (var data in datas)
            {
                if (!forceOverrideFeature)
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
            string iconFileName = Extensions.GetFileWithTail(FeatureIconResFolder, $"_{json.FeatureID}", "png");

            so.featureID = json.FeatureID;
            so.featureName = json.FeatureName;
            so.featureDesc = json.Desc;
            so.typeName = json.TypeName;

            if(string.IsNullOrEmpty(iconFileName))
            {
                Debug.LogError($"feature {json.FeatureName} icon sprite null");
                return;
            }

            string tmpPath = $@"{FeatureIconResFolder}/{iconFileName}";
            var tmpSpr = Resources.Load<Sprite>(tmpPath);
            so.icon = tmpSpr;
        }

        void LoadCharacter(List<Character> datas)
        {
            Debug.Log(string.Format("<color=green>{0}</color>", $"start load character, data count: {datas.Count}"));
            string resFolderChara = fullSOFolderChara.Substring("Assets/".Length);
            int createCount = 0;
            
            foreach (var data in datas)
            {
                if (!forceOverrideChara)
                {
                    var tmpSO = Resources.Load<SOCharacterInfo>(
                        $@"{resFolderChara}/character_so_{data.CharacterName}_{data.CharacterID}");
                    if (tmpSO != null)
                    {
                        Debug.Log(string.Format("<color=yellow>{0}</color>",
                            $"SOCharacterInfo already existed, continue - character_so_{data.CharacterName}_{data.CharacterID}"));
                        continue;
                    }
                }
                
                SOCharacterInfo so = ScriptableObject.CreateInstance<SOCharacterInfo>();
                ConvertCharacterJsonToSO(so, data);

                AssetDatabase.CreateAsset(so, $@"{fullSOFolderChara}/character_so_{data.CharacterName}_{data.CharacterID}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"create character_so_{data.CharacterName}_{data.CharacterID}.asset");
                createCount++;
            }
            
            Debug.Log(string.Format("<color=green>{0}</color>", $"finish load character, create count: {createCount}"));
        }
        
        void ConvertCharacterJsonToSO(SOCharacterInfo so, Character json)
        {
            so.characterID = json.CharacterID;
            so.characterName = json.CharacterName;

            so.rarity = (RarityEnum)json.Rarity;
            so.sanCost = json.SanCost;
            so.deathFuncDescription = json.DeathDesc;
            so.cardSprite = LoadCardSprite(json);
            so.hp = json.Hp;
            so.attack = json.Atk;
            so.moveSpd = json.MoveSpd;
            so.defend = json.Def;
            
            LoadFeatureAndSpecialFeature(so, json);
            
            so.pieceSprite = LoadPieceSprite(json);
            so.width = json.Width;
            so.height = json.Height;
            so.moveDirections = new List<DirEnum>();
            foreach (var dir in json.MoveDirections)
            {
                so.moveDirections.Add((DirEnum) dir);
            }
            so.attackSpd = json.AtkSpd;
            so.accracy = json.Accuracy;
            so.attackRange = json.AtkRange;
            so.life = json.Life;

            if (json.SanCostBonus.Property != PlayerStats.None)
                so.sanCostBonus = new SOCharacterInfo.PlayerBonus()
                    {stat = (PlayerStatsEnum) json.SanCostBonus.Property, multiple = json.SanCostBonus.Multiple};
            if (json.HpBonus.Property != PlayerStats.None)
                so.hpBonus = new SOCharacterInfo.PlayerBonus()
                    {stat = (PlayerStatsEnum) json.HpBonus.Property, multiple = json.HpBonus.Multiple};
            if (json.AtkBonus.Property != PlayerStats.None)
                so.atkBonus = new SOCharacterInfo.PlayerBonus()
                    {stat = (PlayerStatsEnum) json.AtkBonus.Property, multiple = json.AtkBonus.Multiple};
            if (json.AtkSpdBonus.Property != PlayerStats.None)
                so.atkSpdBonus = new SOCharacterInfo.PlayerBonus()
                    {stat = (PlayerStatsEnum) json.AtkSpdBonus.Property, multiple = json.AtkSpdBonus.Multiple};
        }

        Sprite LoadCardSprite(Character json)
        {
            string cardSpriteName = Extensions.GetFileWithTail(CardIconResFolder, $"_{json.CharacterID}", "png");
            if (string.IsNullOrEmpty(cardSpriteName))
            {
                Debug.LogError($"character {json.CharacterName} card sprite null");
                return null;
            }
            return Resources.Load<Sprite>($@"{CardIconResFolder}/{cardSpriteName}");
        }

        void LoadFeatureAndSpecialFeature(SOCharacterInfo so, Character json)
        {
            string resFolderFeature = fullSOFolderFeature.Substring("Assets/Resources/".Length);

            so.features = new List<SOFeature>();
            foreach (var fe in json.Feature_Ref)
            {
                var tmpPath = $"{resFolderFeature}/feature_so_{fe.FeatureName}_{fe.FeatureID}";
                var sof = Resources.Load<SOFeature>(tmpPath);
                if (sof == null)
                {
                    Debug.LogError($"character {json.CharacterName} feature {tmpPath} null");
                    continue;
                }
                so.features.Add(sof);
            }
            
            so.specialFeatures = new List<SOFeature>();
            foreach (var fe in json.SpecialFeature_Ref)
            {
                var tmpPath = $"{resFolderFeature}/feature_so_{fe.FeatureName}_{fe.FeatureID}";
                var sof = Resources.Load<SOFeature>(tmpPath);
                if (sof == null)
                {
                    Debug.LogError($"character {json.CharacterName} feature {tmpPath} null");
                    continue;
                }
                so.specialFeatures.Add(sof);
            }
        }
        
        Sprite LoadPieceSprite(Character json)
        {
            string pieceSpriteName = Extensions.GetFileWithTail(PieceIconResFolder, $"_{json.CharacterID}", "png");
            if (string.IsNullOrEmpty(pieceSpriteName))
            {
                Debug.LogError($"character {json.CharacterName} piece sprite null");
                return null;
            }
            return Resources.Load<Sprite>($@"{PieceIconResFolder}/{pieceSpriteName}");
        }


        // 载入json文件
        private JSONNode Loader(string fileName)
        {
            return JSON.Parse(File.ReadAllText(Application.dataPath + "/Resources/Json/" + fileName + ".json"));
        }
    }
}