
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
public sealed partial class BufferNumerical : Luban.BeanBase
{
    public BufferNumerical(ByteBuf _buf) 
    {
        BufferId = _buf.ReadInt();
        Level = _buf.ReadInt();
        {int __n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);Args = new int[__n0];for(var __index0 = 0 ; __index0 < __n0 ; __index0++) { int __e0;__e0 = _buf.ReadInt(); Args[__index0] = __e0;}}
    }

    public static BufferNumerical DeserializeBufferNumerical(ByteBuf _buf)
    {
        return new BufferNumerical(_buf);
    }

    public readonly int BufferId;
    /// <summary>
    /// 等级
    /// </summary>
    public readonly int Level;
    /// <summary>
    /// arg_1
    /// </summary>
    public readonly int[] Args;
   
    public const int __ID__ = -462052488;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "bufferId:" + BufferId + ","
        + "level:" + Level + ","
        + "args:" + Luban.StringUtil.CollectionToString(Args) + ","
        + "}";
    }
}

}
