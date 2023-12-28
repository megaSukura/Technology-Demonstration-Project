using System.Collections.Generic;
using UnityEngine;
using System;
namespace SkillSystem
{
public class Skill 
{
    public string SkillName ;
   public bool isActivated {get{return _isActivated;}set{_isActivated = value;}}
   private bool _isActivated = false;
   public bool isReady {get{return _isReady;}set{_isReady = value;/*Debug.Log("set "+SkillName+" "+_isReady);*/}}
   private bool _isReady = true;
   public bool isAutoReset = true;
   public float Progress = 0;
   /// <summary>
    /// 只有当进度为1时才会为true,如果被中途打断则会为false
    /// </summary>
    public bool IsEnd{get{return  Progress>=1 ;}}
   //
   public GameEntity entity;
   public SkillManager manager;
   public List<ScheduleLine> scheduleLines = new List<ScheduleLine>();
   public List<ReadyLine> readyLines = new List<ReadyLine>();
    public List<SkillEffectBase> effects = new List<SkillEffectBase>();
    //
    public Skill(string skillname,GameEntity user,bool isAutoReset=false)
    {
        SkillName = skillname;
        entity = user;
        this.isAutoReset = isAutoReset;
    }
    public Skill(string skillname,GameEntity user,SkillEffectBase[] effects ,bool isAutoReset=false)
    {
        SkillName = skillname;
        entity = user;
        this.isAutoReset = isAutoReset;
        this.effects.AddRange(effects);
    }

    // 激活技能
    public void Activate()
    {   
        if (isActivated)
        {Debug.Log("had");;return;}
        Debug.Log("Activate");
        if (isReady)
        {   Debug.Log("isReady");
            isActivated = true;
            foreach (SkillEffectBase effect in effects)
            {
                effect.Apply(this);
            }
            //
            isReady = false;
        }
    }
    // 
    public void Update()
    {
        if (isActivated)
        {
            foreach (ReadyLine line in readyLines)
            {
                line.Update();
            }
            if(Progress>=1)
            {   
                End();
                return;
            }
            //注意这里的顺序
            foreach (SkillEffectBase effect in effects)
            {
                SetProgress(effect.Update());
            }
            
            
            foreach (ScheduleLine line in scheduleLines)
            {
                line.Update();
            }
            
        }
    }
    //
    public void End(){
        isActivated = false;
        Progress = 0;
        foreach (SkillEffectBase effect in effects)
        {
            effect.Stop();
        }
        if (isAutoReset)
        {
            isReady = true;
        }
        
    }
    //
    public void SetProgress(float progress)
    {
        if(progress>Progress)
        {
            Progress = progress;
            if (Progress >= 1)
            {
                Progress = 1;
            }
        }
    }
    //
    public void AddEffect(SkillEffectBase effect)
    {
        effects.Add(effect);
    }
    //
    public ScheduleLine ConnectTo(Skill to, Func<bool> trigger, bool isResetTargetReady=true, bool isEndFrom=false)
    {
        ScheduleLine line = new ScheduleLine(this, to, trigger, isResetTargetReady, isEndFrom);
        scheduleLines.Add(line);
        return line;
    }
    public ScheduleLine ConnectTo(Skill to, Trigger trigger, bool isResetTargetReady=true, bool isEndFrom=false)
    {
        ScheduleLine line = new ScheduleLine(this, to, trigger, isResetTargetReady, isEndFrom);
        scheduleLines.Add(line);
        return line;
    }

    public  ScheduleLine ConnectFromAlways(  SkillManager manager , Func<bool> trigger, bool isResetTargetReady = false)
        {
            ScheduleLine line = new ScheduleLine(null, this, trigger, isResetTargetReady, false);
            manager.AlwaysSceduleLines.Add(line);
            return line;
        }
    public  ScheduleLine ConnectFromAlways(  SkillManager manager , Trigger trigger, bool isResetTargetReady = false)
        {
            ScheduleLine line = new ScheduleLine(null, this, trigger, isResetTargetReady, false);
            manager.AlwaysSceduleLines.Add(line);
            return line;
        }
    //
    public ReadyLine ReadyTargetWhen(Skill target, Func<bool> trigger)
    {
        ReadyLine line = new ReadyLine(target, trigger);
        readyLines.Add(line);
        return line;
    }
    public ReadyLine ReadyTargetWhen(Skill target, Trigger trigger)
    {
        ReadyLine line = new ReadyLine(target, trigger);
        readyLines.Add(line);
        return line;
    }
    public ReadyLine ReadyTargetWhenEnd(Skill target)
    {
        ReadyLine line = new ReadyLine(target,this);
        readyLines.Add(line);
        return line;
    }

}
   
}