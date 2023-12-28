using UnityEngine;
using System;
public class EntityOutPort :TRefParameterOutputPort<GameEntity>
{
    public EntityOutPort(Func<RefParameter<GameEntity>> parameterGetter) : base(parameterGetter)
    {
    }
}