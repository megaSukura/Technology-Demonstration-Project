using System;

public class AttributeCommand<T> : PriorityCommand
{
    public AttributeValue<T> target;
    public EntityCore Sender;

    protected AttributeCommand(int priority,EntityCore sender) : base(priority, null)
    {
        Sender = sender;
    }
    protected AttributeCommand(int priority) : base(priority, null)
    {
    }

    public void Signature(EntityCore entity){
        Sender=entity;
    }




}
#region  具体 Command
public class AttributeChangeCommand<T> : AttributeCommand<T>
{
    protected AttributeChangeCommand(int priority) : base(priority)
    {
    }

    protected AttributeChangeCommand(int priority, EntityCore sender) : base(priority, sender)
    {
    }
    #region Change command
    
    public T TargetValue;
    protected void ExecuteChange(){
        target.Value = TargetValue;
    }
    public static AttributeChangeCommand<T> ChangeCommand(T _targetValue,AttributeValue<T> _target,int priority=0){
        var t= new AttributeChangeCommand<T>(priority);
        t.target=_target;
        t.TargetValue = _targetValue;
        t.ExecuteAction = t.ExecuteChange;
        return t;
        
    }

#endregion

}

public class AttributeCustomCommand<T> : AttributeCommand<T>
{
    protected AttributeCustomCommand(int priority) : base(priority)
    {
    }
    protected AttributeCustomCommand(int priority, EntityCore sender) : base(priority, sender)
    {
    }

    #region custom command
    protected Action<AttributeCustomCommand<T>> customCommand;
    protected void ExecuteCustom(){
        customCommand(this);
    }
    public static AttributeCustomCommand<T> CustomCommand( Action<AttributeCustomCommand<T>> command ,AttributeValue<T> _target,int priority){
        var t= new AttributeCustomCommand<T>(priority);
        t.target=_target;
        t.customCommand = command;
        t.ExecuteAction =t.ExecuteCustom;
        return t;
        
    }
    
#endregion
}
#endregion

public class FloatChangeCommand : AttributeCommand<float>
{
    protected FloatChangeCommand(int priority) : base(priority)
    {
    }

    protected FloatChangeCommand(int priority, EntityCore sender) : base(priority, sender)
    {
    }
    #region Change command
    
    public float TargetValue;
    protected void ExecuteChange(){
        target.Value += TargetValue;
    }
    public static FloatChangeCommand ChangeCommand(float changeNum,AttributeValue<float> _target,int priority=0){
        var t= new FloatChangeCommand(priority);
        t.target=_target;
        t.TargetValue = changeNum;
        t.ExecuteAction = t.ExecuteChange;
        return t;
        
    }
    
#endregion

}

#region 扩展

public static class AttributeCommandExtend{

    public static AttributeCommand<T> GetBlockCommand<T>( AttributeValue<T> valueToBlock, AttributeCommand<T> commandToBlock ) where T :struct
    {
        return AttributeCustomCommand<T>.CustomCommand(
                (_)=>{
                    valueToBlock.AttributeCommandHandler.RemoveAll(
                        (o)=>{
                             return o.Equals(commandToBlock); 
                             });
                 },
                 valueToBlock,
                 100);

    }

    public static AttributeCommand<T> GetCommandToBlockThis<T>( this AttributeCommand<T> commandToBlock ) where T :struct
    {
        return GetBlockCommand<T>( commandToBlock.target,commandToBlock );
    }

}
    
#endregion