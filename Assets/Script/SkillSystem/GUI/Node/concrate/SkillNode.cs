
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
public class SkillNode : Node
{
    SkillSystem.Skill Instance;
    SkillEffectMultiInPort MultiInPort;
    string SkillName="new skill";
    bool isAutoReset=true;
    ScheduleLineMultiInPort scheduleLineMultiInPort;
    ReadyLineMultiInPort readyLineMultiInPort;
    public SkillSystem.Skill BuildSkill()
    {
        SkillSystem.SkillEffectBase[] effectBases =(MultiInPort.Build()).Select(x=>x.value).ToArray();
        Instance = new SkillSystem.Skill(SkillName,nodeEditorController.targetEntity,effectBases,isAutoReset);
        return Instance;
    }
    public void ConnectScheduleLine()
    {
        List<RefParameter<SkillSystem.ScheduleLine>> output = scheduleLineMultiInPort.Build();
        if(output==null)
            return;
        SkillSystem.ScheduleLine[] scheduleLines = (output).Select(x=>x.value).ToArray();
        foreach (var item in scheduleLines)
        {
            item.from = Instance;
        }
        Instance.scheduleLines.AddRange(scheduleLines);
        //ReadyLine
        List<RefParameter<SkillSystem.ReadyLine>> output2 = readyLineMultiInPort.Build();
        if(output2==null)
            return;
        SkillSystem.ReadyLine[] readyLines = (output2).Select(x=>x.value).ToArray();
        foreach (var item in readyLines)
        {
            if(item.trigger==null){
                item.trigger = new SkillSystem.SkillEndTrigger(Instance);
            }
        }
        Instance.readyLines.AddRange(readyLines);
    }
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        var outInstance = this.SkillOutputPort("Skill",() => {if(Instance==null) throw new System.Exception("Instance is null")
        ;return new RefParameter<SkillSystem.Skill>(Instance);}, body_input, out GameObject portOut);
        var namePort = this.GetStringInputField("SkillName",body_input, out GameObject NameInput);
        namePort.onValueChanged.AddListener((string value) => { SkillName = value; });

        var isAutoResetPort = this.GetToggle("isAutoReset",body_input, out GameObject isAutoResetToggle);
        isAutoResetPort.onValueChanged.AddListener((bool value) => { isAutoReset = value; });

        MultiInPort = this.SkillEffectMultiInputPort("effects",body_input, out GameObject port1);
        //
        
        scheduleLineMultiInPort = this.ScheduleLineMultiInputPort("Schedule",body_output, out GameObject port3);//注意是InputPort
        readyLineMultiInPort = this.ReadyLineMultiInputPort("Ready",body_output, out GameObject port4);//注意是InputPort
        deleteAction=() => {
            outInstance.Delete();
            MultiInPort.Delete();
            scheduleLineMultiInPort.Delete();
            readyLineMultiInPort.Delete();
            
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Skill";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
