using System;
using System.Collections.Generic;
using UnityEngine.Pool;
public class AttributeModule : IModule
{
    public EntityCore core { get; set ;}
    public AttributeModule(EntityCore core)
    {
        this.core = core;
    }
    private Dictionary<string ,object> _allAttribute=new Dictionary<string, object>();
    private event Action commandHandle;
    public AttributeValue<T> attributeValue<T>(string name ,T defaultValue ) where T:struct
    {   
        if(_allAttribute.TryGetValue(name,out var t)){
            return (AttributeValue<T>) t;
        }
        var avt=new AttributeValue<T>(defaultValue);
        commandHandle+=avt.ExecuteAllCommand;
        _allAttribute.Add(name,avt );
        return avt;
    }
    public AttributeFloat attributeFloat(string name ,float defaultValue=0f)
    {
        if(_allAttribute.TryGetValue(name,out var t)){
            return (AttributeFloat) t;
        }
        var avt=new AttributeFloat(defaultValue);
        commandHandle+=avt.ExecuteAllCommand;
        _allAttribute.Add(name,avt );
        return avt;
    }
    public void processCommand()=>commandHandle?.Invoke();

    public void Destroy(){
        commandHandle=null;
        _allAttribute.Clear();
    }

#region (static)Expressions

public static Dictionary<string,Expression> Expressions;//public

public static float calculation(string ExpressionName,float[] Params){
    return Expressions[ExpressionName].Invoke(Params);//
     
}
    
#endregion


}

public class AttributeFloat:AttributeValue<float>{
#region Follow
    public void Follow(AttributeFloat targetFloat){
        targetFloat.OnValueChange+=_follow_;
        _follow_target = targetFloat;
    }

    public void DisFollow(){
        if( _follow_target==null)
            return;
        _follow_target.OnValueChange-=_follow_;
        _follow_target=null;
    }

    private AttributeFloat _follow_target;

    public AttributeFloat(float value=0f) : base(value)
    {
    }

    private void _follow_(float old_v,float new_v){
        this.Value=new_v;
    }
#endregion
#region PanleComponent
        public PanleFramework panleFramework=new PanleFramework(null);
        public PanleFramework SetPanleComponent(List<PanleLayer> layers,List<PanleLayer> layersToInclude){
            panleFramework._layers=layers;
             panleFramework.LayersToInclude=layersToInclude;
            return panleFramework;
        }
        public float PanleValue{
            get{
                return panleFramework.CalculatePanle(Value);
            }
        }
        
    #endregion

}

public class AttributeInt : AttributeValue<int>
{
    public AttributeInt(int value=0) : base(value)
    {
    }
}

public delegate float  Expression(params float[] Paramas);

/*
#region Float set get
    protected Dictionary<string,AttributeFloat> FloatValues= new Dictionary<string, AttributeFloat>();

    public AttributeFloat setFloat(string name ,float value=0f){
        if(FloatValues.ContainsKey(name)){
            FloatValues[name].Value = value;
            return FloatValues[name];
        }
        else{
        var t= new AttributeFloat(value);
        if(FloatValues.TryAdd(name,t))
            return t;
        else throw new System.Exception("Duplicate names");
        }
    }
     
    public AttributeFloat getFloat(string name ,float defaultValue=0f){
        var t = FloatValues[name];
        if(t!=null)
        return t;
        else return setFloat(name,defaultValue);
    }
#endregion 

#region Int get set
    protected Dictionary<string,AttributeInt> IntValues= new Dictionary<string, AttributeInt>();

    public AttributeInt setInt(string name ,int value){
        if(IntValues.ContainsKey(name)){
            IntValues[name].Value = value;
            return IntValues[name];
        }
        else{
        var t= new AttributeInt(value);
        if(IntValues.TryAdd(name,t))
            return t;
        else throw new System.Exception("Duplicate names");
        }

    }
     
    public AttributeInt getInt(string name ,int defaultValue){
        var t = IntValues[name];
        if(t!=null)
        return t;
        else return setInt(name,defaultValue);
    }
#endregion 
*/