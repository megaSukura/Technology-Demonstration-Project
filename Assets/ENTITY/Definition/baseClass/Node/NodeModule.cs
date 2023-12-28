using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeModle:IModule
{   public EntityCore core{get;set;}
    public Dictionary<string,LimbNode> limbNodes = new Dictionary<string, LimbNode>();

    public NodeModle(EntityCore core)
    {
        this.core = core;
    }

    
    public LimbNode this[string name]{ 
        get{
            LimbNode _out;
            limbNodes.TryGetValue(name,out _out);
            return _out?_out:null;
        }
     }

}
