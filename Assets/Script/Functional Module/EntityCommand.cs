using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;

public class EntityCommand : PriorityCommand
{
    public static ObjectPool<EntityCommand> pool=new ObjectPool<EntityCommand>(createEntityCommand,null,
    command=>{
        command.Sender=null;
        command.Target=null;
        command.concreteExecuteAction=null;
        command.Priority=0;
    },
    defaultCapacity:20);
    private static EntityCommand createEntityCommand(){
        var t=new EntityCommand(0);
        t.ExecuteAction=t.ExecuteConcreteAction;
        return t;
        
    }
    public GameEntity Sender;
    public GameEntity Target;
    public EntityCommand Signature(GameEntity sender ){
        Sender=sender;
        return this;
    }
    public EntityCommand SetTarget(GameEntity target){
        Target=target;
        return this;
    }
    protected EntityCommand(int priority) : base(priority, null)
    {
    }

    protected Action<EntityCommand> concreteExecuteAction;
    
    private void ExecuteConcreteAction(){
        concreteExecuteAction?.Invoke(this);
    }
    public void SetConcreteExecuteAction(Action<EntityCommand> action){
        concreteExecuteAction=action;
    }

    public static EntityCommand CreateCommand(Action<EntityCommand> action,int priority=0,GameEntity sender=null,GameEntity target=null){
        var t=pool.Get();
        t.SetConcreteExecuteAction(action);
        t.Signature(sender);
        t.SetTarget(target);
        return t;
    } 

}
