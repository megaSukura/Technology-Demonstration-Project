using System;


public abstract class PriorityCommandBase : IComparable<PriorityCommandBase>
{
    public int Priority;

    public virtual void Execute()
    {

    }

    public int CompareTo(PriorityCommandBase other)
    {
        return other.Priority.CompareTo(this.Priority);
    }
}
