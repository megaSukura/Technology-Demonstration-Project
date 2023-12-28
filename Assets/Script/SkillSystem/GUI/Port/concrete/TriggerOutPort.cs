using UnityEngine;
using System;
public class TriggerOutPort :TRefParameterOutputPort<SkillSystem.Trigger>
{
    public TriggerOutPort(Func<RefParameter<SkillSystem.Trigger>> parameterGetter) : base(parameterGetter)
    {
    }
}