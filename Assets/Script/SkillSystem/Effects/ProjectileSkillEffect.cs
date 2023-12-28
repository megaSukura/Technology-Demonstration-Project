using System;

using UnityEngine;

namespace SkillSystem.SkillEfect
{
    //ProjMov 系列效果用于控制投射物的移动
    //ProjMovSetSkillEffect 用于使用Parameter直接设置投射物的速度
    //ProjMovChangeSkillEffect 用于使用rotation和 speedMultiplier 改变投射物的速度
    //ProjMovControlSkillEffect 用于使用自定义的函数控制投射物的速度
    public class ProjMovSetSkillEffect : SkillEffectBase
    {
        RefParameter<Projection[]> proj;
        Parameter<Vector2> desireVelocity;
        public ProjMovSetSkillEffect(RefParameter<Projection[]> proj, Parameter<Vector2> desireVelocity)
        {
            this.proj = proj;
            this.desireVelocity = desireVelocity;
        }
        public override void Apply(Skill user)
        {
            base.Apply(user);
            if(proj!=null)
            if(proj.value!=null)
            if(desireVelocity!=null)
            foreach (var projection in proj.value)
            {
                if(null!=projection&& projection.isActivated)
                projection.DesireVelocity = desireVelocity.value;
            }
        }
        public override float Update()
        {
            return 0;
        }
        public override void Stop()
        {
            
        }

        
    }
    public class ProjMovChangeSkillEffect : SkillEffectBase
    {
        RefParameter<Projection[]> proj;
        Parameter<float> rotation;
        Parameter<float> speedMultiplier;
        public ProjMovChangeSkillEffect(RefParameter<Projection[]> proj, Parameter<float> rotation, Parameter<float> speedMultiplier)
        {
            this.proj = proj;
            this.rotation = rotation;
            this.speedMultiplier = speedMultiplier;
        }
        public override void Apply(Skill user)
        {
            base.Apply(user);
            if(proj!=null)
            if(proj.value!=null)
            if(rotation!=null)
            if(speedMultiplier!=null)
            foreach (var projection in proj.value)
            {   
                if(null!=projection&& projection.isActivated)
                projection.DesireVelocity = Quaternion.Euler(0,0,rotation.value)*projection.DesireVelocity;
                projection.DesireVelocity *= speedMultiplier.value;
            }
            Debug.Log(proj.value.Length);
        }
        public override float Update()
        {
            return 0;
        }
        public override void Stop()
        {
            
        }

        
    }
    public class ProjMovControlSkillEffect : SkillEffectBase
    {
        RefParameter<Projection[]> proj;
        Func<Projection,Vector2> controller;

        public ProjMovControlSkillEffect(RefParameter<Projection[]> proj, Func<Projection, Vector2> controller)
        {
            this.proj = proj;
            this.controller = controller;
        }
        public override void Apply(Skill user)
        {
            base.Apply(user);
            if(controller!=null)
            if(proj!=null)
            if(proj.value!=null)
            foreach (var projection in proj.value)
            {
                if(null!=projection&& projection.isActivated)
                projection.DesireVelocity = controller(projection);
            }
            //proj.value.DesireVelocity = controller(proj.value);
        }
        public override float Update()
        {
            return 0;
        }
        public override void Stop()
        {
            
        }

        
    }
}