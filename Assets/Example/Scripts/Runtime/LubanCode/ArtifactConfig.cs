
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
public sealed partial class ArtifactConfig : Luban.BeanBase
{
    public ArtifactConfig(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        Name = _buf.ReadString();
        PassiveSkillId = _buf.ReadInt();
        Describe = _buf.ReadString();
    }

    public static ArtifactConfig DeserializeArtifactConfig(ByteBuf _buf)
    {
        return new ArtifactConfig(_buf);
    }

    /// <summary>
    /// Id
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 名字
    /// </summary>
    public readonly string Name;
    public readonly int PassiveSkillId;
    /// <summary>
    /// 祝福描述
    /// </summary>
    public readonly string Describe;
   
    public const int __ID__ = 1043471316;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "passiveSkillId:" + PassiveSkillId + ","
        + "describe:" + Describe + ","
        + "}";
    }
}

}
