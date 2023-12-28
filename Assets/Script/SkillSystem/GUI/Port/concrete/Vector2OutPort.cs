using UnityEngine;
using System;
public class Vector2OutPort :TParameterOutputPort<Vector2>
{
    public Vector2OutPort(Func<Parameter<Vector2>> parameterGetter) : base(parameterGetter)
    {
    }
}