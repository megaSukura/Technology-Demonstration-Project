using System;


public class PriorityCommand : PriorityCommandBase
{   
    
    protected Action ExecuteAction;

    protected PriorityCommand(int priority,Action executeAction)
    {
        this.Priority=priority;
        ExecuteAction = executeAction;
    }

    public override void Execute()
    {
        ExecuteAction?.Invoke();
    }

}
