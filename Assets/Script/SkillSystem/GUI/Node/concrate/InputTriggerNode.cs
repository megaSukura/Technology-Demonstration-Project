
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class InputTriggerNode : Node
{
    string[] KeyNames=new string[]{"Fire1","Fire2","Fire3","Jump"};
    string selectedKeyName;
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        var inputDropdown = this.GetDropdown(body_input, out GameObject port1);
        //var  = name_input.AddComponent<Dropdown>();
        inputDropdown.options = new List<Dropdown.OptionData>();
        foreach(var name in KeyNames){
            inputDropdown.options.Add(new Dropdown.OptionData(name));
        }
        inputDropdown.value = 0;
        inputDropdown.captionText.text = KeyNames[0];
        inputDropdown.onValueChanged.AddListener((value) => {
            inputDropdown.captionText.text = KeyNames[value];
            selectedKeyName = KeyNames[value];
        });
        if(KeyNames.Length>0){
            selectedKeyName=KeyNames[0];
        }
        var _out=this.TriggerOutputPort("out trigger",() => new SkillSystem.InputTrigger(selectedKeyName), body_output, out GameObject port2);
        //
        deleteAction=() => {
            _out.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Input Trigger";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
