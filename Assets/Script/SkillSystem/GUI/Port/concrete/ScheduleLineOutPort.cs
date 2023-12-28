using UnityEngine;
using System;
public class ScheduleLineOutPort :TRefParameterOutputPort<SkillSystem.ScheduleLine>
{
    public ScheduleLineOutPort(Func<RefParameter<SkillSystem.ScheduleLine>> parameterGetter) : base(parameterGetter)
    {
    }
}