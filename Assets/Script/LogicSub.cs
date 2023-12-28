using System.Collections.Generic;
using UnityEditor;
//因为作用限制,所以这个类不能用对象池,只能GC
public abstract class LogicSub
{
    public int priority;
    /// <summary>
    /// 逻辑子程序的执行
    /// </summary>
    /// <>true:继续执行下一个逻辑子程序</>
    /// <>false:停止执行逻辑子程序</>
    
    public abstract bool Execute();
    public virtual void OnEnd(){}
    #if UNITY_EDITOR
    /// <summary>
    /// 绘制逻辑子程序的编辑器
    /// </summary>
    public virtual void OndrawInspector()
    {
        //name
        EditorGUILayout.LabelField("LogicSub");
    }
    #endif
}


public class LogicSubhandler
{
    public List<LogicSub> logicSubs = new List<LogicSub>();
    public T FindLogicSub<T>() where T : LogicSub
    {
        return logicSubs.Find((logicSub) => logicSub is T) as T;
    }
    public void AddLogicSub(LogicSub logicSub)
    {
        logicSubs.Add(logicSub);
        logicSubs.Sort((a, b) => a.priority.CompareTo(b.priority));
    }
    public void RemoveLogicSub(LogicSub logicSub)
    {
        logicSubs.Remove(logicSub);
        logicSub.OnEnd();
    }
    public void RemoveAllLogicSub()
    {   
        foreach(var logicSub in logicSubs)
            logicSub.OnEnd();
        logicSubs.Clear();
    }
    /// <summary>
    /// 逻辑子程序的执行
    /// </summary>
    public void UpdateLogicSubs()
    {
        for (int i = 0; i < logicSubs.Count; i++)
        {
            if (!logicSubs[i].Execute())
             return;
        }
    }
}