
using UnityEngine;
using TMPro;
using System.Linq;
public class ReadyLineNode : Node
{
   SkillInPort to;
   TriggerInPort trigger;
   
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
        

        var _from = this.ReadyLineOutputPort("From",() => {
             return new RefParameter<SkillSystem.ReadyLine>(new SkillSystem.ReadyLine(
                to.Build(),
                GetTrigger()
             )); 
             }, body_input, out GameObject port1);

        //
        to = this.SkillInputPort("To",null, body_output, out GameObject port2);

        trigger=this.TriggerInputPort("trigger",body_input, out GameObject port3);
        deleteAction=() => {
            _from.Delete();
            to.Delete();
            trigger.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "ReadyLiner";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
