
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
public sealed partial class DialogueTrigger : Luban.BeanBase
{
    public DialogueTrigger(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        TriggerType = _buf.ReadInt();
        TriggerValue = _buf.ReadInt();
    }

    public static DialogueTrigger DeserializeDialogueTrigger(ByteBuf _buf)
    {
        return new DialogueTrigger(_buf);
    }

    public readonly int Id;
    /// <summary>
    /// Trigger类型
    /// </summary>
    public readonly int TriggerType;
    /// <summary>
    /// Trigger值
    /// </summary>
    public readonly int TriggerValue;
   
    public const int __ID__ = -592017248;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "triggerType:" + TriggerType + ","
        + "triggerValue:" + TriggerValue + ","
        + "}";
    }
}

}
