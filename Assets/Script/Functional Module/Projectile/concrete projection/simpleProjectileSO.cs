using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 追踪target的演示projectile ScriptableObject
/// 错误演示:逻辑信息的设置由发射者设置
/// </summary>
[CreateAssetMenu(fileName = "simpleProjectileSO", menuName = "ProjectileSO/simpleProjectileSO", order = 1)]
public class simpleProjectileSO : ProjectileSObase
{   
    public float Radius;
    public float LifeTime;
    public float StartSpeed;
    [HideInInspector]
    public Vector2 VelocityDir;
    [HideInInspector]
    public GameEntity target;
    public override void SetBaseInfo(Projection proj)
    {
        config = new Projection.ProjectionConfig(
            position: proj.sender.transform.position,
            scale: Vector3.one*0.2f,
            velocity: VelocityDir.normalized*StartSpeed,
            lifeTime: LifeTime,
            radius: Radius,
            _isPierced: true,
            sender: null    //sender is null, 所以不会替换
        );
        proj.SetProjectionConfig(config);
    }
    public override void SetFilterInfo(Projection proj)
    {
        
    }

    public override void SetHitInfo(Projection proj)
    {
        proj.AddEffectCommands(new EntityCommand[]{
            EntityCommand.CreateCommand((_)=>{Debug.Log("hit");},priority:0,sender:proj.sender)
        });
    }

    public override void SetLogicInfo(Projection proj)
    {
        if(target==null)
            return;
        proj.AddUpdateLogicSub(new simpleLogicSub(target));
    }
}