
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class ProgressTriggerNode : Node
{
    float maxProgress = 1;
    float minProgress = 0;
    SkillInPort skillInPort;
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
         skillInPort = this.SkillInputPort("Skill",null, body_input, out GameObject portout);
        var minProgressInput = this.GetInputField(body_input, out GameObject port2);
        minProgressInput.onValueChanged.AddListener((value) => {
            if(value!=null&&value!="")
            minProgress = float.Parse(value);
        });
        minProgressInput.transform.Find("Placeholder").GetComponent<Text>().text ="minProgress";
        var maxProgressInput = this.GetInputField(body_input, out GameObject port1);
        maxProgressInput.onValueChanged.AddListener((value) => {
            if(value!=null&&value!="")
            maxProgress = float.Parse(value);
        });
        maxProgressInput.transform.Find("Placeholder").GetComponent<Text>().text ="maxProgress";
        
        var out_=this.TriggerOutputPort("out trigger",() => new SkillSystem.SkillProgressTrigger(skillInPort.Build(),minProgress,maxProgress), body_output, out GameObject outport);
        //
        deleteAction=() => {
            skillInPort.Delete();
            out_.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Progress Trigger";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
