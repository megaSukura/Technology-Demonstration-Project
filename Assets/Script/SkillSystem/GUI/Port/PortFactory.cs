using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public static class PortFactory 
{
    public static GameObject PortPrefab;
    
    //return port component with gameobject
    public static floatInPort FloatInputPort(this NodeBase node,String portName,float defaultValue, GameObject parent, out GameObject port)
    {
        //Debug.Log(PortPrefab);
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        //Debug.Log(port);
        floatInPort portComponent = port.AddComponent<floatInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        //Debug.Log(portComponent);
        portComponent.defaultValue = defaultValue;
        portComponent.Owner = node;
        return portComponent;
    }
    public static floatOutPort FloatOutputPort(this NodeBase node,String portName,Func<Parameter<float>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        floatOutPort portComponent = port.AddComponent<floatOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    public static Vector2InPort Vector2InputPort(this NodeBase node,String portName,Vector2 defaultValue, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        Vector2InPort portComponent = port.AddComponent<Vector2InPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color.red;
        portComponent.defaultValue = defaultValue;
        portComponent.Owner = node;
        return portComponent;
    }
    public static Vector2OutPort Vector2OutputPort(this NodeBase node,String portName,Func<Parameter<Vector2>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        Vector2OutPort portComponent = port.AddComponent<Vector2OutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.red;
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = parent.GetComponent<NodeBase>();
        return portComponent;
    }
    public static EntityInPort EntityInputPort(this NodeBase node,String portName,GameEntity defaultValue, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        EntityInPort portComponent = port.AddComponent<EntityInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponent<Image>().color = Color.green;
        portComponent.defaultValue = defaultValue;
        portComponent.Owner = node;
        return portComponent;
    }
    public static EntityOutPort EntityOutputPort(this NodeBase node,String portName,Func<RefParameter<GameEntity>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        EntityOutPort portComponent = port.AddComponent<EntityOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.green;
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    public static SkillEffectMultiInPort SkillEffectMultiInputPort(this NodeBase node,String portName, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        SkillEffectMultiInPort portComponent = port.AddComponent<SkillEffectMultiInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color.blue;
        portComponent.defaultValue = null;
        portComponent.Owner = node;
        return portComponent;
    }
    public static SkillOutPort SkillOutputPort(this NodeBase node,String portName,Func<RefParameter<SkillSystem.Skill>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        SkillOutPort portComponent = port.AddComponent<SkillOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color.yellow;
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    public static SkillInPort SkillInputPort(this NodeBase node,String portName, SkillSystem.Skill defaultValue, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        SkillInPort portComponent = port.AddComponent<SkillInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.yellow;
        portComponent.defaultValue = defaultValue;
        portComponent.Owner = node;
        return portComponent;
    }
    public static ScheduleLineMultiInPort ScheduleLineMultiInputPort(this NodeBase node,String portName, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        ScheduleLineMultiInPort portComponent = port.AddComponent<ScheduleLineMultiInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.cyan;
        portComponent.defaultValue = null;
        portComponent.Owner = node;
        return portComponent;
    }
    public static ScheduleLineOutPort ScheduleLineOutputPort(this NodeBase node,String portName,Func<RefParameter<SkillSystem.ScheduleLine>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        ScheduleLineOutPort portComponent = port.AddComponent<ScheduleLineOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color.cyan;
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    public static ReadyLineMultiInPort ReadyLineMultiInputPort(this NodeBase node,String portName, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        ReadyLineMultiInPort portComponent = port.AddComponent<ReadyLineMultiInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.Lerp(Color.cyan,Color.white,0.8f);
        portComponent.defaultValue = null;
        portComponent.Owner = node;
        return portComponent;
    }
    public static ReadyLineOutPort ReadyLineOutputPort(this NodeBase node,String portName,Func<RefParameter<SkillSystem.ReadyLine>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        ReadyLineOutPort portComponent = port.AddComponent<ReadyLineOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color.Lerp(Color.cyan,Color.white,0.8f);
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    //getter port
    public static Vector2GetterInPort Vector2GetterInputPort(this NodeBase node,String portName, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        Vector2GetterInPort portComponent = port.AddComponent<Vector2GetterInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color .Lerp(Color.red,Color.black,0.5f);
        portComponent.defaultValue = null;
        portComponent.Owner = node;
        return portComponent;
    }
    public static Vector2GetterOutPort Vector2GetterOutputPort(this NodeBase node,String portName,Func<RefParameter<Getter<Vector2>>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        Vector2GetterOutPort portComponent = port.AddComponent<Vector2GetterOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color .Lerp(Color.red,Color.black,0.5f);
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    public static FloatGetterInPort FloatGetterInputPort(this NodeBase node,String portName, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        FloatGetterInPort portComponent = port.AddComponent<FloatGetterInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color .Lerp(Color.white,Color.black,0.5f);
        portComponent.defaultValue = null;
        portComponent.Owner = node;
        return portComponent;
    }
    public static FloatGetterOutPort FloatGetterOutputPort(this NodeBase node,String portName,Func<RefParameter<Getter<float>>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        FloatGetterOutPort portComponent = port.AddComponent<FloatGetterOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color .Lerp(Color.white,Color.black,0.5f);

        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    //Trigger
    public static TriggerInPort TriggerInputPort(this NodeBase node,String portName, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        TriggerInPort portComponent = port.AddComponent<TriggerInPort>();
        port.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.black, 0.5f);
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        portComponent.defaultValue = null;
        portComponent.Owner = node;
        return portComponent;
    }
    public static TriggerOutPort TriggerOutputPort(this NodeBase node,String portName,Func<RefParameter<SkillSystem.Trigger>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        TriggerOutPort portComponent = port.AddComponent<TriggerOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.black, 0.5f);
        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    public static SkillEffectOutPort SkillEffectOutputPort(this NodeBase node, string portName,Func<RefParameter<SkillSystem.SkillEffectBase>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        SkillEffectOutPort portComponent = port.AddComponent<SkillEffectOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.blue;

        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    //ProjectileLogicSub
    public static ProjectileLogicSubOutPort ProjectileLogicSubOutputPort(this NodeBase node, string portName,Func<RefParameter<ProjectileLogicSub>> parameterGetter, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        ProjectileLogicSubOutPort portComponent = port.AddComponent<ProjectileLogicSubOutPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        port.GetComponent<Image>().color = Color.magenta;

        portComponent.ParameterGetter = parameterGetter;
        portComponent.Owner = node;
        return portComponent;
    }
    public static ProjectileLogicSubMultiInPort ProjectileLogicSubMultiInputPort(this NodeBase node, string portName, GameObject parent, out GameObject port)
    {
        port = GameObject.Instantiate(PortPrefab, parent.transform);
        ProjectileLogicSubMultiInPort portComponent = port.AddComponent<ProjectileLogicSubMultiInPort>();
        port.GetComponentInChildren<TextMeshProUGUI>().text = portName;
        port.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        port.GetComponent<Image>().color = Color.magenta;

        portComponent.defaultValue = null;
        portComponent.Owner = node;
        return portComponent;
    }


    //Other UGUI 
    public static GameObject DropDownPrefab;
    public static Dropdown GetDropdown(this NodeBase node,GameObject parent, out GameObject dropdown)
    {
        dropdown = GameObject.Instantiate(DropDownPrefab, parent.transform);
        return dropdown.GetComponent<Dropdown>();
    }
    public static GameObject InputFieldPrefab;
    public static InputField GetInputField(this NodeBase node,GameObject parent, out GameObject inputField)
    {
        inputField = GameObject.Instantiate(InputFieldPrefab, parent.transform);
        return inputField.GetComponent<InputField>();
    }
    public static GameObject TogglePrefab;
    public static Toggle GetToggle(this NodeBase node,String toggleName,GameObject parent, out GameObject toggle)
    {
        toggle = GameObject.Instantiate(TogglePrefab, parent.transform);
        toggle.transform.Find("Label").GetComponent<Text>().text = toggleName;

        return toggle.GetComponent<Toggle>();
    }
    public static GameObject StringInputFieldPrefab;
    public static InputField GetStringInputField(this NodeBase node,String fieldName,GameObject parent, out GameObject inputField)
    {
        inputField = GameObject.Instantiate(StringInputFieldPrefab, parent.transform);
        inputField.transform.Find("Placeholder").GetComponent<Text>().text = fieldName;
        return inputField.GetComponent<InputField>();
    }

  
}