using UnityEngine;
using System;
public class FloatGetterOutPort :TRefParameterOutputPort<Getter<float>>
{
    public FloatGetterOutPort(Func<RefParameter<Getter<float>>> parameterGetter) : base(parameterGetter)
    {
    }
}