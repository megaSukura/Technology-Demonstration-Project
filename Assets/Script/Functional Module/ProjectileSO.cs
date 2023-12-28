using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 本质是为了方便的工具类,用于设置Projectile的基础信息
/// 只负责设置初始信息(基础信息,碰撞信息,过滤信息,逻辑信息)
/// 逻辑信息的设置由发射者设置
/// </summary>
public abstract class ProjectileSObase : ScriptableObject
{
    [HideInInspector]
    public Projection.ProjectionConfig config;

    public virtual void SetBaseInfo(Projection proj){
        proj.SetProjectionConfig(config);
        proj.transform.position = proj.sender.transform.position;
    }
    public abstract void SetHitInfo(Projection proj);
    public abstract void SetFilterInfo(Projection proj);
    public abstract void SetLogicInfo(Projection proj);
}
