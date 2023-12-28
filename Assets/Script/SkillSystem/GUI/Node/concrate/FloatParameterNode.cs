
using UnityEngine;
using TMPro;
public class FloatParameterNode : Node
{
    float defaultValue;
    Parameter<float> parameterInstance;
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        var defalutValueInput = this.GetInputField(body_input, out GameObject input1);
        defalutValueInput.onValueChanged.AddListener((value) => {
            if(value!=null&&value!="")
            defaultValue = float.Parse(value);
        });
        var in_getter = this.FloatGetterInputPort("in Getter",body_input, out GameObject port1);
        deleteAction = () => {
            parameterInstance = null;
            in_getter.Delete();
        };
        var out_= this.FloatOutputPort("float Parameter",() => {if(parameterInstance==null)
                                    {
                                        parameterInstance=new Parameter<float>(defaultValue);
                                        parameterInstance.SetGetter(in_getter.Build());
                                    }
                                     return parameterInstance;
                                    }, body_output, out GameObject port2);
        //
        deleteAction=() => {
            in_getter.Delete();
            out_.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "float Parameter";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
