using System;
using UnityEngine;
public class PriorityCommandHandler<T>where T:PriorityCommand{

    private BinaryHeap<T> CommandCache=new BinaryHeap<T>(3);
    public virtual void Send(T command){
        CommandCache.Enqueue(command);
    }

    public bool RemoveAll(Predicate< T > match){
        return CommandCache.Remove(match) !=null ;
        
    }

    public void ExecuteAll(){
        Debug.Log(CommandCache.Count);
        while (CommandCache.Count>0)
        {
            CommandCache.Dequeue().Execute();
        }
    }

}