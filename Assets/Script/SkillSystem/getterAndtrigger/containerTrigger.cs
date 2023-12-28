using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public class AndTrigger : Trigger
    {
        public List<Trigger> triggers = new List<Trigger>();
        public AndTrigger(List<Trigger> triggers)
        {
            this.triggers = triggers;
        }
        public AndTrigger(Trigger[] triggers)
        {
            this.triggers.AddRange(triggers);
        }
        public AndTrigger(Trigger trigger1, Trigger trigger2)
        {
            this.triggers.Add(trigger1);
            this.triggers.Add(trigger2);
        }

        public override bool Get()
        {
            foreach (Trigger trigger in triggers)
            {
                if (!trigger.IsTriggered())
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class OrTrigger : Trigger
    {
        public List<Trigger> triggers = new List<Trigger>();
        public OrTrigger(List<Trigger> triggers)
        {
            this.triggers = triggers;
        }
        public OrTrigger(Trigger[] triggers)
        {
            this.triggers.AddRange(triggers);
        }
        public OrTrigger(Trigger trigger1, Trigger trigger2)
        {
            this.triggers.Add(trigger1);
            this.triggers.Add(trigger2);
        }
        public override bool Get()
        {
            foreach (Trigger trigger in triggers)
            {
                if (trigger.IsTriggered())
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class NotTrigger : Trigger
    {
        public Trigger trigger;
        public NotTrigger(Trigger trigger)
        {
            this.trigger = trigger;
        }
        public override bool Get()
        {
            return !trigger.IsTriggered();
        }
    }


}