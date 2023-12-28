public abstract class Getter<T> :IGetter<T> where T : struct
{
    public abstract T Get();
}

public interface IGetter<T>
{
    T Get();
}