using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseProjectileSO", menuName = "ProjectileSO/BaseProjectileSO", order = 1)]
public class BaseProjectileSO : ProjectileSObase
{
    public float speedMultiplier;
    public float damage;
    public float LifeTime;
    public float hitRadius;
    public bool isPiercing;
    

    public override void SetBaseInfo(Projection proj)
    {
        config=new Projection.ProjectionConfig(){
            position=proj.sender.transform.position,
            lifeTime=LifeTime,
            radius=0.5f,
            isPierced=isPiercing,
            scale=Vector2.one*hitRadius/2,
        };
        base.SetBaseInfo(proj);
        proj.SpeedMultiplier=speedMultiplier;
    }

    public override void SetFilterInfo(Projection proj)
    {
        
    }

    public override void SetHitInfo(Projection proj)
    {
        
        proj.AddEffectCommand(
            DamageCommand.Get(damage,0,proj.sender)
        );
        proj.AddEffectCommand(
            EntityCommand.CreateCommand(
                (tcommand)=>
                {
                    tcommand.Target.entityRigidbody.AddForce(proj.ProjectionRigidbody.velocity.normalized*proj.SpeedMultiplier*2,ForceMode2D.Impulse);
                }
            )
        );
    }

    public override void SetLogicInfo(Projection proj)
    {
        proj.AddUpdateLogicSub(
            new BaseMoveProjectileLogicSub()
        );
    }
}