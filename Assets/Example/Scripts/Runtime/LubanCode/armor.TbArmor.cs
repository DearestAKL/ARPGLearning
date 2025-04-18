
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;


namespace cfg.armor
{
public partial class TbArmor
{
    private readonly System.Collections.Generic.Dictionary<int, ArmorConfig> _dataMap;
    private readonly System.Collections.Generic.List<ArmorConfig> _dataList;
    
    public TbArmor(ByteBuf _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, ArmorConfig>();
        _dataList = new System.Collections.Generic.List<ArmorConfig>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            ArmorConfig _v;
            _v = ArmorConfig.DeserializeArmorConfig(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, ArmorConfig> DataMap => _dataMap;
    public System.Collections.Generic.List<ArmorConfig> DataList => _dataList;

    public ArmorConfig GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public ArmorConfig Get(int key) => _dataMap[key];
    public ArmorConfig this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}
