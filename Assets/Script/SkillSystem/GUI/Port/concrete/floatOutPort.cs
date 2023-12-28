using UnityEngine;
using System;
public class floatOutPort :TParameterOutputPort<float>
{
    public floatOutPort(Func<Parameter<float>> parameterGetter) : base(parameterGetter)
    {
    }
}