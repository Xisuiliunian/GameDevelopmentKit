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



namespace Game.Hot.Editor
{

public sealed partial class DROneConfig :  Bright.Config.EditorBeanBase 
{
    public DROneConfig()
    {
            TestVector3 = new Vector3();
    }

    public override void LoadJson(SimpleJSON.JSONObject _json)
    {
        { 
            var _fieldJson = _json["Test"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  Test = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["TestVector3"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsObject) { throw new SerializationException(); }  TestVector3 = Vector3.LoadJsonVector3(_fieldJson);
            }
        }
        
    }

    public override void SaveJson(SimpleJSON.JSONObject _json)
    {
        {
            _json["Test"] = new JSONNumber(Test);
        }
        {

            if (TestVector3 == null) { throw new System.ArgumentNullException(); }
            { var __bjson = new JSONObject();  Vector3.SaveJsonVector3(TestVector3, __bjson); _json["TestVector3"] = __bjson; }
        }
    }

    public static DROneConfig LoadJsonDROneConfig(SimpleJSON.JSONNode _json)
    {
        DROneConfig obj = new DROneConfig();
        obj.LoadJson((SimpleJSON.JSONObject)_json);
        return obj;
    }
        
    public static void SaveJsonDROneConfig(DROneConfig _obj, SimpleJSON.JSONNode _json)
    {
        _obj.SaveJson((SimpleJSON.JSONObject)_json);
    }

    /// <summary>
    /// 匹配最大时间
    /// </summary>
    public int Test { get; set; }

    public Vector3 TestVector3 { get; set; }

}

}