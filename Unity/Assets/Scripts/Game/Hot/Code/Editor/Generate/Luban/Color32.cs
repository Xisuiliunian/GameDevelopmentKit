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

public sealed partial class Color32 :  Bright.Config.EditorBeanBase 
{
    public Color32()
    {
    }

    public override void LoadJson(SimpleJSON.JSONObject _json)
    {
        { 
            var _fieldJson = _json["r"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  R = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["g"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  G = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["b"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  B = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["a"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  A = _fieldJson;
            }
        }
        
    }

    public override void SaveJson(SimpleJSON.JSONObject _json)
    {
        _json["$type"] = "Color32";
        {
            _json["r"] = new JSONNumber(R);
        }
        {
            _json["g"] = new JSONNumber(G);
        }
        {
            _json["b"] = new JSONNumber(B);
        }
        {
            _json["a"] = new JSONNumber(A);
        }
    }

    public static Color32 LoadJsonColor32(SimpleJSON.JSONNode _json)
    {
        Color32 obj = new Color32();
        obj.LoadJson((SimpleJSON.JSONObject)_json);
        return obj;
    }
        
    public static void SaveJsonColor32(Color32 _obj, SimpleJSON.JSONNode _json)
    {
        _obj.SaveJson((SimpleJSON.JSONObject)_json);
    }

    public byte R { get; set; }

    public byte G { get; set; }

    public byte B { get; set; }

    public byte A { get; set; }

}

}