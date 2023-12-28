using System;
using System.Collections.Generic;
using UnityEngine;

public class StageModule:IModule
{
    public EntityCore core{get;set;}

    public StageModule(EntityCore core)
    {
        this.core = core;
    }

    public StageModule()
    {
    }

    public Dictionary<int,Iperformer> performers = new Dictionary<int, Iperformer>();

    public Iperformer this[int index]{get{
        return performers[index];
    }}
    private int lastIndex=10000;

    public event Action UpdatePerformers;

    public void Update() {
        UpdatePerformers ?.Invoke();
    }

    public int addPerformer(Iperformer performer){
        performers.Add(lastIndex,performer);
        performer.register(this);
        return lastIndex++;
    }

    public void deletePerformer(int index){
        performers.Remove(index);
    }

    public Iperformer GetIperformer(int index){
        return performers[index];
    }

    public static void connet(Iperformer outPerformer,string outPort ,Iperformer inPerformer,string inPort  ){

        ((IconnectPort)(inPerformer.getInputPort(inPort)))?.connectOutputPort( outPerformer.getOutputPort(outPort) );

    }

    public void connet(int outPerformer,string outPort ,int inPerformer,string inPort  ){

        performers[inPerformer].getInputPort(inPort)?.connectOutputPort( performers[outPerformer].getOutputPort(outPort) );

    }


}
