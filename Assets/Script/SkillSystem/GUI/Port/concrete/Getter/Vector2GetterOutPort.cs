using UnityEngine;
using System;
public class Vector2GetterOutPort :TRefParameterOutputPort<Getter<Vector2>>
{
    public Vector2GetterOutPort(Func<RefParameter<Getter<Vector2>>> parameterGetter) : base(parameterGetter)
    {
    }
}