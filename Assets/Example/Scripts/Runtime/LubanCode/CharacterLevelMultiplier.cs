
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;


namespace cfg
{
public sealed partial class CharacterLevelMultiplier : Luban.BeanBase
{
    public CharacterLevelMultiplier(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        MultiplierA = _buf.ReadFloat();
        MultiplierB = _buf.ReadFloat();
    }

    public static CharacterLevelMultiplier DeserializeCharacterLevelMultiplier(ByteBuf _buf)
    {
        return new CharacterLevelMultiplier(_buf);
    }

    /// <summary>
    /// Level
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 4星 成长系数
    /// </summary>
    public readonly float MultiplierA;
    /// <summary>
    /// 5星 成长系数
    /// </summary>
    public readonly float MultiplierB;
   
    public const int __ID__ = -1468683748;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "multiplierA:" + MultiplierA + ","
        + "multiplierB:" + MultiplierB + ","
        + "}";
    }
}

}
