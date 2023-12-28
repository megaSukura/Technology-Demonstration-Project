using UnityEngine;

public class ActorEventInputDatas    {

    public string stringParameter ;
        //
        // 摘要:
        //     Float parameter that is stored in the event and will be sent to the function.
    public float floatParameter ;
        //
        // 摘要:
        //     Int parameter that is stored in the event and will be sent to the function.
    public int intParameter ;
        //
        // 摘要:
        //     Object reference parameter that is stored in the event and will be sent to the
        //     function.
    public Object objectReferenceParameter ;
        //
        // 摘要:
        //     The name of the function that will be called.
        
        //
        // 摘要:
        //     The time at which the event will be fired off.
    public float time ;

    public ActorEventInputDatas(string _stringParameter,float _floatParameter,int _intParameter,Object _objectReferenceParameter,float _time){
        stringParameter = _stringParameter;
        floatParameter = _floatParameter;
        intParameter = _intParameter;
        objectReferenceParameter = _objectReferenceParameter;
        time = _time;
    }

}