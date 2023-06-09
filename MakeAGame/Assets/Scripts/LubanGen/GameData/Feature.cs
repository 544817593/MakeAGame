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

public sealed partial class Feature :  Bright.Config.BeanBase 
{
    public Feature(JSONNode _json) 
    {
        { if(!_json["featureID"].IsNumber) { throw new SerializationException(); }  FeatureID = _json["featureID"]; }
        { if(!_json["featureName"].IsString) { throw new SerializationException(); }  FeatureName = _json["featureName"]; }
        { if(!_json["desc"].IsString) { throw new SerializationException(); }  Desc = _json["desc"]; }
        { if(!_json["typeName"].IsString) { throw new SerializationException(); }  TypeName = _json["typeName"]; }
        PostInit();
    }

    public Feature(int featureID, string featureName, string desc, string typeName ) 
    {
        this.FeatureID = featureID;
        this.FeatureName = featureName;
        this.Desc = desc;
        this.TypeName = typeName;
        PostInit();
    }

    public static Feature DeserializeFeature(JSONNode _json)
    {
        return new GameData.Feature(_json);
    }

    /// <summary>
    /// 特性ID
    /// </summary>
    public int FeatureID { get; private set; }
    /// <summary>
    /// 特性名字
    /// </summary>
    public string FeatureName { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 方法类名（待定，可能通过反射调用）
    /// </summary>
    public string TypeName { get; private set; }

    public const int __ID__ = 152457668;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "FeatureID:" + FeatureID + ","
        + "FeatureName:" + FeatureName + ","
        + "Desc:" + Desc + ","
        + "TypeName:" + TypeName + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
