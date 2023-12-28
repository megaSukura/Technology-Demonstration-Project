
using UnityEngine;
using TMPro;
using System.Linq;
public class AlwaysScheduleLineNode : Node
{
    SkillInPort to;
   TriggerInPort trigger;
  public bool IsResetTargetReady = false;
    public SkillSystem.Trigger GetTrigger()
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
    public SkillSystem.Skill GetSkill()
    {
        return to.Build().value;
    }
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        
        var Toggle = this.GetToggle("isResetTargetReady", body_input, out GameObject isResetTargetReadyToggle);
        Toggle.onValueChanged.AddListener((bool value) => { IsResetTargetReady = value; });
        Toggle.isOn = IsResetTargetReady;
        //
        to = this.SkillInputPort("to",null, body_output, out GameObject port2);

        trigger=this.TriggerInputPort("trigger",body_input, out GameObject port3);
        deleteAction=() => {
            to.Delete();
            trigger.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "AlwaysScheduleLiner";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
