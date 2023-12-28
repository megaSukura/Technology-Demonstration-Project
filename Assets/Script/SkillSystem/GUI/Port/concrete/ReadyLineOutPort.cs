using UnityEngine;
using System;
public class ReadyLineOutPort :TRefParameterOutputPort<SkillSystem.ReadyLine>
{
    public ReadyLineOutPort(Func<RefParameter<SkillSystem.ReadyLine>> parameterGetter) : base(parameterGetter)
    {
    }
}