
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class EntityVector2GetterParameterNode : Node
{
    string[] attributeNames=new string[]{"RigPosition","VisualPosition","Velocity","DirectionToMouse"};
    string selectedAttributeName;
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        var inputDropdown = this.GetDropdown(body_input, out GameObject port1);
        //var  = name_input.AddComponent<Dropdown>();
        inputDropdown.options = new List<Dropdown.OptionData>();
        foreach(var name in attributeNames){
            inputDropdown.options.Add(new Dropdown.OptionData(name));
        }
        inputDropdown.value = 0;
        inputDropdown.captionText.text = attributeNames[0];
        inputDropdown.onValueChanged.AddListener((value) => {
            inputDropdown.captionText.text = attributeNames[value];
            selectedAttributeName = attributeNames[value];
        });


        
        var _out =this.Vector2GetterOutputPort("out getter",() => new EntityVector2Getter(nodeEditorController.targetEntity,selectedAttributeName??attributeNames[0]), body_output, out GameObject port2);
        //
        deleteAction=() => {
            _out.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Entity Vector2 Getter";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
