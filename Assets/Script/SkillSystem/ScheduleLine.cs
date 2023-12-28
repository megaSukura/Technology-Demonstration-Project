
using System;
using UnityEngine;
namespace SkillSystem
{
public class ScheduleLine
{
    public Skill from;
    public Skill to;
    public Func<bool> AgentTrigger;//TODO:专门的trigger对象
    public Trigger trigger;
    public bool IsResetTargetReady = false;
    public bool IsEndFrom = false;
    //set
    public ScheduleLine SetIsResetTargetReady(bool isResetTargetReady)
    {
        IsResetTargetReady = isResetTargetReady;
        return this;
    }
    public ScheduleLine SetIsEndFrom(bool isEndFrom)
    {
        IsEndFrom = isEndFrom;
        return this;
    }
    //

        public ScheduleLine(Skill from, Skill to, Func<bool> trigger, bool isResetTargetReady, bool isEndFrom)
        {
            this.from = from;
            this.to = to;
            this.AgentTrigger = trigger;
            IsResetTargetReady = isResetTargetReady;
            IsEndFrom = isEndFrom;
        }

        public ScheduleLine(Skill from, Skill to, Trigger trigger, bool isResetTargetReady, bool isEndFrom)
        {
            this.from = from;
            this.to = to;
            this.trigger = trigger;
            AgentTrigger = GetFromTrigger;
            IsResetTargetReady = isResetTargetReady;
            IsEndFrom = isEndFrom;
        }
            private bool GetFromTrigger()=>trigger.IsTriggered();

        public void Update()
    {
        if (AgentTrigger.Invoke())
        {
            
            Debug.Log("ScheduleLine.Update trigger  ");
            if (IsResetTargetReady)
            {
                
                to.isReady = true;
            }
            if (IsEndFrom&&null!=from)
            {
                from.End();
            }
        
            if(to.isReady)
            {
                
                to.Activate();
            }
        
        }
    }
}
/// <summary>
/// 用于设置技能的准备状态为true
/// 默认是在技能结束时设置为true
/// </summary>
public class ReadyLine
{
    public Skill to;
    public Func<bool> AgentTrigger;//TODO:专门的trigger对象
    public Trigger trigger;

    public ReadyLine(Skill to,Skill from){
        this.to = to;
        trigger = new SkillEndTrigger(from);
        AgentTrigger = GetFromTrigger;
    }
    public ReadyLine(Skill to, Func<bool> trigger)
    {
        this.to = to;
        this.AgentTrigger = trigger;
    }

    public ReadyLine(Skill to, Trigger trigger)
    {
        this.to = to;
        this.trigger = trigger;
        AgentTrigger = GetFromTrigger;
    }
    private bool GetFromTrigger()=>trigger.IsTriggered();

    public void Update()
    {
        if (AgentTrigger.Invoke()&&null!=to)
        {

            to.isReady = true;
        }
    }
}

}