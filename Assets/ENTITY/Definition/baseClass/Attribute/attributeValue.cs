
using System.Collections.Generic;
using UnityEngine;

public class AttributeValue<T> : ResponsiveValue<T>
{
    public string name;
    public override T Value { 
        get => base.Value; 
        set {
            if(!_value.Equals(value)){
                
                T new_v=value;  T old_v = _value;
                T temp;     bool isChange = true;
                foreach (var HandlerSortCtx in valueChangeHandlersSort)
                {
                    temp = new_v;
                    new_v = valueChangeHandlers[HandlerSortCtx.name](old_v,new_v,out isChange);
                    old_v=temp;
                    if(!isChange) return;
                }
                if(isChange)CallValueChangeEvent(_value,new_v);
                _value=new_v;
            }
        } 
        }

    

    #region valueChangeHandler
    protected Dictionary<string,ValueChangeHandler<T>> valueChangeHandlers = new Dictionary<string,ValueChangeHandler<T>>();
    //之所以使用string间接储存而不是直接存委托的原因是方便使用lamda表达式加减
    //不用heap的原因是需要handler持续存在
    protected List<(string name,int order)> valueChangeHandlersSort = new List<(string name,int order)>();

    public AttributeValue(T value)
    {
        Value = value;
    }

    public AttributeValue(string name, T value)
    {
        this.name = name;
        Value = value;
    }

    public bool AddValueChangeHandler(ValueChangeHandler<T> handler,string handlerName,int priority = 0){
        
        if( valueChangeHandlers.TryAdd(handlerName,handler)){
            valueChangeHandlersSort.Add((handlerName,priority));
            OrderPriority();
            return true;
        }else return false;
        
    }
    public int GetPriorityOfHandler(string handlerName){
        return valueChangeHandlersSort.Find((e)=>{return e.name==handlerName;}).order;
    }
    public void SetPriorityOfHandler(string handlerName,int priority){
        if(valueChangeHandlersSort.RemoveAll((ctx)=>{ return ctx.name==handlerName; }) >0)
            valueChangeHandlersSort.Add((handlerName,priority));
        OrderPriority();
    }
    public bool RemoveValueChangeHandler(string handlerName){
        valueChangeHandlersSort.RemoveAll((unitCtx)=>{ return unitCtx.name==handlerName; });
        OrderPriority();
        return valueChangeHandlers.Remove(handlerName);
    }
private void OrderPriority(){
        valueChangeHandlersSort.Sort((e1,e2)=>{ return e2.order.CompareTo(e1.order);});
    }
    #endregion
    #region AttributeCommandHandler
    public AttributeCommandHandler<T> AttributeCommandHandler { get => attributeCommandHandler??( attributeCommandHandler = new AttributeCommandHandler<T>()); }
    private AttributeCommandHandler<T> attributeCommandHandler;
    public void SendCommand(AttributeCommand<T> command)=> AttributeCommandHandler.Send(command);
    public void ExecuteAllCommand()=>AttributeCommandHandler.ExecuteAll();
    #endregion
    

}
#region delegate type define
public delegate T ValueChangeHandler<T>(T oldValue,T newValue,out bool isChange);
public delegate T ValueDecoratorHandler<T>(T Value);
#endregion


/// <summary>
/// 用于处理属性值装饰器的类
/// </summary>
/// <typeparam name="属性值类型"></typeparam>
 public class ValueDecorator<TT> {
    public string name;
    protected ValueDecoratorHandler<TT> valueDecoratorHandler;
    private bool isBind=false;

    public void SetHandler(ValueDecoratorHandler<TT> handler){
        valueDecoratorHandler=handler;
        isBind=true;
    }

    public void CleanHandler()=>valueDecoratorHandler=null;

    public TT decorate(TT value){
        if(value!=null&&isBind)
        return valueDecoratorHandler.Invoke(value);
        else return value;
    }

 }
public static class ValueDecoratorHandlerExtension {
    public static T decorate<T>(this T v,ValueDecorator<T> Decorator){
        return Decorator.decorate(v);
    }

    public static T decorate<T>(this T v,ValueDecoratorHandler<T> Decorator){
        return Decorator.Invoke(v);
    }

}