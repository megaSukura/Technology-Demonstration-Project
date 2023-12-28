
using UnityEngine;
using TMPro;
public class Vector2ParameterNode : Node
{
    Vector2 defaultValue;
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        defaultValue = Vector2.zero;
        var defalutValueInputx = this.GetInputField(body_input, out GameObject input1);
        defalutValueInputx.onValueChanged.AddListener((value) => {
            defaultValue.x = float.Parse(value);
        });
        var defalutValueInputy = this.GetInputField(body_input, out GameObject input2);
        defalutValueInputy.onValueChanged.AddListener((value) => {
            defaultValue.y = float.Parse(value);
        });
        var in_getter = this.Vector2GetterInputPort("in getter",body_input, out GameObject port1);
        var _out=this.Vector2OutputPort("Vector2",() => new Parameter<Vector2>(defaultValue,in_getter.Build()), body_output, out GameObject port2);
        //
        deleteAction = () => {
            _out.Delete();
            in_getter.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Vector2 Parameter";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
