using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ShootAbility : AbilityBase
{
    public ShootAbility(GameEntity owner) : base(owner, "ShootAbility")
    {
    }

    public override void StartAbility()
    {
        base.StartAbility();
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();
    }

    public override void EndAbility()
    {
        base.EndAbility();
    }
    
    //~~~~~~~~~shootAbility specific~~~~~~~~~~
    public Projection ShootProjectile(ProjectileSObase projectileSO)
    {
        var proj = Projection.GetProjection(owner,projectileSO);
        return proj;

    }
    public Projection[] ShootProjectile(ProjectileSObase projectileSO,Vector2 startPos,Vector2 shootVelocity,int shootNum=1,float shootAngle=0,params ProjectileLogicSub[] addedProjectileLogicSubs)
    {
        
        // 为了避免除以零错误，首先检查 shootNum 是否大于 0
        if (shootNum <= 0) {
            Debug.LogError("射击数量必须大于零！");
            return null ;
        }
        if (shootNum == 1) {
        // 创建第一个弹射物
        Projection projectile = Projection.GetProjection(owner,projectileSO,startPos,shootVelocity);
        if(addedProjectileLogicSubs!=null) projectile.AddUpdateLogicSub(addedProjectileLogicSubs);
        // 如果只需要一个弹射物，直接返回
            return new Projection[]{projectile} ;
        }

        // 计算每个弹射物之间的间隔角度
        float intervalAngle = shootAngle / (shootNum - 1);

        // 计算第一个弹射物的旋转角度
        float startAngle = -shootAngle / 2f;
        // Projection temp_proj;
        var projs = new Projection[shootNum];
        // 依次创建其他弹射物，并根据间隔角度和起始角度计算它们的旋转角度
        for (int i = 0; i < shootNum; i++) {
            float angle = startAngle + i * intervalAngle;
            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * shootVelocity;
             projs[i]= Projection.GetProjection(owner,projectileSO, startPos, dir);
            if(addedProjectileLogicSubs!=null) projs[i].AddUpdateLogicSub(addedProjectileLogicSubs);
        }

        return projs ;

    }
    public Projection Shoot(ProjectileSObase projectileSO){
        return ShootProjectile(projectileSO);
    }
    
}