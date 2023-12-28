using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
///实体核心类,用于管理实体的所有模块
/// </summary>
public class EntityCore : MonoBehaviour
{
public moduleType moduleTypes=moduleType.None;

public NodeModle nodeModle;
public StageModule stageModule;

public AttributeModule attributeModule;
public BuffModule buffModule;

//ArtistModule

private void Awake()
    {
        Init();
    }

private void Update() {

    if(stageModule!=null){
        stageModule.Update();
    }
        
    }

void LateUpdate()
{
    if(attributeModule!=null){
        attributeModule.processCommand();
    }
}

void OnDestroy()
{
    if(attributeModule!=null){
        attributeModule.Destroy();
    }
}

    private void Init()
    {
        var _t = moduleTypes;
        var i = 0;
        while (_t != 0 && i <= 3)
        {
            if (((int)_t & (1 << i)) != 0)
            {
                createModle((moduleType)(1 << i));
                
                _t = _t & (~(moduleType)(1 << i));
            }
            i++;
        }
    }

public bool containModle(moduleType type){

    return (moduleTypes&type)!=0;

}

public void createModle(moduleType type){
    switch (type)
    {   
        case moduleType.Node:
        if(nodeModle==null){
            nodeModle=new NodeModle(this);
            moduleTypes|= moduleType.Node;
        }break;

        case moduleType.Stage:
        if(stageModule==null){
            stageModule=new StageModule(this);
            moduleTypes|= moduleType.Stage;
        }break;

        case moduleType.Attribute:
        if(attributeModule==null){
            attributeModule=new AttributeModule(this);
            moduleTypes|= moduleType.Attribute;
        }break;

        case moduleType.Buff:
        if(buffModule==null){
            buffModule=new BuffModule(this);
            moduleTypes|= moduleType.Buff;
        }break;
        
        default:break;
    }
}

public object GetModule(moduleType type){

    switch (type)
    {   
        case moduleType.Node:
        if(nodeModle!=null){
            return nodeModle;
        }else
        {
            createModle(type);
            return nodeModle;
        }
        

        case moduleType.Stage:
        if(stageModule!=null){
            return stageModule;
        }else
        {
            createModle(type);
            return stageModule;
        }
        
        case moduleType.Attribute:
        if(attributeModule!=null){
            return attributeModule;
        }else
        {
            createModle(type);
            return attributeModule;
        }
        case moduleType.Buff:
        if(buffModule!=null){
            return buffModule;
        }else
        {
            createModle(type);
            return buffModule;
        }
        
        default:return null;

    }
    

}

}
[Flags]
public enum moduleType
{
    None=       0,
    Node=       1<<0,
    Stage=      1<<1,
    Attribute=  1<<2,
    Buff=       1<<3,
    Artist=     1<<4
}

public static class gameobjectEntityCoreExtention{

    public static EntityCore GetEntityCore(this GameObject gameObject){
        var a = gameObject.GetComponent<EntityCore>();
        return a?a:gameObject.AddComponent<EntityCore>();
    }

    public static StageModule GetStageModule(this GameObject gameObject){
        var s = gameObject.GetEntityCore().GetModule(moduleType.Stage);
        return (StageModule)s;

    }

    public static NodeModle GetNodeModule(this GameObject gameObject){
        var s = gameObject.GetEntityCore().GetModule(moduleType.Node);
        return (NodeModle)s;

    }

    public static AttributeModule GetAttributeModule(this GameObject gameObject){
        var s = gameObject.GetEntityCore().GetModule(moduleType.Attribute);
        return (AttributeModule)s;

    }

    public static BuffModule GetBuffModule(this GameObject gameObject){
        var s = gameObject.GetEntityCore().GetModule(moduleType.Buff);
        return (BuffModule)s;

    }
}