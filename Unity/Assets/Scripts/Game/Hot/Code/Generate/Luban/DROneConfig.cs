//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace Game.Hot
{
public sealed partial class DROneConfig :  Bright.Config.BeanBase 
{
    public DROneConfig(ByteBuf _buf) 
    {
        Test = _buf.ReadInt();
        TestVector3 = ExternalTypeUtility.NewFromVector3(Vector3.DeserializeVector3(_buf));
        PostInit();
    }

    public static DROneConfig DeserializeDROneConfig(ByteBuf _buf)
    {
        return new DROneConfig(_buf);
    }

    /// <summary>
    /// 匹配最大时间
    /// </summary>
    public int Test { get; private set; }
    public UnityEngine.Vector3 TestVector3 { get; private set; }

    public const int __ID__ = -2019618726;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, IDataTable> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Test:" + Test + ","
        + "TestVector3:" + TestVector3 + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}