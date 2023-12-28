using System;
using UnityEngine;
public class EntityCommandHandler{

    private BinaryHeap<EntityCommand> CommandCache=new BinaryHeap<EntityCommand>(3);
    public virtual void Send(EntityCommand command){
        CommandCache.Enqueue(command);
    }

    public bool RemoveAll(Predicate< EntityCommand > match){
        var t= CommandCache.Remove(match);
        if(t==null)
            return false;
        //EntityCommand.pool.Release(t);
        return true;
    }

    public void ExecuteAll(){
        //Debug.Log(CommandCache.Count);
        while (CommandCache.Count>0)
        {//Debug.Log(CommandCache.Count);
            var t=CommandCache.Dequeue();
            t.Execute();
            //EntityCommand.pool.Release(t);
        }
    }

}