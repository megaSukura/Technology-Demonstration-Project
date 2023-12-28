
using System;
public class ResponsiveValue<T>
{
    
    public virtual T Value{
        get{
            return _value;
        }
        set{
            if(!_value.Equals(value)){
                CallValueChangeEvent(_value,value);
                _value=value;
            }
        }
    }

    protected T _value;

    public T set(T val)=>Value=val;
    public T get()=>Value;

    public event Action<T,T> OnValueChange;

    public void CallValueChangeEvent(T old_value,T new_value) => OnValueChange?.Invoke(old_value,new_value);

    
}
