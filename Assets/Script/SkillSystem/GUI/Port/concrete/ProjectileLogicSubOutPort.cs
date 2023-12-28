using UnityEngine;
using System;
public class ProjectileLogicSubOutPort :TRefParameterOutputPort<ProjectileLogicSub>
{
    public ProjectileLogicSubOutPort(Func<RefParameter<ProjectileLogicSub>> parameterGetter) : base(parameterGetter)
    {
    }
}