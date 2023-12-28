using UnityEngine;
using System;
using SkillSystem;
public class SkillEffectOutPort :TRefParameterOutputPort<SkillEffectBase>
{
    public SkillEffectOutPort(Func<RefParameter<SkillEffectBase>> parameterGetter) : base(parameterGetter)
    {
    }
}