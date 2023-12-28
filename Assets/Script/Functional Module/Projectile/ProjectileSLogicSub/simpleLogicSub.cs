using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 追踪target的演示逻辑子程序
/// </summary>
public class simpleLogicSub : ProjectileLogicSub
{
    
    public GameEntity target;

    public simpleLogicSub(GameEntity target,int priority=0)
    {
        this.target = target;
        this.priority = priority;
    }
    public override ProjectileLogicSub Clone()
    {
        return new simpleLogicSub(target);
    }
    public override ProjectileLogicSub GetSnapshot()
    {
        return new simpleLogicSub(target);
    }

    public override bool Execute()
    {
        if(!target.gameObject||target==null)
            return true;
        var dir = target.transform.position - proj.transform.position;
        proj.ProjectionRigidbody.velocity=dir.normalized*proj.DesireVelocity.magnitude;
        return true;
    }
}
