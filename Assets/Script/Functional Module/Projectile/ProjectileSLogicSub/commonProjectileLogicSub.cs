using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseMoveProjectileLogicSub : ProjectileLogicSub
{
    public Parameter<Vector2> DesireVelocity;

    public BaseMoveProjectileLogicSub(Parameter<Vector2> desireVelocity)
    {
        DesireVelocity = desireVelocity;
    }

    public BaseMoveProjectileLogicSub()
    {
    }

    public override bool Execute()
    {   if(DesireVelocity!=null)
        
        proj.DesireVelocity = DesireVelocity.value*proj.ProjectionRigidbody.velocity.magnitude;
        
        proj.ProjectionRigidbody.velocity = proj.DesireVelocity;
        return true;
    }
    public override ProjectileLogicSub Clone()
    {
        return new BaseMoveProjectileLogicSub(DesireVelocity);
    }
    public override ProjectileLogicSub GetSnapshot()
    {
        return new BaseMoveProjectileLogicSub(DesireVelocity.value);
    }
}

//有最大转向角的追踪LogicSub
public class TrackingLogicSub : ProjectileLogicSub
{
    public RefParameter<GameEntity> target;
    public float maxTurnAngle;
    public TrackingLogicSub(RefParameter<GameEntity> target, float maxTurnAngle, int priority = 1)
    {
        this.target = target;
        this.maxTurnAngle = maxTurnAngle;
        this.priority = priority;
    }
    public override ProjectileLogicSub Clone()
    {
        return new TrackingLogicSub(target, maxTurnAngle);//ReferenceParameter
    }
    public override ProjectileLogicSub GetSnapshot()
    {
        
        return new TrackingLogicSub(target.value, maxTurnAngle);
    }

    public override bool Execute()
    {   
        if ( target == null||null==target.value||!target.value.gameObject)
            return true;
        Vector2 dir = target.value.transform.position - proj.transform.position;
        var desireVelocity = dir.normalized * proj.DesireVelocity.magnitude;
        var currentVelocity = proj.ProjectionRigidbody.velocity;
        var angle = Vector2.Angle(currentVelocity, desireVelocity);
        if (angle > maxTurnAngle)
        {
            var axis = Vector3.Cross(currentVelocity, desireVelocity);
            var sign = axis.z > 0 ? 1 : -1;
            //Debug.Log("sign:" + sign);
            var newvelo = Quaternion.AngleAxis(sign * maxTurnAngle*Time.deltaTime*10, Vector3.forward) * currentVelocity;
            proj.DesireVelocity = newvelo;
        }
        else
        {
            proj.DesireVelocity = desireVelocity;
        }

        return true;
    }
    public override string ToString()
    {
        return "target:" + target.value + " maxTurnAngle:" + maxTurnAngle;
    }
}