using System;
using System.Collections.Generic;
using UnityEngine;

public class Parameter<T> where T : struct
{
    public T value{get { return AgentGetter == null ? defaultValue : AgentGetter.Invoke(); }}
    public T defaultValue;
    private Func<T> AgentGetter;
    private Getter<T> getter;
    public Parameter(T defaultValue, Func<T> agentGetter)
    {
        this.defaultValue = defaultValue;
        this.AgentGetter = agentGetter;
    }
    public Parameter(T defaultValue, Getter<T> getter)
    {
        this.defaultValue = defaultValue;
        this.getter = getter;
        this.AgentGetter = getFromGetter;
    }
        private T getFromGetter() => getter.Get();
    public Parameter(T defaultValue)
    {
        this.defaultValue = defaultValue;
    }
    public void SetAgentGetter(Func<T> agentGetter)
    {
        if(agentGetter==null)
            return;
        this.AgentGetter = agentGetter;
    }
    public void SetGetter(Getter<T> getter)
    {
        if(getter==null)
            return;
        this.getter = getter;
        this.AgentGetter = getFromGetter;
    }
    public void Reset()
    {
        AgentGetter = null;
        getter = null;
    }

    public static implicit operator T(Parameter<T> parameter)
    {
        return parameter.value;
    }

    public static implicit operator Parameter<T>(T value)
    {
        return new Parameter<T>(value);
    }
    public static implicit operator Parameter<T>(Getter<T> getter)
    {
        return new Parameter<T>(default(T),getter);
    }
}

public class RefParameter<T> where T : class
{
    public T value{get { return AgentGetter == null ? defaultValue : AgentGetter.Invoke(); }}
    public T defaultValue;
    private Func<T> AgentGetter;
    private refGetter<T> getter;
    public RefParameter(T defaultValue, Func<T> agentGetter)
    {
        this.defaultValue = defaultValue;
        this.AgentGetter = agentGetter;
    }
    public RefParameter(T defaultValue, refGetter<T> getter)
    {
        this.defaultValue = defaultValue;
        this.getter = getter;
        this.AgentGetter = getFromGetter;
    }
        private T getFromGetter() => getter.Get();
    public RefParameter(T defaultValue)
    {
        this.defaultValue = defaultValue;
    }
    public void SetAgentGetter(Func<T> agentGetter)
    {
        if(agentGetter==null)
            return;
        this.AgentGetter = agentGetter;
    }
    public void SetGetter(refGetter<T> getter)
    {
        if(getter==null)
            return;
        this.getter = getter;
        this.AgentGetter = getFromGetter;
    }
    public void Reset()
    {
        AgentGetter = null;
        getter = null;
    }

    public static implicit operator T(RefParameter<T> parameter)
    {
        return parameter.value;
    }

    public static implicit operator RefParameter<T>(T value)
    {
        return new RefParameter<T>(value);
    }
    public static implicit operator RefParameter<T>(refGetter<T> getter)
    {
        return new RefParameter<T>(default(T),getter);
    }
}
