//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ET
{

public partial class Tables
{
    public DTStartMachineConfig DTStartMachineConfig { private set; get; }
    public DTStartProcessConfig DTStartProcessConfig { private set; get; }
    public DTStartSceneConfig DTStartSceneConfig { private set; get; }
    public DTStartZoneConfig DTStartZoneConfig { private set; get; }
    public DTOneConfig DTOneConfig { private set; get; }
    public DTAIConfig DTAIConfig { private set; get; }
    public DTUnitConfig DTUnitConfig { private set; get; }

    private Dictionary<string, IDataTable> _tables;

    public IEnumerable<IDataTable> DataTables => _tables.Values;

    public IDataTable GetDataTable(string tableName) => _tables.TryGetValue(tableName, out var v) ? v : null;

    public async Task LoadAsync(System.Func<string, Task<ByteBuf>> loader)
    {
        _tables = new Dictionary<string, IDataTable>();
        List<Task> loadTasks = new List<Task>();
        DTStartMachineConfig = new DTStartMachineConfig(() => loader("dtstartmachineconfig")); 
        loadTasks.Add(DTStartMachineConfig.LoadAsync());
        _tables.Add("DTStartMachineConfig", DTStartMachineConfig);
        DTStartProcessConfig = new DTStartProcessConfig(() => loader("dtstartprocessconfig")); 
        loadTasks.Add(DTStartProcessConfig.LoadAsync());
        _tables.Add("DTStartProcessConfig", DTStartProcessConfig);
        DTStartSceneConfig = new DTStartSceneConfig(() => loader("dtstartsceneconfig")); 
        loadTasks.Add(DTStartSceneConfig.LoadAsync());
        _tables.Add("DTStartSceneConfig", DTStartSceneConfig);
        DTStartZoneConfig = new DTStartZoneConfig(() => loader("dtstartzoneconfig")); 
        loadTasks.Add(DTStartZoneConfig.LoadAsync());
        _tables.Add("DTStartZoneConfig", DTStartZoneConfig);
        DTOneConfig = new DTOneConfig(() => loader("dtoneconfig")); 
        loadTasks.Add(DTOneConfig.LoadAsync());
        _tables.Add("DTOneConfig", DTOneConfig);
        DTAIConfig = new DTAIConfig(() => loader("dtaiconfig")); 
        loadTasks.Add(DTAIConfig.LoadAsync());
        _tables.Add("DTAIConfig", DTAIConfig);
        DTUnitConfig = new DTUnitConfig(() => loader("dtunitconfig")); 
        loadTasks.Add(DTUnitConfig.LoadAsync());
        _tables.Add("DTUnitConfig", DTUnitConfig);

        await Task.WhenAll(loadTasks);

        PostInit();
        DTStartMachineConfig.Resolve(_tables); 
        DTStartProcessConfig.Resolve(_tables); 
        DTStartSceneConfig.Resolve(_tables); 
        DTStartZoneConfig.Resolve(_tables); 
        DTOneConfig.Resolve(_tables); 
        DTAIConfig.Resolve(_tables); 
        DTUnitConfig.Resolve(_tables); 
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        DTStartMachineConfig.TranslateText(translator); 
        DTStartProcessConfig.TranslateText(translator); 
        DTStartSceneConfig.TranslateText(translator); 
        DTStartZoneConfig.TranslateText(translator); 
        DTOneConfig.TranslateText(translator); 
        DTAIConfig.TranslateText(translator); 
        DTUnitConfig.TranslateText(translator); 
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}