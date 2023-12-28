
using UnityEngine;
using TMPro;
using SkillSystem;
using System.Linq;
public class ShootSkillEffectNode : Node
{
    
    public override void SetBody(GameObject body_input, GameObject body_output)
    {
        var duration = this.FloatInputPort("duration",1, body_input, out GameObject port1);
        var posOffset = this.Vector2InputPort("posOffset",Vector2.zero, body_input, out GameObject port2);
        var projSpeed = this.FloatInputPort("projSpeed",10, body_input, out GameObject port3);
        var projVDir = this.Vector2InputPort("projVDir",Vector2.left, body_input, out GameObject port4);
        var projCount= this.FloatInputPort("projCount",1, body_input, out GameObject port5);
        var projAngle= this.FloatInputPort("projAngle",30, body_input, out GameObject port6);
        var projDelay= this.FloatInputPort("projDelay",0, body_input, out GameObject port7);
        var projLogicSubs = this.ProjectileLogicSubMultiInputPort("projLogicSubs",body_input, out GameObject port8);
        // ProjectileLogicSub
        var _out=this.SkillEffectOutputPort("Effect out",() => new RefParameter<SkillEffectBase>(
            new SkillSystem.SkillEfect.shootSkillEffect(
                duration.Build(),
                GameSystem.StartProjectileSO_static,
                posOffset.Build(),
                projSpeed.Build(),
                projVDir.Build(),
                new Parameter<int>((int)(projCount.Build().value)),
                projAngle.Build(),
                projDelay.Build(),
                projLogicSubs.Build().Select(x=>x.value).ToArray()
                // new ProjectileLogicSub[] { }
            )
        ), body_output, out GameObject portOut);
        //
        deleteAction = () => {
            duration.Delete();
            posOffset.Delete();
            projSpeed.Delete();
            projVDir.Delete();
            projCount.Delete();
            projAngle.Delete();
            projDelay.Delete();
            _out.Delete();
        };
    }

    public override void SetHeader(GameObject header)
    {
        var textComponent= header.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Shoot Skill Effect";
        textComponent.fontSize = 24;
        textComponent.alignment = TextAlignmentOptions.Center;
    }
}
