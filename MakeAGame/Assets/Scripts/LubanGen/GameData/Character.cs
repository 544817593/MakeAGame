//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg.GameData
{ 

public sealed partial class Character :  Bright.Config.BeanBase 
{
    public Character(JSONNode _json) 
    {
        { if(!_json["characterID"].IsNumber) { throw new SerializationException(); }  CharacterID = _json["characterID"]; }
        { if(!_json["characterName"].IsString) { throw new SerializationException(); }  CharacterName = _json["characterName"]; }
        { if(!_json["rarity"].IsNumber) { throw new SerializationException(); }  Rarity = (GameData.Rarity)_json["rarity"].AsInt; }
        { if(!_json["cardPack"].IsNumber) { throw new SerializationException(); }  CardPack = (GameData.CardPack)_json["cardPack"].AsInt; }
        { if(!_json["sanCost"].IsNumber) { throw new SerializationException(); }  SanCost = _json["sanCost"]; }
        { if(!_json["sanCostBonus"].IsObject) { throw new SerializationException(); }  SanCostBonus = GameData.PlayerBonus.DeserializePlayerBonus(_json["sanCostBonus"]);  }
        { if(!_json["deathDesc"].IsString) { throw new SerializationException(); }  DeathDesc = _json["deathDesc"]; }
        { if(!_json["hp"].IsNumber) { throw new SerializationException(); }  Hp = _json["hp"]; }
        { if(!_json["hpBonus"].IsObject) { throw new SerializationException(); }  HpBonus = GameData.PlayerBonus.DeserializePlayerBonus(_json["hpBonus"]);  }
        { if(!_json["atk"].IsNumber) { throw new SerializationException(); }  Atk = _json["atk"]; }
        { if(!_json["atkBonus"].IsObject) { throw new SerializationException(); }  AtkBonus = GameData.PlayerBonus.DeserializePlayerBonus(_json["atkBonus"]);  }
        { if(!_json["moveSpd"].IsNumber) { throw new SerializationException(); }  MoveSpd = _json["moveSpd"]; }
        { if(!_json["def"].IsNumber) { throw new SerializationException(); }  Def = _json["def"]; }
        { var __json0 = _json["feature"]; if(!__json0.IsArray) { throw new SerializationException(); } Feature = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  Feature.Add(__v0); }   }
        { var __json0 = _json["specialFeature"]; if(!__json0.IsArray) { throw new SerializationException(); } SpecialFeature = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  SpecialFeature.Add(__v0); }   }
        { if(!_json["width"].IsNumber) { throw new SerializationException(); }  Width = _json["width"]; }
        { if(!_json["height"].IsNumber) { throw new SerializationException(); }  Height = _json["height"]; }
        { var __json0 = _json["moveDirections"]; if(!__json0.IsArray) { throw new SerializationException(); } MoveDirections = new System.Collections.Generic.List<GameData.MoveDir>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { GameData.MoveDir __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = (GameData.MoveDir)__e0.AsInt; }  MoveDirections.Add(__v0); }   }
        { if(!_json["atkSpd"].IsNumber) { throw new SerializationException(); }  AtkSpd = _json["atkSpd"]; }
        { if(!_json["atkSpdBonus"].IsObject) { throw new SerializationException(); }  AtkSpdBonus = GameData.PlayerBonus.DeserializePlayerBonus(_json["atkSpdBonus"]);  }
        { if(!_json["accuracy"].IsNumber) { throw new SerializationException(); }  Accuracy = _json["accuracy"]; }
        { if(!_json["atkRange"].IsNumber) { throw new SerializationException(); }  AtkRange = _json["atkRange"]; }
        { if(!_json["life"].IsNumber) { throw new SerializationException(); }  Life = _json["life"]; }
        { if(!_json["DeathFuncName"].IsString) { throw new SerializationException(); }  DeathFuncName = _json["DeathFuncName"]; }
        PostInit();
    }

    public Character(int characterID, string characterName, GameData.Rarity rarity, GameData.CardPack cardPack, int sanCost, GameData.PlayerBonus sanCostBonus, string deathDesc, float hp, GameData.PlayerBonus hpBonus, float atk, GameData.PlayerBonus atkBonus, float moveSpd, float def, System.Collections.Generic.List<int> feature, System.Collections.Generic.List<int> specialFeature, int width, int height, System.Collections.Generic.List<GameData.MoveDir> moveDirections, float atkSpd, GameData.PlayerBonus atkSpdBonus, float accuracy, int atkRange, int life, string DeathFuncName ) 
    {
        this.CharacterID = characterID;
        this.CharacterName = characterName;
        this.Rarity = rarity;
        this.CardPack = cardPack;
        this.SanCost = sanCost;
        this.SanCostBonus = sanCostBonus;
        this.DeathDesc = deathDesc;
        this.Hp = hp;
        this.HpBonus = hpBonus;
        this.Atk = atk;
        this.AtkBonus = atkBonus;
        this.MoveSpd = moveSpd;
        this.Def = def;
        this.Feature = feature;
        this.SpecialFeature = specialFeature;
        this.Width = width;
        this.Height = height;
        this.MoveDirections = moveDirections;
        this.AtkSpd = atkSpd;
        this.AtkSpdBonus = atkSpdBonus;
        this.Accuracy = accuracy;
        this.AtkRange = atkRange;
        this.Life = life;
        this.DeathFuncName = DeathFuncName;
        PostInit();
    }

    public static Character DeserializeCharacter(JSONNode _json)
    {
        return new GameData.Character(_json);
    }

    /// <summary>
    /// 角色id
    /// </summary>
    public int CharacterID { get; private set; }
    /// <summary>
    /// 角色名字
    /// </summary>
    public string CharacterName { get; private set; }
    /// <summary>
    /// 稀有度
    /// </summary>
    public GameData.Rarity Rarity { get; private set; }
    /// <summary>
    /// 卡包
    /// </summary>
    public GameData.CardPack CardPack { get; private set; }
    /// <summary>
    /// 精神消耗
    /// </summary>
    public int SanCost { get; private set; }
    /// <summary>
    /// 精神消耗加成
    /// </summary>
    public GameData.PlayerBonus SanCostBonus { get; private set; }
    /// <summary>
    /// 死面功能描述
    /// </summary>
    public string DeathDesc { get; private set; }
    /// <summary>
    /// 血量
    /// </summary>
    public float Hp { get; private set; }
    /// <summary>
    /// 血量加成
    /// </summary>
    public GameData.PlayerBonus HpBonus { get; private set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public float Atk { get; private set; }
    /// <summary>
    /// 攻击力加成
    /// </summary>
    public GameData.PlayerBonus AtkBonus { get; private set; }
    /// <summary>
    /// 移动速度（秒/每行动）
    /// </summary>
    public float MoveSpd { get; private set; }
    /// <summary>
    /// 防御力
    /// </summary>
    public float Def { get; private set; }
    /// <summary>
    /// 特性
    /// </summary>
    public System.Collections.Generic.List<int> Feature { get; private set; }
    public System.Collections.Generic.List<GameData.Feature> Feature_Ref { get; private set; }
    /// <summary>
    /// 额外属性
    /// </summary>
    public System.Collections.Generic.List<int> SpecialFeature { get; private set; }
    public System.Collections.Generic.List<GameData.Feature> SpecialFeature_Ref { get; private set; }
    /// <summary>
    /// 棋子宽
    /// </summary>
    public int Width { get; private set; }
    /// <summary>
    /// 棋子高
    /// </summary>
    public int Height { get; private set; }
    /// <summary>
    /// 可移动方向
    /// </summary>
    public System.Collections.Generic.List<GameData.MoveDir> MoveDirections { get; private set; }
    /// <summary>
    /// 攻速（秒/每次）
    /// </summary>
    public float AtkSpd { get; private set; }
    /// <summary>
    /// 攻速加成
    /// </summary>
    public GameData.PlayerBonus AtkSpdBonus { get; private set; }
    /// <summary>
    /// 命中率
    /// </summary>
    public float Accuracy { get; private set; }
    /// <summary>
    /// 攻击范围（几格之内）
    /// </summary>
    public int AtkRange { get; private set; }
    /// <summary>
    /// 寿命（秒）
    /// </summary>
    public int Life { get; private set; }
    /// <summary>
    /// 死面方法的类名
    /// </summary>
    public string DeathFuncName { get; private set; }

    public const int __ID__ = -1827334537;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        SanCostBonus?.Resolve(_tables);
        HpBonus?.Resolve(_tables);
        AtkBonus?.Resolve(_tables);
        { GameData.TbFeature __table = (GameData.TbFeature)_tables["GameData.TbFeature"]; this.Feature_Ref = new System.Collections.Generic.List<GameData.Feature>(); foreach(var __e in Feature) { this.Feature_Ref.Add(__table.GetOrDefault(__e)); } }
        { GameData.TbFeature __table = (GameData.TbFeature)_tables["GameData.TbFeature"]; this.SpecialFeature_Ref = new System.Collections.Generic.List<GameData.Feature>(); foreach(var __e in SpecialFeature) { this.SpecialFeature_Ref.Add(__table.GetOrDefault(__e)); } }
        AtkSpdBonus?.Resolve(_tables);
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        SanCostBonus?.TranslateText(translator);
        HpBonus?.TranslateText(translator);
        AtkBonus?.TranslateText(translator);
        AtkSpdBonus?.TranslateText(translator);
    }

    public override string ToString()
    {
        return "{ "
        + "CharacterID:" + CharacterID + ","
        + "CharacterName:" + CharacterName + ","
        + "Rarity:" + Rarity + ","
        + "CardPack:" + CardPack + ","
        + "SanCost:" + SanCost + ","
        + "SanCostBonus:" + SanCostBonus + ","
        + "DeathDesc:" + DeathDesc + ","
        + "Hp:" + Hp + ","
        + "HpBonus:" + HpBonus + ","
        + "Atk:" + Atk + ","
        + "AtkBonus:" + AtkBonus + ","
        + "MoveSpd:" + MoveSpd + ","
        + "Def:" + Def + ","
        + "Feature:" + Bright.Common.StringUtil.CollectionToString(Feature) + ","
        + "SpecialFeature:" + Bright.Common.StringUtil.CollectionToString(SpecialFeature) + ","
        + "Width:" + Width + ","
        + "Height:" + Height + ","
        + "MoveDirections:" + Bright.Common.StringUtil.CollectionToString(MoveDirections) + ","
        + "AtkSpd:" + AtkSpd + ","
        + "AtkSpdBonus:" + AtkSpdBonus + ","
        + "Accuracy:" + Accuracy + ","
        + "AtkRange:" + AtkRange + ","
        + "Life:" + Life + ","
        + "DeathFuncName:" + DeathFuncName + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
