using UnityEngine;
using System;

#region Getter
    
    public class AttributeValueGetter<T>:Getter<T> where T : struct
    {
        public AttributeValueGetter(AttributeValue<T> attribute)
        {
            this.attribute = attribute;
        }
        public AttributeValue<T> attribute;
        public override T Get()
        {
            return attribute.Value;
        }
    }
    
    public class AttributeFloatGetter : Getter<float>
    {
        public AttributeFloatGetter(AttributeFloat attribute)
        {
            this.attribute = attribute;
        }
        public AttributeFloat attribute;
        public override float Get()
        {
            return attribute.PanleValue;
        }
    }
    
    public class GeneralParameterGetter<T> : Getter<T> where T : struct
    {
        public GeneralParameterGetter(Parameter<T> parameter)
        {
            this.parameter = parameter;
        }
        public Parameter<T> parameter;
        public override T Get()
        {
            return parameter.value;
        }
    }
    
    public class CustomEntityGetter<T> : Getter<T> where T : struct
    {
        public CustomEntityGetter(GameEntity entity,Func<GameEntity,T> getter)
        {
            this.entity = entity;
            this.getter = getter;
        }
        public Func<GameEntity,T> getter;
        public GameEntity entity;
        public override T Get()
        {
            return getter.Invoke(entity);
        }
    }
    
    public class EntityGetter<T> : Getter<T> where T : struct
    {
        /// <summary>
        /// 从实体中获取参数的Getter类,注意损耗(装箱)
        /// </summary>
        public EntityGetter(GameEntity entity, string parameterName)
        {
            this.entity = entity;
            this.parameterName = parameterName;
        }
        public GameEntity entity;
        public string parameterName;
        public override T Get()
        {
            return entity.GetParameter<T>(parameterName);
        }
    }
    
    public class EntityFloatGetter : Getter<float>
    {
        /// <summary>
        /// 从实体中获取float参数的Getter类
        /// </summary>
        public EntityFloatGetter(GameEntity entity, string parameterName)
        {
            this.entity = entity;
            this.parameterName = parameterName;
        }
        public GameEntity entity;
        public string parameterName;
        public override float Get()
        {
            return entity.GetFloatParameter(parameterName);
        }
    }
    
    public class EntityVector2Getter : Getter<Vector2>
    {
        /// <summary>
        /// 从实体中获取Vector2参数的Getter类
        /// </summary>
        public EntityVector2Getter(GameEntity entity, string parameterName)
        {
            this.entity = entity;
            this.parameterName = parameterName;
        }
        public GameEntity entity;
        public string parameterName;
        public override Vector2 Get()
        {
            return entity.GetVector2Parameter(parameterName);
        }
    }
    #region Other getter
    public abstract class refGetter<T> : IGetter<T>
    {    
        public abstract T Get();
    }
    public abstract class EntityRefGetter : refGetter<GameEntity>
    {
    }
    public class CloseEntityGetter : EntityRefGetter
    {
        public CloseEntityGetter(GameEntity entity, float range)
        {
            this.entity = entity;
            this.range = range;
        }
        public GameEntity entity;
        public float range;
        private GameEntity _cache;
        private int _cacheFrame;
        public override GameEntity Get()
        {
            if(_cacheFrame==Time.frameCount)
                return _cache;
            _cache= entity.GetClosestEntity(range);
            _cacheFrame = Time.frameCount;
            return _cache;
        }
    }

#endregion
#endregion

#region Trigger
namespace SkillSystem
{

public class SkillEndTrigger : Trigger
{
    public SkillEndTrigger(Skill skill)
    {
        this.skill = skill;
    }
    public Skill skill;
    public override bool Get()
    {
        return skill.IsEnd;
    }
}

public class SkillProgressTrigger : Trigger
{
    public SkillProgressTrigger(Skill skill,float triggerZoneMin,float triggerZoneMax)
    {
        this.skill = skill;
        this.triggerZoneMin = triggerZoneMin;
        this.triggerZoneMax = triggerZoneMax;
    }
    public Skill skill;
    public float triggerZoneMin;
    public float triggerZoneMax;
    public override bool Get()
    {
        return skill.Progress >= triggerZoneMin && skill.Progress <= triggerZoneMax;
    }
}

public class ColdownTrigger : Trigger
{
    public ColdownTrigger(TimerExtend.ColdDown coldDown)
    {
        this.coldDown = coldDown;
    }
    public TimerExtend.ColdDown coldDown;
    public override bool Get()
    {
        return coldDown.coldDown();
    }
}

/// <summary>
/// 按键触发器(按下)
///TODO:自己的输入系统
/// </summary>
public class KeyTrigger : Trigger
{
    public KeyTrigger(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
    public KeyCode keyCode;
    public override bool Get()
    {
        return Input.GetKey(keyCode);
    }
}

/// <summary>
/// 按键触发器(按下)
/// </summary>
public class KeyDownTrigger : Trigger
{
    public KeyDownTrigger(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
    public KeyCode keyCode;
    public override bool Get()
    {
        return Input.GetKeyDown(keyCode);
    }
}

/// <summary>
/// 按键触发器(抬起)
/// </summary>
public class KeyUpTrigger : Trigger
{
    public KeyUpTrigger(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
    public KeyCode keyCode;
    public override bool Get()
    {
        return Input.GetKeyUp(keyCode);
    }

}

/// <summary>
/// input触发器
/// </summary>
public class InputTrigger : Trigger
{
    public InputTrigger(string inputName)
    {
        this.inputName = inputName;
    }
    public string inputName;
    public override bool Get()
    {
        return Input.GetButtonDown(inputName);
    }
}
}

#endregion