using UnityEngine;
using System;
namespace SkillSystem
{
    
    public interface ITrigger
{
    bool IsTriggered();
}
[Serializable]
public abstract class Trigger : Getter<bool>, ITrigger
{
    public bool IsTriggered(){
        return Get();
    }

    public static Trigger operator &(Trigger trigger1, Trigger trigger2)
        {
            return new AndTrigger(trigger1, trigger2);
        }
        public static Trigger operator |(Trigger trigger1, Trigger trigger2)
        {
            return new OrTrigger(trigger1, trigger2);
        }
        public static Trigger operator !(Trigger trigger)
        {
            return new NotTrigger(trigger);
        }
}



}
