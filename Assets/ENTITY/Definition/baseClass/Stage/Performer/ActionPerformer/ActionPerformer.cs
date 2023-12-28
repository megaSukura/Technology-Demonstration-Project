using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ActorActionPerformer : Iperformer
{
    public string[] getAllInputPort()
    {
        throw new System.NotImplementedException();
    }

    public string[] getAllOutputPort()
    {
        throw new System.NotImplementedException();
    }

    public dynamic getInputPort(string name)
    {
        throw new System.NotImplementedException();
    }

    public dynamic getOutputPort(string name)
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    public void register(StageModule stageModule)
    {
        throw new System.NotImplementedException();
    }
}











public class PerformerInputPort<T>:InputPort<T>{

    public string description=string.Empty;

    public PerformerInputPort(string name="")
    {
        this.name = name;
    }
}

public class PerformerOutputPort<T>:OutputPort<T>{

    public string description=string.Empty;

    public PerformerOutputPort(string name="")
    {
        this.name = name;
    }
    
}