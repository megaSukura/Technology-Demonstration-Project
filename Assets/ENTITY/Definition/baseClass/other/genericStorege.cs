using System;
using System.Collections.Generic;

public class storage{
    private Dictionary<string,int> _intL;
    private Dictionary<string,float> _floatL;
    private Dictionary<string,string> _stringL;
    private Dictionary<string,bool> _boolL;

    private Dictionary<string,int> intL{get{ return _intL??( new Dictionary<string,int>() ); }}
    private Dictionary<string,float> floatL { get { return _floatL ?? (new Dictionary<string,float>()); } }
    private Dictionary<string,string> stringL { get { return _stringL ?? (new Dictionary<string,string>()); } }
    private Dictionary<string,bool> boolL { get { return _boolL ?? (new Dictionary<string,bool>()); } }

    public T Get<T>(string name) where T :struct
    {
        if (typeof(T) == typeof(float))
    {
        return (T)(object)floatL[name];
    }
        if (typeof(T) == typeof(int))
    {
        return (T)(object)intL[name];
    }
    else if (typeof(T) == typeof(string))
    {
        return (T)(object)stringL[name];
    }
    else if (typeof(T) == typeof(bool))
    {
        return (T)(object)boolL[name];
    }
    else
    {
        throw new ArgumentException("Unsupported type");
    }
    }

    public void Set<T>(string name, T value) where T : struct
{
    if (typeof(T) == typeof(float))
    {
        floatL[name] = (float)(object)value;
    }
    else if (typeof(T) == typeof(int))
    {
        intL[name] = (int)(object)value;
    }
    else if (typeof(T) == typeof(string))
    {
        stringL[name] = (string)(object)value;
    }
    else if (typeof(T) == typeof(bool))
    {
        boolL[name] = (bool)(object)value;
    }
    else
    {
        throw new ArgumentException("Unsupported type");
    }
}

}