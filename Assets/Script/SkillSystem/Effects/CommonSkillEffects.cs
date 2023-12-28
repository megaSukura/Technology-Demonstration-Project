using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem.SkillEfect
{
    public class DebugSkillEffect : SkillEffectBase
    {
        float progress;
        Timer timer;
        public string DebugInfo;

        public DebugSkillEffect(string debugInfo)
        {
            DebugInfo = debugInfo;
        }

        public override void Apply(Skill user)
        {
            base.Apply(user);
            Debug.Log("DebugSkillEffect Apply: " + DebugInfo);
            timer=Timer.SetTimer(1).OnUpdate((_, ti) => { progress = ti; }).OnEnd(() => { progress = 1; });
        }

        public override float Update()
        {
            Debug.Log("DebugSkillEffect Update: " + DebugInfo);
            return progress;
        }

        public override void Stop()
        {   
            timer.Stop();
            progress = 0;
            Debug.Log("DebugSkillEffect Stop: " + DebugInfo);
        }
    }
    public class CustomSkillEffect : SkillEffectBase
    {
        float duration;
        float progress;
        Timer timer;
        Action<Skill> apply;
        Func<float> update;
        Action stop;
        public CustomSkillEffect(float duration, Action<Skill> apply, Func<float> update, Action stop)
        {
            this.duration = duration;
            this.apply = apply;
            this.update = update;
            this.stop = stop;
        }

        public override void Apply(Skill user)
        {
            base.Apply(user);
            apply(user);
            timer=Timer.SetTimer(duration).OnUpdate((_, ti) => { progress = ti; }).OnEnd(() => { progress = 1; });
            timer.BindTo(user.entity.gameObject);
        }

        public override float Update()
        {
            if (update!=null)
            return update();
            return progress;
        }

        public override void Stop()
        {
            timer.Stop();
            stop?.Invoke();
            progress = 0;
        }
    }
    public class ParameterDefaultChangeEffect<T>:SkillEffectBase where T:struct
    {
        Parameter<T> parameter;
        Parameter<T> targetValue;
        Parameter<T> endtTargetValue;
        Func<Skill,T> ApplyGetter;
        Func<Skill,T> EndGetter;
        
        public ParameterDefaultChangeEffect(Parameter<T> parameter,Parameter<T> targetValue, Parameter<T> endtTargetValue)
        {
            this.parameter = parameter;
            this.targetValue = targetValue;
            this.endtTargetValue = endtTargetValue;
        }
        public ParameterDefaultChangeEffect(Parameter<T> parameter,Func<Skill,T> ApplyGetter,Func<Skill,T> EndGetter)
        {
            this.parameter = parameter;
            this.ApplyGetter = ApplyGetter;
            this.EndGetter = EndGetter;
        }
        public override void Apply(Skill user)
        {
            base.Apply(user);
            if(targetValue!=null)
            parameter.defaultValue = targetValue.value;
            else if(ApplyGetter!=null)
            parameter.defaultValue = ApplyGetter(user);
        }

        public override float Update()
        {
            return 0;
        }

        public override void Stop()
        {
            if(endtTargetValue!=null)
            parameter.defaultValue = endtTargetValue.value;
            else if(EndGetter!=null)
            parameter.defaultValue = EndGetter(user);
        }
    }
    public class RefParameterDefaultChangeEffect<T>:SkillEffectBase where T:class
    {
        RefParameter<T> parameter;
        RefParameter<T> targetValue;
        RefParameter<T> endtTargetValue;
        Func<Skill,T> ApplyGetter;
        Func<Skill,T> EndGetter;
        
        public RefParameterDefaultChangeEffect(RefParameter<T> parameter,RefParameter<T> targetValue, RefParameter<T> endtTargetValue)
        {
            this.parameter = parameter;
            this.targetValue = targetValue;
            this.endtTargetValue = endtTargetValue;
        }
        public RefParameterDefaultChangeEffect(RefParameter<T> parameter,Func<Skill,T> ApplyGetter,Func<Skill,T> EndGetter)
        {
            this.parameter = parameter;
            this.ApplyGetter = ApplyGetter;
            this.EndGetter = EndGetter;
        }
        public override void Apply(Skill user)
        {
            base.Apply(user);
            if(targetValue!=null)
            parameter.defaultValue = targetValue.value;
            else if(ApplyGetter!=null)
            parameter.defaultValue = ApplyGetter(user);
        }

        public override float Update()
        {
            return 0;
        }

        public override void Stop()
        {
            if(endtTargetValue!=null)
            parameter.defaultValue = endtTargetValue.value;
            else if(EndGetter!=null)
            parameter.defaultValue = EndGetter(user);
        }
    }
    public class shootSkillEffect : SkillEffectBase
    {
        ProjectileSObase projectileSO;
        Parameter<Vector2> posOffset;
        Parameter<float> projectileSpeed;
        Parameter<Vector2> projectileDirection;
        Parameter<int> projectileCount;
        Parameter<float> projectileAngle;
        Parameter<float> DelayPerProjectile;
        ProjectileLogicSub[] addedProjectileLogicSubs;
        //
        float duration;
        float progress;
        List<Projection> ShootedProjections=new List<Projection>();
        Timer timer;
        public shootSkillEffect(float duration,ProjectileSObase projectileSO, Parameter<Vector2> posOffset, Parameter<float> projectileSpeed, Parameter<Vector2> projectileDirection, Parameter<int> projectileCount, Parameter<float> projectileAngle, Parameter<float> delayPerProjectile, params ProjectileLogicSub[] addedProjectileLogicSubs)
        {
            this.duration = duration;
            this.projectileSO = projectileSO;
            this.posOffset = posOffset;
            this.projectileSpeed = projectileSpeed;
            this.projectileDirection = projectileDirection;
            this.projectileCount = projectileCount;
            this.projectileAngle = projectileAngle;
            this.DelayPerProjectile = delayPerProjectile;
            this.addedProjectileLogicSubs = addedProjectileLogicSubs;
        }
        

        public override void Apply(Skill user)
        {
            base.Apply(user);
            ShootedProjections.Clear();
            var shootAbility = user.entity.FindAbility<ShootAbility>();
             ProjectileLogicSub[] const_subs = addedProjectileLogicSubs.GetSnapshotAll();//防止在运行时修改,快照机制
            if (DelayPerProjectile.value==0f)
            {
                var ts=shootAbility.ShootProjectile(projectileSO, (Vector2)user.entity.entityCollider.bounds.center+posOffset, projectileDirection.value.normalized*projectileSpeed.value,projectileCount,projectileAngle,addedProjectileLogicSubs);
                if(ts!=null)
                ShootedProjections.AddRange(ts);

            }else{
                var count = projectileCount.value;
                if(DelayPerProjectile.value*projectileCount.value>duration){
                    count = (int)(duration/DelayPerProjectile.value);
                }
                    
                    
                for (int i = 0; i < count; i++)
                {
                    Action _f = () =>
                    {
                       var ts= shootAbility.ShootProjectile(projectileSO, (Vector2)user.entity.entityCollider.bounds.center+posOffset, projectileDirection.value.normalized*projectileSpeed.value,1,projectileAngle,const_subs);
                          if(ts!=null)
                            ShootedProjections.AddRange(ts);
                    };
                    Timer.SetTimer(DelayPerProjectile.value*i).OnEnd(_f).BindTo(user.entity.gameObject);
                }
            }
            
            timer= Timer.SetTimer(duration).OnUpdate((_,ti) => { progress = ti; }).OnEnd(() => { progress=1; });
            timer.BindTo(user.entity.gameObject);
        }

        public override float Update()
        {
            
            return progress;
        }

        public override void Stop()
        {
            timer?.Stop();
            progress = 0;//!!!!!!!!!!!!!!!!
            
        }
    
        public Func<Projection[]> CreateProjectionsGetter()
        {
            return () => ShootedProjections.ToArray();
        }
    
    }
    
}
