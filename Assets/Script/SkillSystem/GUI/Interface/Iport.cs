
public interface IUIportOut<T> where T :struct
{
    public NodeBase Owner{get;}
    Parameter<T> Build();
}
public interface IUIportIn<T> where T :struct
{
    public NodeBase Owner{get;}
    void OnConnect(IUIportOut<T> OutPort);
    void OnDesconnect(IUIportOut<T> OutPort);
}

public interface IUIportRefOut<T> where T : class
{
    public NodeBase Owner{get;}
    RefParameter<T> Build();
}
public interface IUIportRefIn<T> where T : class
{
    public NodeBase Owner{get;}
    void OnConnect(IUIportRefOut<T> OutPort);
    void OnDesconnect(IUIportRefOut<T> OutPort);
}