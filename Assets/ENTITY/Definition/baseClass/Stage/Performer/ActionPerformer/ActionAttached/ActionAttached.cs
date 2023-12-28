using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionAttached
{
    public string name;

    public ActorAction myActorAction;

    public virtual void init(){}

    public virtual void undo(){}

    public virtual void actionStart(){}

    public virtual void actionUpdate(){}

    public virtual void actionEnd(){}


}
