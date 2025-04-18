
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;


namespace cfg.chest
{
public partial class TbChest
{
    private readonly System.Collections.Generic.Dictionary<int, ChestConfig> _dataMap;
    private readonly System.Collections.Generic.List<ChestConfig> _dataList;
    
    public TbChest(ByteBuf _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, ChestConfig>();
        _dataList = new System.Collections.Generic.List<ChestConfig>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            ChestConfig _v;
            _v = ChestConfig.DeserializeChestConfig(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, ChestConfig> DataMap => _dataMap;
    public System.Collections.Generic.List<ChestConfig> DataList => _dataList;

    public ChestConfig GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public ChestConfig Get(int key) => _dataMap[key];
    public ChestConfig this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}
