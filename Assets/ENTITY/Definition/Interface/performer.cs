using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iperformer
{
    dynamic getInputPort(string name);
    dynamic getOutputPort(string name);
    string[] getAllInputPort();
    string[] getAllOutputPort();

    void Init();
    //TODO:delete()!!!!
    public void register(StageModule stageModule);
}
