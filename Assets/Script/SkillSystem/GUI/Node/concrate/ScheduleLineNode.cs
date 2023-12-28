
using UnityEngine;
using TMPro;
using System.Linq;
public class ScheduleLineNode : Node
{
   SkillInPort to;
   TriggerInPort trigger;
   bool IsResetTargetReady = false;
    bool IsEndFrom = false;
    SkillSystem.Trigger GetTrigger()
    {
        var triggers = trigger?.Build().Select(x => x .value).ToArray();
        if(triggers==null)
        {
            return null;
        }
        if (triggers.Length == 0)
        {
            return null;
        }
        else if (triggers.Length == 1)
        {
            return triggers[0];
        }
        else
        {
            return new SkillSystem.AndTrigger(triggers);
        }

    }
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        var from_ = this.ScheduleLineOutputPort("From",() => {
             return new RefParameter<SkillSystem.ScheduleLine>(new SkillSystem.ScheduleLine(
                null,
                to.Build(),
                GetTrigger(),
                IsResetTargetReady,
                IsEndFrom
             )); 
             }, body_input, out GameObject port1);
        var Toggle = this.GetToggle("isResetTargetReady", body_input, out GameObject isResetTargetReadyToggle);
        Toggle.onValueChanged.AddListener((bool value) => { IsResetTargetReady = value; });
        var Toggle2 = this.GetToggle("IsEndFrom", body_input, out GameObject IsEndFromToggle);
        Toggle2.onValueChanged.AddListener((bool value) => { IsEndFrom = value; });

        //
        to = this.SkillInputPort("To",null, body_output, out GameObject port2);

        trigger=this.TriggerInputPort("trigger",body_input, out GameObject port3);
        deleteAction=() => {
            from_ .Delete();
            to.Delete();
            trigger.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "ScheduleLiner";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
