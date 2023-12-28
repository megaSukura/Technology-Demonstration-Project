using System;
using System.Collections.Generic;
using UnityEngine;
/*
output     =>      input     =>        other
    |                |
    outputEvent      inputEvent

*/

public class InputPort<T>:IconnectPort
{
    public string name;
    public string valueType=typeof(T).ToString();

    private OutputPort<T> connetedOutputPort;
    public T in_value;

    public event Action<T> inputEvent;
    public bool ifTypeMatch(object a){
        return a is OutputPort<T>;
    }
    
    public void connectOutputPort(object output){

        if(ifTypeMatch(output)){
            if(connetedOutputPort!=null){
                connetedOutputPort.outputEvent-=input;
            }
        ((OutputPort<T> )output).outputEvent+=input;
        connetedOutputPort=(OutputPort<T> )output;}
        else
        return;
    }
    public void disConnectOutputPort(){
        if(connetedOutputPort!=null){
                connetedOutputPort.outputEvent-=input;
            }
        connetedOutputPort=null;
    }

    //私有
    public void input(T value){
        in_value = value;
        inputEvent ?.Invoke(value);
    }

    public T GetValue(){
        return in_value;
    }


}


public class OutputPort<T>{

   public string name;
    public string valueType=typeof(T).ToString();



    public T out_value;

    public event Action<T> outputEvent;

    //公有
    public void output(T value){
        out_value = value;//??? 
        outputEvent?.Invoke(out_value);
    }

    public void output(T value,T defult_value){
        out_value = value;//??? 
        outputEvent?.Invoke(out_value);
        out_value=defult_value;
    }
    public T GetValue(){
        return out_value;
    }

}

interface IconnectPort{
    public void connectOutputPort(object output);
    public void disConnectOutputPort();
}
