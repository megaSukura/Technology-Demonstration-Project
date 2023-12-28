
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using SkillSystem;
public class ProjectileLogicSubNode : Node
{
    readonly string[] logicNames=new string[]{"BaseMoveProjectileLogicSub"};
    
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        var inputDropdown = this.GetDropdown(body_input, out GameObject port1);
        //var  = name_input.AddComponent<Dropdown>();
        inputDropdown.options = new List<Dropdown.OptionData>();
        foreach(var name in logicNames){
            inputDropdown.options.Add(new Dropdown.OptionData(name));
        }
        inputDropdown.value = 0;
        inputDropdown.captionText.text = logicNames[0];
        inputDropdown.onValueChanged.AddListener((value) => {
            inputDropdown.captionText.text = logicNames[value];
        });

        var _in_getter = this.Vector2GetterInputPort("in getter",body_input, out GameObject _);

        var _out =this.ProjectileLogicSubOutputPort("out logic",() => new BaseMoveProjectileLogicSub(
            new Parameter<Vector2>(Vector2.zero,_in_getter.Build())
        ), body_output, out GameObject port2);
        //
        deleteAction=() => {
            _out.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Projectile LogicSub Effect";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
