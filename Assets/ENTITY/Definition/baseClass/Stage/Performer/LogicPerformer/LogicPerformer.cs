using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LogicPerformer : Iperformer
{

    public enum LogicType
    {
        AND,
        OR,
        NOT,
    }

    public LogicType type;
    int inputNum=2;

    private Dictionary<string,PerformerInputPort<bool>> inputPort=new Dictionary<string, PerformerInputPort<bool>>();
    private PerformerOutputPort<bool> outputPort=new  PerformerOutputPort<bool>();

    public LogicPerformer(LogicType type, int inputNum=2)
    {
        this.type = type;
        this.inputNum = inputNum;
    }

    public string[] getAllInputPort()
    {
        return inputPort.Keys.ToArray();
    }

    public string[] getAllOutputPort()
    {
        return new []{"output"};
    }

    public dynamic getInputPort(string name)
    {
        return inputPort.ContainsKey(name)? inputPort[name] : null ;
    }

    public dynamic getOutputPort(string name)
    {
        return outputPort;
    }

    public void Init()
    {
        if (type==LogicType.NOT)
        {
            inputNum=1;
            inputPort.Add("input", new PerformerInputPort<bool>("input") );
            inputPort["input"].inputEvent+=(_=>  operations());
            
        }else{
            PerformerInputPort<bool> _in;
            for (int i = 1; i <= inputNum; i++)
            {   
                var _name ="input "+i.ToString();

                _in = new PerformerInputPort<bool>(_name);
                _in.inputEvent+=(_=>  operations());
                inputPort.Add(_name, _in );
            }
            }
        
        //outputPort.Add( "output", new PerformerOutputPort<bool>());


    }
    public StageModule my_system;

    public void register(StageModule stageModule){ my_system = stageModule; }
    public void operations(){
        bool _out=false;
        switch (type)
        {
            case LogicType.AND:
                _out=true;
                foreach (var port in inputPort)
                {
                    _out&=port.Value.GetValue();
                }
                break;
            case LogicType.OR:
                _out=false;
                foreach (var port in inputPort)
                {
                    _out|=port.Value.GetValue();
                }
                break;

            case LogicType.NOT:
                _out = !inputPort["input"].GetValue();
                break;

            default:break;
        }
#if UNITY_EDITOR
        Debug.Log(type.ToString()+inputNum+": "+_out);
#endif
        outputPort.output(_out);
    }


}
