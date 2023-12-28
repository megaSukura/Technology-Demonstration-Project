using UnityEngine;
using System;
using SkillSystem;
public class SkillOutPort :TRefParameterOutputPort<Skill>
{
    public SkillOutPort(Func<RefParameter<Skill>> parameterGetter) : base(parameterGetter)
    {
    }
    public override RefParameter<Skill> Build()
    {
        
        instance = ParameterGetter();
        return instance;
    }
}