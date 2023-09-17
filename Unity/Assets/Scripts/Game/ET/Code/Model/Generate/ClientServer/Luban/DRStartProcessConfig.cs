//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace ET
{
public sealed partial class DRStartProcessConfig :  Bright.Config.BeanBase 
{
    public DRStartProcessConfig(ByteBuf _buf) 
    {
        StartConfig = _buf.ReadString();
        Id = _buf.ReadInt();
        MachineId = _buf.ReadInt();
        Port = _buf.ReadInt();
        PostInit();
    }

    public static DRStartProcessConfig DeserializeDRStartProcessConfig(ByteBuf _buf)
    {
        return new DRStartProcessConfig(_buf);
    }

    /// <summary>
    /// 开启类型
    /// </summary>
    public string StartConfig { get; private set; }
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 所属机器
    /// </summary>
    public int MachineId { get; private set; }
    /// <summary>
    /// 外网端口
    /// </summary>
    public int Port { get; private set; }

    public const int __ID__ = -417016195;
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
        + "StartConfig:" + StartConfig + ","
        + "Id:" + Id + ","
        + "MachineId:" + MachineId + ","
        + "Port:" + Port + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}