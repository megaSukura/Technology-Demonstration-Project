using System;
using UnityEngine;
public class AttributeCommandHandler<T>{

    private BinaryHeap<AttributeCommand<T>> CommandCache=new BinaryHeap<AttributeCommand<T>>(3);
    public void Send(AttributeCommand<T> command){
        CommandCache.Enqueue(command);
    }
    public bool RemoveAll(Predicate< AttributeCommand<T> > match){
        return CommandCache.Remove(match) !=null ;
        
    }
    public void ExecuteAll(){
        
       // Debug.Log(CommandCache.Count);
        while (CommandCache.Count>0)
        {
            CommandCache.Dequeue().Execute();
        }
    }

}